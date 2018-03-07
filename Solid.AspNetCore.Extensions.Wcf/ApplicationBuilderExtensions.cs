using Microsoft.AspNetCore.Builder;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Middleware;
using Solid.AspNetCore.Extensions.Wcf.Builders;

namespace Solid.AspNetCore.Extensions.Wcf
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWcfService<TService, TContract>(this IApplicationBuilder builder, PathString path)
        {
            return builder.UseWcfService<TService>(path, b => b.AddServiceEndpoint<TContract>());
        }

        public static IApplicationBuilder UseWcfService<TService>(this IApplicationBuilder builder, PathString path, Action<IEndpointBuilder<TService>> action)
        {
            var endpoints = builder.ApplicationServices.GetService<EndpointBuilder<TService>>();
            action(endpoints);
            return builder.Map(path, b => b.UseMiddleware<WcfProxyMiddleware<TService>>(path));
        }
    }
}
