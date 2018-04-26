using Solid.AspNetCore.Extensions.Wcf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IServiceHostFactoryDelegateFactory
    {
        ServiceHostFactoryDelegate<TService> Create<TService, TServiceHost>()
            where TServiceHost : ServiceHost;
    }
}
