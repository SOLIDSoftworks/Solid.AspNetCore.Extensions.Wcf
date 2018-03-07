using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
