using Microsoft.Extensions.Logging;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    internal class AspNetCoreTransportBindingElement : TransportBindingElement
    {
        internal IMessageFactory MessageFactory { get; }

        private ILoggerFactory _loggerFactory;

        internal IAspNetCoreHandler Handler { get; }

        public AspNetCoreTransportBindingElement(IAspNetCoreHandler handler, IMessageFactory factory, ILoggerFactory loggerFactory)
        {
            Handler = handler;
            MessageFactory = factory;

            _loggerFactory = loggerFactory;
        }

        protected AspNetCoreTransportBindingElement(IAspNetCoreHandler handler, IMessageFactory factory, ILoggerFactory loggerFactory, TransportBindingElement elementToBeCloned) 
            : base(elementToBeCloned)
        {
            Handler = handler;
            MessageFactory = factory;

            _loggerFactory = loggerFactory;
        }

        public override string Scheme => "http";

        public override BindingElement Clone()
        {
            return new AspNetCoreTransportBindingElement(Handler, MessageFactory, _loggerFactory, this);
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            return typeof(TChannel) == typeof(IReplyChannel);
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (!CanBuildChannelListener<TChannel>(context))
            {
                throw new ArgumentException(String.Format("Unsupported channel type: {0}.", typeof(TChannel).Name));
            }
            return (IChannelListener<TChannel>)(object)new AspNetCoreChannelListener(this, Handler, MessageFactory, _loggerFactory, context);
        }
    }
}
