using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Tests.Host;
using Solid.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests
{
    public class BehaviorTestFixture : IDisposable
    {
        private IInstanceTestService _singleton;

        public BehaviorTestFixture()
        {
            TestingServer = new TestingServerBuilder()
                .AddAspNetCoreHostFactory()
                .AddStartup<BehaviorTestStartup>()
                .Build();
        }

        public TestingServer TestingServer { get; }

        public IInstanceTestService GetService(string username, string password)
        {
            if (_singleton == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "singleton");
                var binding = new WS2007HttpBinding(SecurityMode.Message);
                binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
                binding.Security.Message.EstablishSecurityContext = false;
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<IInstanceTestService>(binding, endpoint);

                factory.Credentials.UserName.UserName = username;
                factory.Credentials.UserName.Password = password;

                var client = factory.CreateChannel();
                _singleton = client;

                var channel = client as ICommunicationObject;
                channel.Closed += (sender, args) =>
                {
                    _singleton = null;
                };
            }
            return _singleton;
        }

        public void VerifyUserNamePassword(string username, string password)
        {
            BehaviorTestStartup.MockUserNamePasswordValidator.Verify(v => v.Validate(username, password));
        }

        public void Dispose()
        {
            Close(_singleton);
            TestingServer.Dispose();
        }

        private void Close(object service)
        {
            var channel = service as ICommunicationObject;
            if (channel != null)
                channel.Close();
        }
    }
}
