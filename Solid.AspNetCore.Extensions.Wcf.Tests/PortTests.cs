using Solid.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solid.AspNetCore.Extensions.Wcf.Tests
{
    public class PortTests : IClassFixture<HttpTestingServerFixture>
    {
        private HttpTestingServerFixture _fixture;

        public PortTests(HttpTestingServerFixture fixture)
        {
            _fixture = fixture;
        }

        public TestingServer TestingServer => _fixture.TestingServer;

        [Fact]
        public void ShouldHostWcfServiceOnSeperatePort()
        {
            var proxied = _fixture.GetProxiedService();
            Assert.True(proxied.IsProxied());
        }

        //[Fact]        
        //public void ShouldHostWcfServiceOnMainHostPort()
        //{
        //    var direct = _fixture.GetDirectService();
        //    Assert.True(direct.IsDirect());
        //}
    }
}
