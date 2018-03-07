using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Builders
{
    internal class ServiceHostBuilder<TService>: IServiceHostBuilder<TService>
    {
        internal ServiceHostBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
