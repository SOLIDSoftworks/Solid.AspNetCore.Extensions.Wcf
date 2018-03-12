using Simple.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Simple.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EchoWcfService : IEchoContract
    {
        private IEchoService _service;

        public EchoWcfService(IEchoService service)
        {
            _service = service;
        }

        public string EchoString(string value)
        {
            return _service.Echo(value);
        }
    }
}
