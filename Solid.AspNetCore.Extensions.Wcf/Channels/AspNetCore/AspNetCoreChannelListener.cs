using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    class AspNetCoreChannelListener<TService> : AsyncChannelListenerBase<IReplyChannel>
    {
        Guid Id = Guid.NewGuid();

        public AspNetCoreChannelListener(AspNetCoreTransportBindingElement<TService> bindingElement, IAspNetCoreHandler handler, IMessageFactory messageFactory, BindingContext context) 
            : base(context.Binding, new Uri(context.ListenUriBaseAddress, context.ListenUriRelativeAddress))
        {
            BindingElement = bindingElement;
            Handler = handler;
            MessageFactory = messageFactory;
            BindingContext = context;
        }

        public AspNetCoreTransportBindingElement<TService> BindingElement { get; }
        public IAspNetCoreHandler Handler { get; }
        public IMessageFactory MessageFactory { get; }
        public BindingContext BindingContext { get; }


        protected override async Task<IReplyChannel> OnAcceptChannelAsync(TimeSpan timeout)
        {
            var channel = await Handler.OpenAsync(Uri);
            var reply = new AspNetCoreReplyChannel<TService>(this, channel);
            return reply;
        }

        protected override async Task OnCloseAsync(TimeSpan timeout)
        {
            await Handler.CloseAsync(Uri);
        }

        protected override Task OnOpenAsync(TimeSpan timeout)
        {
            return Task.CompletedTask;
        }

        protected override Task<bool> OnWaitForChannelAsync(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
        protected override void OnAbort()
        {
        }

        public override T GetProperty<T>()
        {
            var property =  BindingElement.GetProperty<T>(BindingContext);
            return property;
        }
    }
}
