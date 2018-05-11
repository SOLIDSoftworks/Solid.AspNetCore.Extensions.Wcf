using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Middleware
{
    internal class AspNetCoreTransportMiddleware<TService> 
    {
        private object _lock = new object();
        private RequestDelegate _next;
        private IServiceHostProvider<TService> _provider;
        private PathString _path;
        private IAspNetCoreHandler _handler;

        public AspNetCoreTransportMiddleware(RequestDelegate next, IServiceHostProvider<TService> provider, IAspNetCoreHandler handler, PathString path)
        {
            _next = next;
            _provider = provider;
            _path = path;
            _handler = handler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (CanHandleRequest(context))
            {
                var host = _provider.Host;
                if (host.State != CommunicationState.Opened)
                {
                    try
                    {
                        host.Open();
                    }
                    catch(Exception ex)
                    {

                    }
                }


                if (await _handler.CanHandleAsync(context))
                {
                    await _handler.HandleAsync(context);
                    return;
                }
            }
            await _next(context);
        }

        private bool CanHandleRequest(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(_path);
        }
    }
}
