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
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public interface IEndpointBuilder<TService>
    {
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>();
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>(string path);
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding);
        IEndpointBuilder<TService> AddServiceEndpoint<TContract>(Binding binding, string path);

    }
}
