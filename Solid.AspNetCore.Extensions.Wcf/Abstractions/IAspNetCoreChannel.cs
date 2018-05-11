using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IAspNetCoreChannel
    {
        Uri BaseAddress { get; }
        Task OpenAsync();
        Task HandleAsync(HttpContext context);
        Task<HttpContext> ReceiveAsync();
        Task ReplyAsync(HttpContext context);
        Task CloseAsync();
    }
}
