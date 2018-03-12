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
    public class HttpTestingServerFixture : IDisposable
    {
        private IInstanceTestService _perCall;
        private IInstanceTestService _singleton;

        public HttpTestingServerFixture()
        {
            TestingServer = new TestingServerBuilder()
                .AddAspNetCoreHostFactory()
                .AddStartup<Startup>()
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
            }
            return _singleton;
        }

        public void Dispose()
        {
            TestingServer.Dispose();
            Close(_perCall);
        }

        private void Close(object service)
        {
            var channel = service as ICommunicationObject;
            if (channel != null)
                channel.Close();
        }
    }
}
