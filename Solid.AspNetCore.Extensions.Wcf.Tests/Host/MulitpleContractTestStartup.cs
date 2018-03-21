using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host
{
    public class MulitpleContractTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWcfService<MultipleContract>();
        }

        public void Configure(IApplicationBuilder builder)
        {
            builder.UseWcfService<MultipleContract>("/multiple", b => b.AddServiceEndpoint<IEchoContract>("/echo").AddServiceEndpoint<ICounterContract>("counter"));
        }
    }
}
