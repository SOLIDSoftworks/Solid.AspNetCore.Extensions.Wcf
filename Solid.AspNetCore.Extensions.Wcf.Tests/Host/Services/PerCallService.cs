using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PerCallService : IInstanceTestService
    {
        private Guid _instanceId = Guid.NewGuid();
        private int _counter;
        public int Counter() => ++_counter;

        public Guid InstanceId() => _instanceId;
    }
}
 