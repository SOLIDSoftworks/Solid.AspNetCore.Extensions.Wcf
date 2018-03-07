using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Extensions
{
    internal static class HttpContextExtensions
    {
        private static HttpClient _client = new HttpClient();

        public static async Task<HttpResponseMessage> ForwardToAsync(this HttpContext context, Uri url, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(new HttpMethod(context.Request.Method), new Uri(url, context.Request.Path + context.Request.QueryString));

            var content = await context.GetHttpRequestContentAsync();
            if (content != null)
                request.Content = content;

            request.Headers.Add("Host", context.Request.Host.Value);
            foreach (var header in context.Request.Headers)
            {
                if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable()))
                    request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable());
            }

            return await _client.SendAsync(request);
        }

        public static async Task RespondWithAsync(this HttpContext context, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            var headers = response
                .Headers
                .Where(header => !header.Key.Equals("transfer-encoding", StringComparison.OrdinalIgnoreCase))
                .Concat(response.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>());

            foreach (var header in headers)
                context.Response.Headers.Add(header.Key, new StringValues(header.Value.ToArray()));

            context.Response.StatusCode = (int)response.StatusCode;
            await response.Content.CopyToAsync(context.Response.Body);
        }

        private static async Task<HttpContent> GetHttpRequestContentAsync(this HttpContext context)
        {
            var stream = new MemoryStream();
            await context.Request.Body.CopyToAsync(stream);
            if (stream.Length == 0) return null;

            stream.Position = 0;
            return new StreamContent(stream);
        }
    }
}
