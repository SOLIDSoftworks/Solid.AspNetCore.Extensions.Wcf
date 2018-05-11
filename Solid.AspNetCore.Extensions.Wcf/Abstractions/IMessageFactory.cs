using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    public interface IMessageFactory
    {
        Task<Message> CreateMessageAsync(HttpContext http, BindingContext binding);
    }
}
