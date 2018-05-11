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

namespace Solid.AspNetCore.Extensions.Wcf.Behaviors
{
    internal class AspNetCoreListenerBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            for(var i = serviceHostBase.ChannelDispatchers.Count - 1; i >= 0; i++)
            {
                var dispatcher = serviceHostBase.ChannelDispatchers[i] as ChannelDispatcher;
                if (dispatcher == null) continue;
                if (!dispatcher.BindingName.StartsWith("ServiceMetadata")) continue;

                serviceHostBase.ChannelDispatchers.RemoveAt(i);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
