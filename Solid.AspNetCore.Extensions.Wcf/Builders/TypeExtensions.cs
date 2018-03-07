using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal static class TypeExtensions
    {
        public static ServiceLifetime GetServiceLifetime(this Type serviceType)
        {
            var behavior = serviceType.GetCustomAttributes(true).OfType<ServiceBehaviorAttribute>().FirstOrDefault();
            if (behavior == null || behavior.InstanceContextMode != InstanceContextMode.Single) return ServiceLifetime.Scoped;
            return ServiceLifetime.Singleton;
        }
    }
}
