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
    internal class AspNetCoreHandler : IAspNetCoreHandler
    {
        private readonly ConcurrentDictionary<Uri, IAspNetCoreChannel> _channels = new ConcurrentDictionary<Uri, IAspNetCoreChannel>();

        public async Task<IAspNetCoreChannel> OpenAsync(Uri baseAddress)
        {
            var channel = _channels.GetOrAdd(baseAddress, uri => new AspNetCoreChannel(uri));
            await channel.OpenAsync();
            return channel;
        }

        public Task CloseAsync(Uri baseAddress)
        {
            var channel = _channels.GetOrAdd(baseAddress, uri => new AspNetCoreChannel(uri));
            return channel.CloseAsync();
        }

        public Task<bool> CanHandleAsync(HttpContext context)
        {
            var url = context.Request.GetRequestUri();
            return Task.FromResult(_channels.ContainsKey(url));
        }

        public async Task HandleAsync(HttpContext context)
        {
            var url = context.Request.GetRequestUri();
            var channel = _channels[url];
            await channel.HandleAsync(context);
        }

        class AspNetCoreChannel : IAspNetCoreChannel
        {
            private SemaphoreSlim _lock = new SemaphoreSlim(1);
            private BufferBlock<HttpContext> _buffer = new BufferBlock<HttpContext>();
            private ConcurrentDictionary<string, TaskCompletionSource<HttpContext>> _requests = new ConcurrentDictionary<string, TaskCompletionSource<HttpContext>>();
            public AspNetCoreChannel(Uri baseAddress)
            {
                BaseAddress = baseAddress;
            }

            public Uri BaseAddress { get; }

            public Task CloseAsync()
            {
                _lock.Release();
                return Task.CompletedTask;
            }

            public async Task HandleAsync(HttpContext context)
            {
                var source = _requests.GetOrAdd(context.TraceIdentifier, id => new TaskCompletionSource<HttpContext>());
                await _buffer.SendAsync(context);
                await source.Task;
            }

            public async Task OpenAsync()
            {
                await _lock.WaitAsync();
            }

            public async Task<HttpContext> ReceiveAsync()
            {
                return await _buffer.ReceiveAsync();
            }

            public Task ReplyAsync(HttpContext context)
            {
                var source = _requests.GetOrAdd(context.TraceIdentifier, id => new TaskCompletionSource<HttpContext>());
                source.SetResult(context);
                return Task.CompletedTask;
            }
        }
    }
}
