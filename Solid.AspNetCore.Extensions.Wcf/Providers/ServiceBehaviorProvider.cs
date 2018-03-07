using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Providers
{
    internal class ServiceBehaviorProvider<TService>
    {
        public ServiceBehaviorProvider(IServiceBehavior behavior)
        {
            Behavior = behavior;
        }
        public IServiceBehavior Behavior { get; }
    }
}
