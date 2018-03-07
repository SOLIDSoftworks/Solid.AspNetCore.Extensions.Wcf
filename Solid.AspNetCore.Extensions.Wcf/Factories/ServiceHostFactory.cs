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

            AddGlobalBehaviors(host);
            AddInstanceBehaviors(host);
            //EnsureSingleInstanceContextMode(host);

            return host;
        }

        private void AddGlobalBehaviors<TService>(AspNetCoreServiceHost<TService> host)
        {
            host.DescriptionInitialized += (sender, args) =>
            {
                var behaviors = _provider.GetServices<IServiceBehavior>();
                foreach (var behavior in behaviors)
                    host.Description.Behaviors.Add(behavior);
            };
        }

        private void AddInstanceBehaviors<TService>(AspNetCoreServiceHost<TService> host)
        {
            host.DescriptionInitialized += (sender, args) =>
            {
                var behaviorProviders = _provider.GetServices<ServiceBehaviorProvider<TService>>();
                foreach (var provider in behaviorProviders)
                    host.Description.Behaviors.Add(provider.Behavior);
            };
        }

        private void EnsureSingleInstanceContextMode<TService>(AspNetCoreServiceHost<TService> host)
        {
            host.DescriptionInitialized += (sender, args) =>
            {
                var behaviors = host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().Where(b => b.InstanceContextMode != InstanceContextMode.Single);
                foreach (var behavior in behaviors)
                    behavior.InstanceContextMode = InstanceContextMode.Single;
            };
        }
    }
}
