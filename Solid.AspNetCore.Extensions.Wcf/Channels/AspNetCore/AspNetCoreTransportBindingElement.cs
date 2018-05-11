using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    internal class AspNetCoreTransportBindingElement<TService> : TransportBindingElement
    {
        internal IMessageFactory MessageFactory { get; }
        internal IAspNetCoreHandler Handler { get; }

        public AspNetCoreTransportBindingElement(IAspNetCoreHandler handler, IMessageFactory factory)
        {
            Handler = handler;
            MessageFactory = factory;
        }

        protected AspNetCoreTransportBindingElement(IAspNetCoreHandler handler, IMessageFactory factory, TransportBindingElement elementToBeCloned) 
            : base(elementToBeCloned)
        {
            Handler = handler;
            MessageFactory = factory;
        }

        public override string Scheme => "http";

        public override BindingElement Clone()
        {
            return new AspNetCoreTransportBindingElement<TService>(Handler, MessageFactory, this);
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
            return (IChannelListener<TChannel>)(object)new AspNetCoreChannelListener<TService>(this, Handler, MessageFactory, context);
        }
    }
}
