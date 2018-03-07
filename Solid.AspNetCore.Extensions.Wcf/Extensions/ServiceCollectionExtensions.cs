using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        //public static void TryAddSingleton<TService, TImplementation>(this IServiceCollection services, IEqualityComparer<ServiceDescriptor> comparer)
        //    where TService : class
        //    where TImplementation : class, TService
        //{
        //    var descriptor = ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        //    if (services.Any(d => comparer.Equals(descriptor, d))) return;
        //    services.Add(descriptor);
        //}

        public static void TryAddTransient<TService, TImplementation>(this IServiceCollection services, IEqualityComparer<ServiceDescriptor> comparer)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptor = ServiceDescriptor.Describe(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
            if (services.Any(d => comparer.Equals(descriptor, d))) return;
            services.Add(descriptor);
        }
    }
}
