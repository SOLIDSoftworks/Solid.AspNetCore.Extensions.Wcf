using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceModel;
using System.ServiceModel.Description;
using Solid.AspNetCore.Extensions.Wcf.Channels;
using Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore;

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal class EndpointBuilder<TService> : IEndpointBuilder<TService>
    {
        private IServiceHostProvider<TService> _hostProvider;
        private IServiceProvider _services;

        public EndpointBuilder(IServiceHostProvider<TService> hostProvider, IServiceProvider services)
        {
            _hostProvider = hostProvider;
            _services = services;
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>()
        {            
            return AddServiceEndpoint<TContract>(string.Empty);
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(string path)
        {
            var binding = _services.GetService<Binding>();
            return AddServiceEndpoint<TContract>(binding, path);
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding)
        {
            return AddServiceEndpoint<TContract>(binding, string.Empty);
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding, string path)
        {
            return AddServiceEndpoint<TContract>(binding, path, (_1, _2) => { });
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Action<IServiceProvider, ServiceEndpoint> action)
        {
            var binding = _services.GetService<Binding>();
            return AddServiceEndpoint<TContract>(binding, action);
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(string path, Action<IServiceProvider, ServiceEndpoint> action)
        {
            var binding = _services.GetService<Binding>();
            return AddServiceEndpoint<TContract>(binding, path, action);
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding, Action<IServiceProvider, ServiceEndpoint> action)
        {
            return AddServiceEndpoint<TContract>(binding, string.Empty, action);
        }

        public IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding, string path, Action<IServiceProvider, ServiceEndpoint> action)
        {
            _hostProvider.AddStartupAction(host =>
            {
                var endpoint = host.AddServiceEndpoint(typeof(TContract), SanitizeBinding(binding), path);
                action(_services, endpoint);
            });
            return this;
        }

        private Binding SanitizeBinding(Binding dirty)
        {
            var custom = dirty as CustomBinding;
            if (custom == null)
                custom = new CustomBinding(dirty);

            var security = custom.Elements.OfType<SecurityBindingElement>();
            foreach (var element in security)
                element.AllowInsecureTransport = true;

            Replace<HttpTransportBindingElement>(custom);
            Replace<HttpsTransportBindingElement>(custom);

            return custom;
        }

        private void Replace<TTransport>(CustomBinding custom)
            where TTransport : TransportBindingElement
        {
            var current = custom.Elements.OfType<TTransport>().FirstOrDefault();
            if (current != null)
            {
                custom.Elements.Remove(current);
                //custom.Elements.Add(new HttpTransportBindingElement());
                var handler = _services.GetService<IAspNetCoreHandler>();
                var factory = _services.GetService<IMessageFactory>();
                var aspNetCore = new AspNetCoreTransportBindingElement<TService>(handler, factory);
                custom.Elements.Add(aspNetCore);
            }
        }
    }
}
