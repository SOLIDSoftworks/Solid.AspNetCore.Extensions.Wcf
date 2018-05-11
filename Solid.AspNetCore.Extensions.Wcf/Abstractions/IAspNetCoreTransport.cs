using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IAspNetCoreTransport
    {
        bool CanHandleRequest(HttpContext context);
        Task HandleRequestAsync(HttpContext context);
    }

    internal interface IAspNetCoreTransport<TService> : IAspNetCoreTransport
    {
        void Initialize(PathString basePath);
        Task<HttpContext> RecieveAsync();
        Task ReplyAsync(HttpContext context);
        Task AcceptAsync();
        void EndAccept();
    }
}
