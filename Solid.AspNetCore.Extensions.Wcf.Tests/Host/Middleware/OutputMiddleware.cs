using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Host.Middleware
{
    class OutputMiddleware
    {
        private static object _requestLock = new object();
        private static object _responseLock = new object();
        private RequestDelegate _next;

        public OutputMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var response = context.Response.Body;
            context.Response.Body = new MemoryStream();
            context.Request.Body = await context.Request.Body.ToMemoryStreamAsync();

            if (IsXml(context.Request.ContentType))
                WriteRequestBody(context.Request.Body);
            await _next(context);
            if (IsXml(context.Response.ContentType))
                WriteResponseBody(context.Response.Body);

            context.Response.Body.Position = 0;
            await context.Response.Body.CopyToAsync(response);
            context.Response.Body = response;
        }

        private void WriteResponseBody(Stream body)
        {
            body.Position = 0;
            using (var reader = new StreamReader(body, Encoding.UTF8, true, 4096, true))
            {
                Debug.WriteLine("----------------------- Outgoing response body ---------------------");
                Debug.WriteLine(string.Empty);
                var xml = reader.ReadToEnd();
                var doc = XDocument.Parse(xml);
                Debug.WriteLine(doc.ToString());
                Debug.WriteLine(string.Empty);
            }
        }

        private void WriteRequestBody(Stream body)
        {
            using (var reader = new StreamReader(body, Encoding.UTF8, true, 4096, true))
            {
                Debug.WriteLine("----------------------- Incoming request body ---------------------");
                Debug.WriteLine(string.Empty);
                var xml = reader.ReadToEnd();
                var doc = XDocument.Parse(xml);
                Debug.WriteLine(doc.ToString());
                Debug.WriteLine(string.Empty);
            }
            body.Position = 0;
        }

        private bool IsXml(string contentType)
        {
            var xml = contentType?.StartsWith("application/soap+xml");
            if (xml.HasValue && xml.Value) return true;
            xml = contentType?.StartsWith("text/xml");
            if (xml.HasValue && xml.Value) return true;

            return false;
        }
    }
    internal static class StreamExtensions
    {
        public static async Task<Stream> ToMemoryStreamAsync(this Stream stream)
        {
            if (stream is MemoryStream) return stream;
            var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            memory.Position = 0;
            return memory;
        }
    }
}
