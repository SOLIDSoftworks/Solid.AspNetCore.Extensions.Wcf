using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Builders;
using Solid.AspNetCore.Extensions.Wcf.Extensions;
using Solid.AspNetCore.Extensions.Wcf.Factories;
using Solid.AspNetCore.Extensions.Wcf.Providers;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf
{
    /// <summary>
    /// Extension methods for IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the specified service implementation to the service collection and creates a service host for the specified service.
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="action">Extra configuration actions</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddWcfService<TService>(this IServiceCollection services, Action<IServiceHostBuilder<TService>> action)
            where TService : class
        {
            var type = typeof(TService);
            services
                .TryAddServiceModel()
                .AddSingleton(p => p.GetService<IServiceHostFactory>().Create<TService>())
                .Add(ServiceDescriptor.Describe(type, type, type.GetServiceLifetime()));

            var builder = new ServiceHostBuilder<TService>(services);
            action(builder);
            return services;
        }

        /// <summary>
        /// Adds the specified service implementation to the service collection and creates a service host with metadata for the specified service.
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddWcfServiceWithMetadata<TService>(this IServiceCollection services)
            where TService : class
        {
            return services.AddWcfService<TService>(builder => builder.WithServiceMetadataBehavior());
        }
        
        private static IServiceCollection TryAddServiceModel(this IServiceCollection services)
        {
            var comparer = new ServiceDescriptorImplementationTypeEqualityComparer();

            services.TryAddTransient<Binding, BasicHttpBinding>();
            services.TryAddSingleton<IServiceHostFactory, ServiceHostFactory>();
            services.TryAddSingleton<IBaseAddressFactory, BaseAddressFactory>();
            services.TryAddTransient<IServiceBehavior, AspNetCoreInstanceProviderBehavior>(comparer);
            services.TryAddTransient<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>(comparer);
            services.TryAddSingleton(typeof(EndpointBuilder<>));

            return services;
        }
    }

    class ServiceDescriptorImplementationTypeEqualityComparer : IEqualityComparer<ServiceDescriptor>
    {
        public bool Equals(ServiceDescriptor x, ServiceDescriptor y)
        {
            if (x == null || y == null) return false;
            return x.ImplementationType == y.ImplementationType;
        }

        public int GetHashCode(ServiceDescriptor obj)
        {
            return obj.GetHashCode();
        }
    }
}
