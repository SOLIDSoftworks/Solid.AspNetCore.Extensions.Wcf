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

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    internal class AspNetCoreReplyChannel<TService> : AsyncReplyChannelBase
    {
        private IAspNetCoreChannel _channel;

        public AspNetCoreReplyChannel(AspNetCoreChannelListener<TService> channelManager, IAspNetCoreChannel channel) 
            : base(channelManager.BindingContext.Binding)
        {
            _channel = channel;
            Listener = channelManager;
        }

        public AspNetCoreChannelListener<TService> Listener { get; }

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
            return ReceiveRequestAsync(ReceiveTimeout);
        }

        public override async Task<RequestContext> ReceiveRequestAsync(TimeSpan timeout)
        {
            var http = await _channel.ReceiveAsync();
            var message = await Listener.MessageFactory.CreateMessageAsync(http, Listener.BindingContext);
            return new AspNetCoreRequestContext<TService>(_channel, Listener.BindingContext, message, http);
        }

        protected override void OnAbort()
        {
        }

        protected override Task<bool> OnWaitForChannelAsync(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> WaitForRequestAsync(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
    }
}