using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Providers
{
    internal class AspNetCoreInstanceProvider : IInstanceProvider
    {
        private IServiceProvider _provider;
        private Type _type;

        public AspNetCoreInstanceProvider(IServiceProvider provider, Type serviceType)
        {
            _provider = provider;
            _type = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var scope = new InstanceScope(_provider);
            instanceContext.Extensions.Add(scope);
            var instance = scope.ServiceProvider.GetService(_type);
            return instance;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var scope = instanceContext.Extensions.OfType<InstanceScope>().FirstOrDefault();
            scope?.Dispose();
        }
    }
}
