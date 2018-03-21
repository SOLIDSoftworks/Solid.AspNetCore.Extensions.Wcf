using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MultipleContract : IEchoContract, ICounterContract
    {
        public string Echo(string value)
        {
            return value;
        }

        private int _counter;
        public int Increment()
        {
            return ++_counter;
        }
    }
}
