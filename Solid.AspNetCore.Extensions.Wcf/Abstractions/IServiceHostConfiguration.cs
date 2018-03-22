using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    /// <summary>
    /// The service host configuration
    /// </summary>
    /// <typeparam name="TService">The service implementation type</typeparam>
    public interface IServiceHostConfiguration<TService>
    {
        /// <summary>
        /// The application service collection
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// The factory used to create the service host. 
        /// <para>This should only be used to construct the service host as all behaviors will be added using the service container.</para>
        /// <para>The first variable is the service instance. It will be null </para>
        /// </summary>
        ServiceHostFactoryDelegate<TService> ServiceHostFactory { get; set; }
    }
}
