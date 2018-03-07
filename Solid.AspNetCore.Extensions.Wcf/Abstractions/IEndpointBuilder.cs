using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    /// <summary>
    /// Extensions methods for the endpoint builder
    /// </summary>
    /// <typeparam name="TService">The service implementation type</typeparam>
    public interface IEndpointBuilder<TService>
    {
        /// <summary>
        /// Adds a service endpoint using the default binding to the base address of a service host
        /// </summary>
        /// <typeparam name="TContract">The contract type of the service endpoint</typeparam>
        /// <returns>The endpoint builder</returns>
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>();

        /// <summary>
        /// Adds a service endpoint using the default binding to a specified path on a service host
        /// </summary>
        /// <typeparam name="TContract">The contract type of the service endpoint</typeparam>
        /// <param name="path">The sub path for the endpoint</param>
        /// <returns>The endpoint builder</returns>
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>(string path);

        /// <summary>
        /// Adds a service endpoint using the provided binding to the base address of a service host
        /// </summary>
        /// <typeparam name="TContract">The contract type of the service endpoint</typeparam>
        /// <param name="binding"></param>
        /// <returns>The endpoint builder</returns>
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding);

        /// <summary>
        /// Adds a service endpoint using the provided binding to a specified path on a service host
        /// </summary>
        /// <typeparam name="TContract">The contract type of the service endpoint</typeparam>
        /// <param name="binding"></param>
        /// <param name="path">The sub path for the endpoint</param>
        /// <returns>The endpoint builder</returns>
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding, string path);

    }
}
