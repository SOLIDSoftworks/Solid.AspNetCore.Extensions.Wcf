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

        private IProxiedService _proxied;
        private IDirectService _direct;

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

        public IProxiedService GetProxiedService()
        {
            if (_proxied == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "proxied");
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<IProxiedService>(binding, endpoint);
                var client = factory.CreateChannel();
                _proxied = client;

                var channel = client as ICommunicationObject;
                channel.Closed += (sender, args) =>
                {
                    _proxied = null;
                };
            }
            return _proxied;
        }

        public IDirectService GetDirectService()
        {
            if (_direct == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "direct");
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<IDirectService>(binding, endpoint);
                var client = factory.CreateChannel();
                _direct = client;

                var channel = client as ICommunicationObject;
                channel.Closed += (sender, args) =>
                {
                    _direct = null;
                };
            }
            return _direct;
        }

        public void Dispose()
        {
            Close(_perCall);
            Close(_singleton);
            Close(_proxied);
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
