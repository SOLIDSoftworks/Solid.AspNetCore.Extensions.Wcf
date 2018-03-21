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
    public class MultipleContractTestFixture : IDisposable
    {
        private IEchoContract _echo;
        private ICounterContract _counter;

        public MultipleContractTestFixture()
        {
            TestingServer = new TestingServerBuilder()
                .AddAspNetCoreHostFactory()
                .AddStartup<MulitpleContractTestStartup>()
                .Build();
        }

        public TestingServer TestingServer { get; }

        public IEchoContract GetEchoService()
        {
            if (_echo == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "multiple/echo");
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<IEchoContract>(binding, endpoint);
                var client = factory.CreateChannel();
                _echo = client;

                var channel = client as ICommunicationObject;
                channel.Closed += (sender, args) =>
                {
                    _echo = null;
                };
            }
            return _echo;
        }

        public ICounterContract GetCounterService()
        {
            if (_counter == null)
            {
                var url = new Uri(TestingServer.BaseAddress, "multiple/counter");
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(url);
                var factory = new ChannelFactory<ICounterContract>(binding, endpoint);
                var client = factory.CreateChannel();
                _counter = client;

                var channel = client as ICommunicationObject;
                channel.Closed += (sender, args) =>
                {
                    _counter = null;
                };
            }
            return _counter;
        }

        public void Dispose()
        {
            Close(_echo);
            Close(_counter);
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
