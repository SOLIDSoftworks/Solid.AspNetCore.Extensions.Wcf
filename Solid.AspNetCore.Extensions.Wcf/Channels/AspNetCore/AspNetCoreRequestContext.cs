using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore
{
    class AspNetCoreRequestContext<TService> : AsyncRequestContextBase
    {
        private HttpContext _http;

        public AspNetCoreRequestContext(IAspNetCoreChannel channel, BindingContext context, Message message, HttpContext http)
            : base()
        {
            Channel = channel;
            BindingContext = context;
            RequestMessage = message;
            _http = http;
        }

        public IAspNetCoreChannel Channel { get; }
        public BindingContext BindingContext { get; }
        public override Message RequestMessage { get; }

        public override void Abort()
        {
        }

        public override void Close()
        {
        }

        public override void Close(TimeSpan timeout)
        {
        }

        public override Task ReplyAsync(Message message)
        {
            return ReplyAsync(message, BindingContext.Binding.SendTimeout);
        }

        public override async Task ReplyAsync(Message message, TimeSpan timeout)
        {
            var response = _http.Response;
            if (message.IsFault)
                response.StatusCode = 500;
            
            var encoder = GetMessageEncoder();

            SanitizeMessage(message, encoder.MessageVersion);

            response.ContentType = encoder.MediaType;

            encoder.WriteMessage(message, response.Body);
            await Channel.ReplyAsync(_http);
        }

        private void SanitizeMessage(Message message, MessageVersion version)
        {
            if (version.Addressing != AddressingVersion.None) return;
            message.Headers.Action = null;
            message.Headers.To = null;
        }

        private MessageEncoder GetMessageEncoder()
        {
            var element = BindingContext.Binding.Elements.OfType<MessageEncodingBindingElement>().FirstOrDefault();
            return element?.CreateMessageEncoderFactory().Encoder;
        }
    }
}
