using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels.AspNetCore;

namespace Solid.AspNetCore.Extensions.Wcf.Services
{
    internal class BindingSanitizer : IBindingSanitizer
    {
        private IServiceProvider _services;

        public BindingSanitizer(IServiceProvider services)
        {
            _services = services;
        }

        public Binding SanitizeBinding(Binding binding)
        {
            var custom = binding as CustomBinding;
            if (custom == null)
                custom = new CustomBinding(binding);

            var security = custom.Elements.OfType<SecurityBindingElement>();
            foreach (var element in security)
                element.AllowInsecureTransport = true;

            Replace<HttpTransportBindingElement>(custom);
            Replace<HttpsTransportBindingElement>(custom);

            return custom;
        }

        private void Replace<TTransport>(CustomBinding custom)
            where TTransport : TransportBindingElement
        {
            var current = custom.Elements.OfType<TTransport>().FirstOrDefault();
            if (current != null)
            {
                custom.Elements.Remove(current);
                //custom.Elements.Add(new HttpTransportBindingElement());
                var handler = _services.GetService<IAspNetCoreHandler>();
                var factory = _services.GetService<IMessageFactory>();
                var loggerFactory = _services.GetService<ILoggerFactory>();
                var aspNetCore = new AspNetCoreTransportBindingElement(current.Scheme, handler, factory, loggerFactory);
                custom.Elements.Add(aspNetCore);
            }
        }
    }
}
