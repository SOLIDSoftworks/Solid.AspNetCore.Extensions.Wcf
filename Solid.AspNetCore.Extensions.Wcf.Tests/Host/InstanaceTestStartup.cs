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
    public class InstanaceTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWcfServiceWithMetadata<PerCallService>();
            services.AddWcfServiceWithMetadata<SingletonService>();
            services.AddSingleton<IServiceBehavior, OutputBehavior>();
        }

        public void Configure(IApplicationBuilder builder)
        {
            builder
                .UseMiddleware<OutputMiddleware>()
                .UseWcfService<PerCallService, IInstanceTestService>("/percall")
                .UseWcfService<SingletonService, IInstanceTestService>("/singleton");
        }
    }
}
