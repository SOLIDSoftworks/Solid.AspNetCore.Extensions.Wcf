using Solid.AspNetCore.Extensions.Wcf.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel.Description
{
    internal class AspNetCoreInstanceProviderBehavior : IServiceBehavior
    {
        private IServiceProvider _provider;

        public AspNetCoreInstanceProviderBehavior(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var endpoint in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>().SelectMany(dispatcher => dispatcher.Endpoints))
                endpoint.DispatchRuntime.InstanceProvider = new AspNetCoreInstanceProvider(_provider, serviceDescription.ServiceType);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
