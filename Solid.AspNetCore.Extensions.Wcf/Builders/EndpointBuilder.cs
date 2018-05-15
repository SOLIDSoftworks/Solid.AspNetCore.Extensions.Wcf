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

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal class EndpointBuilder<TService> : IEndpointBuilder<TService>
    {
        private IServiceHostProvider<TService> _hostProvider;
        private IServiceProvider _services;
        private IBindingSanitizer _sanitizer;

        public EndpointBuilder(IServiceHostProvider<TService> hostProvider, IServiceProvider services, IBindingSanitizer sanitizer)
        {
            _hostProvider = hostProvider;
            _services = services;
            _sanitizer = sanitizer;
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
                var endpoint = host.AddServiceEndpoint(typeof(TContract), _sanitizer.SanitizeBinding(binding), path);
                action(_services, endpoint);
            });
            return this;
        }
    }
}
