using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using System.ServiceModel.Description;
using Solid.AspNetCore.Extensions.Wcf.Providers;
using Solid.AspNetCore.Extensions.Wcf.Builders;

namespace Solid.AspNetCore.Extensions.Wcf.Factories
{
    internal class ServiceHostFactory : IServiceHostFactory
    {
        private IServiceProvider _provider;

        public ServiceHostFactory(IServiceProvider provider)
        {
            _provider = provider;         
        }

        public AspNetCoreServiceHost<TService> Create<TService>()
        {
            var type = typeof(TService);
            var host = null as AspNetCoreServiceHost<TService>;
            if (type.GetServiceLifetime() == ServiceLifetime.Singleton)
                host = new AspNetCoreServiceHost<TService>(_provider.GetService<TService>());
            else
                host = new AspNetCoreServiceHost<TService>();

            host.Initializing += AddGlobalBehaviors;
            host.Initializing += (sender, args) => AddInstanceBehaviors<TService>(sender, args);

            return host;
        }

        private void AddGlobalBehaviors(object sender, EventArgs args)
        {
            var host = sender as ServiceHost;
            var behaviors = _provider.GetServices<IServiceBehavior>().ToArray();
            AddBehaviorsToHost(host, behaviors);
        }

        private void AddInstanceBehaviors<TService>(object sender, EventArgs args)
        {
            var host = sender as ServiceHost;
            var behaviors = _provider.GetServices<ServiceBehaviorProvider<TService>>().Select(p => p.Behavior).ToArray();
            AddBehaviorsToHost(host, behaviors);
        }

        private void AddBehaviorsToHost(ServiceHost host, IServiceBehavior[] behaviors)
        {
            foreach (var behavior in behaviors)
            {
                if (typeof(ServiceCredentials).IsAssignableFrom(behavior.GetType()))
                    host.Description.Behaviors.Remove(typeof(ServiceCredentials));

                host.Description.Behaviors.Add(behavior);
            }
        }
    }
}
