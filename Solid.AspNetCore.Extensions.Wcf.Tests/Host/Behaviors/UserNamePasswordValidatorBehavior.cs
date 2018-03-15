using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host.Behaviors
{
    public class UserNamePasswordValidatorBehavior : ServiceCredentials
    {
        private UserNamePasswordValidator _validator;
        
        public UserNamePasswordValidatorBehavior(UserNamePasswordValidator validator)
            : base()
        {
            _validator = validator;

            UserNameAuthentication.CustomUserNamePasswordValidator = _validator;
            UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
            ServiceCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "localhost");
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        protected override ServiceCredentials CloneCore()
        {
            return new UserNamePasswordValidatorBehavior(_validator);
        }
    }
}
