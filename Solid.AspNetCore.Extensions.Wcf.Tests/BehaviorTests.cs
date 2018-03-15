using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solid.AspNetCore.Extensions.Wcf.Tests
{
    public class BehaviorTests : IClassFixture<BehaviorTestFixture>
    {
        private BehaviorTestFixture _fixture;

        public BehaviorTests(BehaviorTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldAddCustomUserNamePasswordValidator()
        {
            var service = _fixture.GetService("username", "password");
            service.InstanceId();

            _fixture.VerifyUserNamePassword("username", "password");
        }
    }
}
