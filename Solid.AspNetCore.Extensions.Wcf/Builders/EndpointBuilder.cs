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

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal class EndpointBuilder<TService> : IEndpointBuilder<TService>
    {
        private AspNetCoreServiceHost<TService> _host;
        private IServiceProvider _services;

        public EndpointBuilder(AspNetCoreServiceHost<TService> host, IServiceProvider services)
        {
            _host = host;
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
            _host.AddEndpoint<TContract>(binding, path);
            return this;
        }
    }
}
