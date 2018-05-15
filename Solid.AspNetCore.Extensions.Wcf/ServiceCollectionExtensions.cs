using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Behaviors;
using Solid.AspNetCore.Extensions.Wcf.Builders;
using Solid.AspNetCore.Extensions.Wcf.Extensions;
using Solid.AspNetCore.Extensions.Wcf.Factories;
using Solid.AspNetCore.Extensions.Wcf.Models;
using Solid.AspNetCore.Extensions.Wcf.Providers;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels.AspNetCore;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel.Description;
using Solid.AspNetCore.Extensions.Wcf.Services;
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
        /// <returns>The service collection</returns>
        public static IServiceCollection AddWcfService<TService>(this IServiceCollection services)
            where TService : class
        {
            return services.AddWcfService<TService>(_ => { });
        }

        /// <summary>
        /// Adds the specified service implementation to the service collection and creates a service host for the specified service.
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="action">Extra configuration actions</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddWcfService<TService>(this IServiceCollection services, Action<IServiceHostConfiguration<TService>> action)
            where TService : class
        {
            var type = typeof(TService);
            services
                .TryAddServiceModel()
                .AddSingleton(p => p.GetService<IServiceHostProvider<TService>>().Host)
                .Add(ServiceDescriptor.Describe(type, type, type.GetServiceLifetime()));

            var builder = new ServiceHostConfigurtion<TService>(services);
            action(builder);

            if (builder.ServiceHostFactory != null)
                services.AddSingleton(builder.ServiceHostFactory);

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

        /// <summary>
        /// Adds a default binding type to the service collection for service endpoints
        /// <para>This binding is bound using Binding as the service type</para>
        /// </summary>
        /// <typeparam name="TBinding">The default binding type</typeparam>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddDefaultBinding<TBinding>(this IServiceCollection services)
            where TBinding : Binding
        {
            var description = services.FirstOrDefault(s => s.ServiceType == typeof(Binding));
            if (description != null)
                services.Remove(description);
            return services.AddTransient<Binding, TBinding>();
        }

        /// <summary>
        /// Adds a default binding factory to the service collection for service endpoints
        /// <para>This binding is bound using Binding as the service type</para>
        /// </summary>
        /// <typeparam name="TBinding">The default binding type</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="factory">The factory method that creates the binding</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddDefaultBinding<TBinding>(this IServiceCollection services, Func<IServiceProvider, TBinding> factory)
            where TBinding : Binding
        {
            var description = services.FirstOrDefault(s => s.ServiceType == typeof(Binding));
            if (description != null)
                services.Remove(description);
            return services.AddTransient<Binding>(factory);
        }

        /// <summary>
        /// Adds a singleton default binding to the service collection for service endpoints
        /// <para>This binding is bound using Binding as the service type</para>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="binding">The default binding instance</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddDefaultBinding(this IServiceCollection services, Binding binding)
        {
            var description = services.FirstOrDefault(s => s.ServiceType == typeof(Binding));
            if (description != null)
                services.Remove(description);
            return services.AddSingleton<Binding>(binding);
        }

        private static IServiceCollection TryAddServiceModel(this IServiceCollection services)
        {
            var comparer = new ServiceDescriptorImplementationTypeEqualityComparer();

            services.TryAddSingleton<IMessageFactory, DefaultMessageFactory>();
            services.TryAddTransient<Binding, BasicHttpBinding>();
            services.TryAddSingleton<IAspNetCoreHandler, AspNetCoreHandler>();
            services.TryAddSingleton<IBindingSanitizer, BindingSanitizer>();
            services.TryAddSingleton(typeof(IServiceHostProvider<>), typeof(ServiceHostProvider<>));
            services.TryAddSingleton<IBaseAddressProvider, BaseAddressProvider>();
            services.TryAddTransient<IServiceBehavior, AspNetCoreInstanceProviderBehavior>(comparer);
            services.TryAddTransient<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>(comparer);
            services.TryAddTransient<IServiceBehavior, MatchAnyAddressServiceBehavior>(comparer);
            services.TryAddTransient<IServiceBehavior, AspNetCoreServiceMetadataBehavior>(comparer);
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
