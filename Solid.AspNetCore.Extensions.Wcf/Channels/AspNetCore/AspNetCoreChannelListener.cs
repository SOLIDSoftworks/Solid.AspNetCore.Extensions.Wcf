using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    class AspNetCoreChannelListener : AsyncChannelListenerBase<IReplyChannel>
    {
        private SemaphoreSlim _lock = new SemaphoreSlim(1);
        private ILogger<AspNetCoreChannelListener> _logger;
        private IAspNetCoreChannel _channel;

        public AspNetCoreChannelListener(AspNetCoreTransportBindingElement bindingElement, IAspNetCoreHandler handler, IMessageFactory messageFactory, ILoggerFactory loggerFactory, BindingContext context) 
            : base(context.Binding, new Uri(context.ListenUriBaseAddress, context.ListenUriRelativeAddress))
        {
            Id = Guid.NewGuid();
            BindingElement = bindingElement;
            Handler = handler;
            MessageFactory = messageFactory;
            BindingContext = context;

            LoggerFactory = loggerFactory;

            _logger = LoggerFactory.CreateLogger<AspNetCoreChannelListener>();
        }
        public Guid Id { get; }
        public AspNetCoreTransportBindingElement BindingElement { get; }
        public IAspNetCoreHandler Handler { get; }
        public IMessageFactory MessageFactory { get; }
        public BindingContext BindingContext { get; }
        public ILoggerFactory LoggerFactory { get; }

        public bool IsGet { get; set; }

        protected override async Task<IReplyChannel> OnAcceptChannelAsync(TimeSpan timeout)
        {
            _logger.LogDebug($"Listener accepting channel ({Id})");
            await _lock.WaitAsync();
            var reply = new AspNetCoreReplyChannel(this, _channel);
            return reply;
        }

        protected override async Task OnCloseAsync(TimeSpan timeout)
        {
            await Handler.CloseAsync(_channel.Id);
            _logger.LogDebug($"Listener closed ({Id})");
        }

        protected override async Task OnOpenAsync(TimeSpan timeout)
        {
            var method = IsGet ? "GET" : "POST";
            var channel = await Handler.OpenAsync(Uri, method);
            _logger.LogDebug($"Listener opened ({Id})");
            _channel = channel;
        }

        protected override Task<bool> OnWaitForChannelAsync(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        protected override void OnAbort()
        {
        }
    }
}
