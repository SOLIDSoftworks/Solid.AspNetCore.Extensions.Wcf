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
    public class InstanceTestFixture : IDisposable
    {
        private IInstanceTestService _perCall;
        private IInstanceTestService _singleton;

        public InstanceTestFixture()
        {
            TestingServer = new TestingServerBuilder()
                .AddAspNetCoreHostFactory()
                .AddStartup<InstanaceTestStartup>()
                .Build();
        }

        public TestingServer TestingServer { get; }

        public IInstanceTestService GetPerCallService()
        {
            if (_perCall == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "percall");
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<IInstanceTestService>(binding, endpoint);
                var client = factory.CreateChannel();
                _perCall = client;

                var channel = client as ICommunicationObject;
                channel.Closed += (sender, args) =>
                {
                    _perCall = null;
                };
            }
            return _perCall;
        }

        public IInstanceTestService GetSingletonService()
        {
            if (_singleton == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "singleton");
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<IInstanceTestService>(binding, endpoint);
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

        public void Dispose()
        {
            Close(_perCall);
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
