using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels.AspNetCore
{
    internal class AspNetCoreHandler : IAspNetCoreHandler
    {
        private readonly ConcurrentDictionary<string, IAspNetCoreChannel> _channels = new ConcurrentDictionary<string, IAspNetCoreChannel>();
        private ILoggerFactory _loggerFactory;
        private ILogger<AspNetCoreHandler> _logger;

        public AspNetCoreHandler(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<AspNetCoreHandler>();
        }

        public async Task<IAspNetCoreChannel> OpenAsync(Uri baseAddress, string method = "POST")
        {
            _logger.LogDebug($"Opening AspNetCoreHandler for {method} - {baseAddress}");
            var url = ConvertToRelative(baseAddress);
            var key = GenerateKey(method, url);
            var channel = _channels.GetOrAdd(key, uri => new AspNetCoreChannel(url, _loggerFactory.CreateLogger<AspNetCoreChannel>()));
            await channel.OpenAsync();
            return channel;
        }

        public Task CloseAsync(Guid channelId)
        {
            var channel = _channels.Values.FirstOrDefault(c => c.Id == channelId);
            return channel?.CloseAsync() ?? Task.CompletedTask;
        }

        public Task<bool> CanHandleAsync(HttpContext context)
        {
            var url = context.Request.GetRequestUri();
            var key = GenerateKey(context.Request.Method, url);
            return Task.FromResult(_channels.ContainsKey(key));
        }

        public async Task HandleAsync(HttpContext context)
        {
            var url = context.Request.GetRequestUri();
            _logger.LogDebug($"Received request for {context.Request.Method} - {url}");
            var key = GenerateKey(context.Request.Method, url);
            var channel = _channels[key];
            await channel.HandleAsync(context);
        }

        private Uri ConvertToRelative(Uri url)
        {
            var path = url.AbsolutePath;
            if (path.Length > 1)
                path = path.TrimEnd('/');
            var query = url.Query;
            return new Uri(path + query, UriKind.Relative);
        }

        private string GenerateKey(string method, Uri url)
        {
            if(url.IsAbsoluteUri)
                url = ConvertToRelative(url);
            return $"{method}_{url}".ToLower();
        }

        class AspNetCoreChannel : IAspNetCoreChannel
        {
            private BufferBlock<HttpContext> _buffer = new BufferBlock<HttpContext>();
            private ConcurrentDictionary<string, TaskCompletionSource<HttpContext>> _requests = new ConcurrentDictionary<string, TaskCompletionSource<HttpContext>>();
            public AspNetCoreChannel(Uri baseAddress, ILogger<AspNetCoreChannel> logger)
            {
                BaseAddress = baseAddress;
                Id = Guid.NewGuid();
                _logger = logger;
                _logger.LogDebug($"AspNetCoreChannel created");
            }

            public Guid Id { get; }

            private ILogger<AspNetCoreChannel> _logger;

            public Uri BaseAddress { get; }

            public Task CloseAsync()
            {
                return Task.CompletedTask;
            }

            public async Task HandleAsync(HttpContext context)
            {
                var source = _requests.GetOrAdd(context.TraceIdentifier, id => new TaskCompletionSource<HttpContext>());
                _logger.LogDebug($"Enqueuing HttpContext for {context.Request.Method} - {context.Request.GetRequestUri()} ({context.TraceIdentifier})");
                await _buffer.SendAsync(context);
                await source.Task;
            }

            public Task OpenAsync()
            {
                return Task.CompletedTask;
            }

            public async Task<HttpContext> ReceiveAsync()
            {
                var context = await _buffer.ReceiveAsync();
                _logger.LogDebug($"Dequeuing HttpContext for {context.Request.Method} - {context.Request.GetRequestUri()} ({context.TraceIdentifier})");
                return context;
            }

            public Task ReplyAsync(HttpContext context)
            {
                var source = _requests.GetOrAdd(context.TraceIdentifier, id => new TaskCompletionSource<HttpContext>());
                _logger.LogDebug($"Replying to {context.Request.Method} - {context.Request.GetRequestUri()} ({context.TraceIdentifier})");
                source.SetResult(context);
                return Task.CompletedTask;
            }
        }
    }
}
