using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf
{
    /// <summary>
    /// Extension methods for IServiceHostBuilder&lt;TService&gt;
    /// </summary>
    public static class ServiceBehaviorExtensions
    {
        /// <summary>
        /// Adds service metadata behavior to a service host
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="configuration">The service host configuration</param>
        /// <returns>The service host configuration</returns>
        public static IServiceHostConfiguration<TService> WithServiceMetadataBehavior<TService>(this IServiceHostConfiguration<TService> configuration)
        {
            return configuration.WithServiceMetadataBehavior(behavior =>
            {
                behavior.HttpGetEnabled = true;
                behavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            });
        }

        /// <summary>
        /// Adds service metadata behavior to a service host
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="configuration">The service host configuration</param>
        /// <param name="action">Configuration action for the service metadata behavior</param>
        /// <returns>The service host configuration</returns>
        public static IServiceHostConfiguration<TService> WithServiceMetadataBehavior<TService>(this IServiceHostConfiguration<TService> configuration, Action<ServiceMetadataBehavior> action)
        {
            var behavior = new ServiceMetadataBehavior();
            action(behavior);
            return configuration.WithServiceBehavior(behavior);
        }

        /// <summary>
        /// Add service behavior to a service host
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="configuration">The service host configuration</param>
        /// <param name="behavior">The service behavior</param>
        /// <returns>The service host configuration</returns>
        public static IServiceHostConfiguration<TService> WithServiceBehavior<TService>(this IServiceHostConfiguration<TService> configuration, IServiceBehavior behavior)
        {
            var provider = new ServiceBehaviorProvider<TService>(behavior);
            configuration.Services.AddSingleton<ServiceBehaviorProvider<TService>>(provider);
            return configuration;
        }
    }
}
