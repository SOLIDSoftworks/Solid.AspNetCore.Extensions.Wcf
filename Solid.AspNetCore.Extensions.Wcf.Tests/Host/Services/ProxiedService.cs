using Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host.Services
{
    public class ProxiedService : IProxiedService
    {

        public bool IsProxied()
        {
            var request = WebOperationContext.Current.IncomingRequest;
            return request.Headers["X-AspNetCore-Trace-Id"] != null;
        }
    }
}
