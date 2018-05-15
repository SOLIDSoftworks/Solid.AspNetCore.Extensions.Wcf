using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    internal class AspNetCoreReplyChannel : AsyncReplyChannelBase
    {
        private IAspNetCoreChannel _channel;
        private ILogger<AspNetCoreReplyChannel> _logger;

        public AspNetCoreReplyChannel(AspNetCoreChannelListener channelManager, IAspNetCoreChannel channel) 
            : base(channelManager)
        {
            _channel = channel;
            _logger = channelManager.LoggerFactory.CreateLogger<AspNetCoreReplyChannel>();
            Listener = channelManager;

            _logger.LogDebug($"Reply channel created using AspNetCoreChannel {_channel.Id} for {channel.BaseAddress}");
        }

        public AspNetCoreChannelListener Listener { get; }

        public override EndpointAddress LocalAddress => new EndpointAddress(_channel.BaseAddress);

        protected override Task OnCloseAsync(TimeSpan timeout)
        {
            return Task.CompletedTask;
        }

        protected override Task OnOpenAsync(TimeSpan timeout)
        {
            return Task.CompletedTask;
        }

        public override Task<RequestContext> ReceiveRequestAsync()
        {
            return ReceiveRequestAsync(DefaultReceiveTimeout);
        }

        public override async Task<RequestContext> ReceiveRequestAsync(TimeSpan timeout)
        {
            var http = await _channel.ReceiveAsync();
            _logger.LogDebug($"Received request on AspNetCoreChannel {_channel.Id}");
            var message = await Listener.MessageFactory.CreateMessageAsync(http, Listener.BindingContext);
            return new AspNetCoreRequestContext(_channel, Listener.BindingContext, message, http);
        }

        protected override void OnAbort()
        {
        }

        public override Task<bool> WaitForRequestAsync(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}