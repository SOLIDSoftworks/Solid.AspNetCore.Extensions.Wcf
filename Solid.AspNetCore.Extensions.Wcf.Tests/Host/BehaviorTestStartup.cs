using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Behaviors;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services;
using System;
using System.Collections.Generic;
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

            services.AddSingleton<UserNamePasswordValidator>(MockUserNamePasswordValidator.Object);
            services.AddSingleton<IServiceBehavior, UserNamePasswordValidatorBehavior>();
            services.AddWcfServiceWithMetadata<SingletonService>().AddDefaultBinding(binding);
        }

        public void Configure(IApplicationBuilder builder)
        {
            builder.UseWcfService<SingletonService, IInstanceTestService>("/singleton");
        }
    }
}
