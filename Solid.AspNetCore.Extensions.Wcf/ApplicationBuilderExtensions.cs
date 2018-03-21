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
    /// <summary>
    /// Extensions methods for IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a contract endpoint to a service host for a service implementation
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <typeparam name="TContract">The service contract</typeparam>
        /// <param name="builder">The application builder</param>
        /// <param name="path">The endpoint for the service host</param>
        /// <returns>The application builder</returns>
        public static IApplicationBuilder UseWcfService<TService, TContract>(this IApplicationBuilder builder, PathString path)
        {
            return builder.UseWcfService<TService>(path, b => b.AddServiceEndpoint<TContract>());
        }

        /// <summary>
        /// Adds one or more contract endpoints to a service host for a service implementation
        /// </summary>
        /// <typeparam name="TService">The service implementation type</typeparam>
        /// <param name="builder">The application builder</param>
        /// <param name="path">The endpoint for the service host</param>
        /// <param name="action">Configuration action the adds endpoints</param>
        /// <returns>The application builder</returns>
        public static IApplicationBuilder UseWcfService<TService>(this IApplicationBuilder builder, PathString path, Action<IEndpointBuilder<TService>> action)
        {
            var endpoints = builder.ApplicationServices.GetService<EndpointBuilder<TService>>();
            action(endpoints);

            return builder.UseMiddleware<WcfProxyMiddleware<TService>>(path);


            //return builder.MapWhen(context => 
            //    context.Request.Path.StartsWithSegments(path), 
            //    b => b.UseMiddleware<WcfProxyMiddleware<TService>>(path));
        }
    }
}
