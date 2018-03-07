using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IServiceHostFactory
    {
        AspNetCoreServiceHost<TService> Create<TService>();
    }
}
