using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Solid.AspNetCore.Extensions.Wcf.Factories
{
    public class DefaultMessageFactory : IMessageFactory
    {
        public Task<Message> CreateMessageAsync(HttpContext http, BindingContext binding)
        {
            var encoder = GetMessageEncoderFactory(binding).Encoder;
            var request = http.Request;

            var message = null as Message;
            if (request.ContentLength != null && request.ContentLength > 0)
                message = encoder.ReadMessage(request.Body, int.MaxValue);
            else
                message = new NullMessage();
            if (message.Headers.To == null)
                message.Headers.To = request.GetRequestUri();
            if (message.Properties.Via == null)
                message.Properties.Via = request.GetRequestUri();
            if (message.Headers.Action == null)
                message.Headers.Action = request.Headers["SOAPAction"].Select(s => s.Trim('"')).FirstOrDefault();
            
            return Task.FromResult(message);
        }

        private MessageEncodingBindingElement GetMessageEncodingBindingElement(BindingContext context)
        {
            return context.Binding.Elements.OfType<MessageEncodingBindingElement>().FirstOrDefault();
        }

        private MessageEncoderFactory GetMessageEncoderFactory(BindingContext context)
        {
            return context.Binding.Elements.OfType<MessageEncodingBindingElement>().FirstOrDefault()?.CreateMessageEncoderFactory();
        }
    }
}
