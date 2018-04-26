﻿using Microsoft.AspNetCore.Http;
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

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal class EndpointBuilder<TService> : IEndpointBuilder<TService>
    {
        private ServiceHost _host;
        private IServiceProvider _services;

        public EndpointBuilder(IServiceHostProvider<TService> hostProvider, IServiceProvider services)
        {
            _host = hostProvider.Host;
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
            var endpoint = _host.AddServiceEndpoint(typeof(TContract), SanitizeBinding(binding), path);
            action(_services, endpoint);
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

            var https = custom.Elements.OfType<TransportBindingElement>().Where(e => e.Scheme == "https").FirstOrDefault();
            if (https != null)
            {
                var http = new HttpTransportBindingElement();
                custom.Elements.Remove(https);
                custom.Elements.Add(http);
            }
            return custom;
        }
    }
}
