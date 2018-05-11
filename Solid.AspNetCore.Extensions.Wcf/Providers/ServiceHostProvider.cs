using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Builders;
using Solid.AspNetCore.Extensions.Wcf.Models;

namespace Solid.AspNetCore.Extensions.Wcf.Providers
{
    internal class ServiceHostProvider<TService> : IServiceHostProvider<TService>
    {
        private Lazy<ServiceHost> _lazyHost;
        private IBaseAddressProvider _baseAddresses;
        private ServiceHostFactoryDelegate<TService> _factory;
        private IEnumerable<IServiceBehavior> _behaviors;
        private IServiceProvider _provider;
        private List<Action<ServiceHost>> _actions;

        public ServiceHostProvider(
            IBaseAddressProvider baseAddresses,
            IEnumerable<IServiceBehavior> globalBehaviors, 
            IEnumerable<ServiceBehaviorProvider<TService>> instanceBehaviors,
            IServiceProvider provider,
            ServiceHostFactoryDelegate<TService> factory = null)
        {
            _baseAddresses = baseAddresses;
            _factory = factory ?? _defaultFactory;
            _behaviors = globalBehaviors.Concat(instanceBehaviors.Select(b => b.Behavior)).ToList();
            _provider = provider;
            _actions = new List<Action<ServiceHost>>();

            _lazyHost = new Lazy<ServiceHost>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public ServiceHost Host => _lazyHost.Value;

        private ServiceHost Initialize()
        {
            var baseAddresses = _baseAddresses.GetBaseAddressesFor<TService>();
            var singleton = default(TService);
            if(typeof(TService).GetServiceLifetime() == ServiceLifetime.Singleton)
                singleton = _provider.GetService<TService>();
            var host = _factory(_provider, singleton, typeof(TService), baseAddresses);
            foreach (var behavior in _behaviors)
                host.Description.Behaviors.Add(behavior);
            foreach (var action in _actions)
                action(host);
            return host;
        }

        public void AddStartupAction(Action<ServiceHost> action)
        {
            _actions.Add(action);
        }

        static readonly ServiceHostFactoryDelegate<TService> _defaultFactory = (provider, singleton, type, baseAddresses) =>
        {
            if (singleton == null)
                return new ServiceHost(type, baseAddresses);
            return new ServiceHost(singleton, baseAddresses);
        };
    }
}
