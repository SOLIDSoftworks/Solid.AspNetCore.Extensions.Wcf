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
    public static class ServiceBehaviorExtensions
    {
        public static IServiceHostBuilder<TService> WithServiceMetadataBehavior<TService>(this IServiceHostBuilder<TService> configuration)
        {
            return configuration.WithServiceMetadataBehavior(behavior =>
            {
                behavior.HttpGetEnabled = true;
                behavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            });
        }

        public static IServiceHostBuilder<TService> WithServiceMetadataBehavior<TService>(this IServiceHostBuilder<TService> configuration, Action<ServiceMetadataBehavior> action)
        {
            var behavior = new ServiceMetadataBehavior();
            action(behavior);
            return configuration.WithServiceBehavior(behavior);
        }

        public static IServiceHostBuilder<TService> WithServiceBehavior<TService>(this IServiceHostBuilder<TService> configuration, IServiceBehavior behavior)
        {
            var provider = new ServiceBehaviorProvider<TService>(behavior);
            configuration.Services.AddSingleton<ServiceBehaviorProvider<TService>>(provider);
            return configuration;
        }
    }
}
