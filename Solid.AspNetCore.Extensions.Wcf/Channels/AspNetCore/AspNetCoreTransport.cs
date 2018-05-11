using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    internal class AspNetCoreTransport<TService> : IAspNetCoreTransport<TService>
    {
        private SemaphoreSlim _lock = new SemaphoreSlim(1);

        private ConcurrentDictionary<string, TaskCompletionSource<HttpContext>> _requests;
        private BufferBlock<HttpContext> _buffer;
        private PathString _path = null;

        public AspNetCoreTransport()
        {
            _requests = new ConcurrentDictionary<string, TaskCompletionSource<HttpContext>>();
            _buffer = new BufferBlock<HttpContext>();
        }

        public async Task AcceptAsync()
        {
            await _lock.WaitAsync();
        }

        public bool CanHandleRequest(HttpContext context)
        {
            if (_path == null) return false;
            return context.Request.Path.StartsWithSegments(_path);
        }

        public void EndAccept()
        {
            _lock.Release();
        }

        public async Task HandleRequestAsync(HttpContext context)
        {
            var source = _requests.GetOrAdd(context.TraceIdentifier, s => new TaskCompletionSource<HttpContext>());
            await _buffer.SendAsync(context);
            await source.Task;
        }

        public void Initialize(PathString basePath)
        {
            _path = basePath;
        }

        public async Task<HttpContext> RecieveAsync()
        {
            return await _buffer.ReceiveAsync();            
        }

        public Task ReplyAsync(HttpContext context)
        {
            var source = null as TaskCompletionSource<HttpContext>;
            if (!_requests.TryRemove(context.TraceIdentifier, out source))
                throw new ArgumentException("Unknown HttpContext");

            source.SetResult(context);

            return Task.CompletedTask;
        }
    }
}
