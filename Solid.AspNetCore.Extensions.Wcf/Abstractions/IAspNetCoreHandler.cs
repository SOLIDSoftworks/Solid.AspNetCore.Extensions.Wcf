using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IAspNetCoreHandler
    {
        Task<IAspNetCoreChannel> OpenAsync(Uri baseAddress, string method = "POST");
        Task<bool> CanHandleAsync(HttpContext context);
        Task HandleAsync(HttpContext context);
        Task CloseAsync(Guid channelId);
    }
}
