using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Extensions;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Middleware
{
    internal class WcfProxyMiddleware<TService>
    {
        private Uri _baseAddress;

        public WcfProxyMiddleware(RequestDelegate next, AspNetCoreServiceHost<TService> host, IBaseAddressFactory addressFactory, PathString path)
        {
            _baseAddress = addressFactory.Create(path);
            host.Initialize(_baseAddress);
            host.Open();
        }

        public async Task Invoke(HttpContext context)
        {
            var response = await context.ForwardToAsync(_baseAddress, context.RequestAborted);
            await context.RespondWithAsync(response, context.RequestAborted);
        }
    }    
}
