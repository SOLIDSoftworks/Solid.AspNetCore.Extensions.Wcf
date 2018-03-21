using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solid.AspNetCore.Extensions.Wcf.Tests
{
    public class MultipleContractTests : IClassFixture<MultipleContractTestFixture>
    {
        private MultipleContractTestFixture _fixture;

        public MultipleContractTests(MultipleContractTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ShouldEcho()
        {
            var service = _fixture.GetEchoService();
            var expected = Guid.NewGuid().ToString();
            var value = service.Echo(expected);
            Assert.Equal(expected, value);
        }

        [Fact]
        public void ShouldEchoUsingRedirectedService()
        {
            var service = _fixture.GetRedirectedEchoService();
            var expected = Guid.NewGuid().ToString();
            var value = service.Echo(expected);
            Assert.Equal(expected, value);
        }

        [Fact]
        public void ShouldIncrement()
        {
            var service = _fixture.GetCounterService();

            var first = service.Increment();
            var second = service.Increment();

            Assert.Equal(1, first);
            Assert.Equal(2, second);
        }
    }
}
