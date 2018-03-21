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
        private RequestDelegate _next;
        private PathString _path;
        private IEnumerable<Uri> _baseAddresses;

        public WcfProxyMiddleware(RequestDelegate next, AspNetCoreServiceHost<TService> host, IBaseAddressFactory addressFactory, PathString path)
        {
            _next = next;
            _path = path;

            _baseAddresses = addressFactory.Create(path);
            host.Initialize(_baseAddresses);
            host.Open();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(_path, StringComparison.OrdinalIgnoreCase))
            {
                var url = GenerateForwardToUrl(context);
                var response = await context.ForwardToAsync(url, context.RequestAborted);
                await context.RespondWithAsync(response, context.RequestAborted);
                return;
            }
            await _next(context);
        }

        private Uri GenerateForwardToUrl(HttpContext context)
        {
            var baseAddress = _baseAddresses.First();
            var path = context.Request.Path.ToString().Remove(0, _path.ToString().Length);
            if (path.StartsWith("/"))
                path = path.Substring(1);
            var query = context.Request.QueryString;

            var url = new Uri(baseAddress, path + query);
            return url;
        }
    }    
}
