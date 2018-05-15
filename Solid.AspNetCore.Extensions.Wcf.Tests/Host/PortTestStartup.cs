using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Behaviors;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Middleware;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host
{
    public class PortTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IServiceBehavior, OutputBehavior>();
            services.AddWcfServiceWithMetadata<ProxiedService>();
            //services.AddWcfServiceWithMetadata<DirectService>();
        }

        public void Configure(IApplicationBuilder builder)
        {
            builder
                .UseMiddleware<OutputMiddleware>()
                .UseWcfService<ProxiedService, IProxiedService>("/proxied");
            //builder.UseWcfService<DirectService, IDirectService>("/direct", true);
        }
    }
}
