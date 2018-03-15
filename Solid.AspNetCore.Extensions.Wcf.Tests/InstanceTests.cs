using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using Solid.Testing;
using System;
using System.ServiceModel;
using Xunit;

namespace Solid.AspNetCore.Extensions.Wcf.Tests
{
    public class InstanceTests : IClassFixture<InstanceTestFixture>
    {
        public InstanceTests(InstanceTestFixture fixture)
        {
            _fixture = fixture;
        }

        private InstanceTestFixture _fixture;

        public TestingServer TestingServer => _fixture.TestingServer;

        [Fact]
        public void ShouldHostPerCallService()
        {
            var service = _fixture.GetPerCallService();
            var first = service.Counter();
            var second = service.Counter();
            var firstId = service.InstanceId();
            var secondId = service.InstanceId();

            Assert.Equal(1, first);
            Assert.Equal(first, second);

            Assert.NotEqual(firstId, secondId);
        }

        [Fact]
        public void ShouldHostSingletonService()
        {
            var service = _fixture.GetSingletonService();
            var first = service.Counter();
            var second = service.Counter();
            var firstId = service.InstanceId();
            var secondId = service.InstanceId();

            Assert.Equal(1, first);
            Assert.Equal(2, second);

            Assert.Equal(firstId, secondId);
        }
    }
}
