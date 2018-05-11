using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    internal static class HttpRequestExtensions
    {
        public static Uri GetRequestUri(this HttpRequest request)
        {
            var url = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
            return new Uri(url);
        }
    }
}
