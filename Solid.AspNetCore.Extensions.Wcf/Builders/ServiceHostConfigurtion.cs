using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal class ServiceHostConfigurtion<TService>: IServiceHostConfiguration<TService>
    {
        internal ServiceHostConfigurtion(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
        public ServiceHostFactoryDelegate<TService> ServiceHostFactory { get; set ; }
    }
}
