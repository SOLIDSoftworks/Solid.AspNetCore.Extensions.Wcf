using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
        private ILogger<AspNetCoreTransportMiddleware<TService>> _logger;

        public AspNetCoreTransportMiddleware(RequestDelegate next, ILogger<AspNetCoreTransportMiddleware<TService>> logger , IServiceHostProvider<TService> provider, IAspNetCoreHandler handler, PathString path)
        {
            _next = next;
            _provider = provider;
            _path = path;
            _handler = handler;

            _logger = logger;
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
                    catch (Exception ex)
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
