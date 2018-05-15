using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Behaviors;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Middleware;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host
{
    public class BehaviorTestStartup
    {
        public static Mock<UserNamePasswordValidator> MockUserNamePasswordValidator;

        static BehaviorTestStartup()
        {
            MockUserNamePasswordValidator = new Mock<UserNamePasswordValidator>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var binding = new WS2007HttpBinding(SecurityMode.Message);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            binding.Security.Message.EstablishSecurityContext = false;
            services.AddLogging(builder =>
            {
                builder
                    .AddDebug()                    
                    .SetMinimumLevel(LogLevel.Debug);
            });
            services.AddSingleton<UserNamePasswordValidator>(MockUserNamePasswordValidator.Object);
            services.AddSingleton<IServiceBehavior, UserNamePasswordValidatorBehavior>();
            services.AddSingleton<IServiceBehavior, OutputBehavior>();
            services.AddWcfServiceWithMetadata<SingletonService>().AddDefaultBinding(binding);
        }

        public void Configure(IApplicationBuilder builder)
        {
            builder
                .UseMiddleware<OutputMiddleware>()
                .UseWcfService<SingletonService, IInstanceTestService>("/singleton");
        }
    }
}
