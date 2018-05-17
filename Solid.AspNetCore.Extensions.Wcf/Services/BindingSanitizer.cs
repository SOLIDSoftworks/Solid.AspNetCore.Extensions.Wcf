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
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Solid.AspNetCore.Extensions.Wcf.Services
{
    internal class BindingSanitizer : IBindingSanitizer
    {
        private IServiceProvider _services;
        private IServer _server;

        public BindingSanitizer(IServiceProvider services, IServer server)
        {
            _services = services;
            _server = server;
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
            where TTransport : HttpTransportBindingElement
        {
            var current = custom.Elements.OfType<TTransport>().FirstOrDefault();
            if (current == null) return;
            if (current is AspNetCoreTransportBindingElement) return;

            custom.Elements.Remove(current);
            //custom.Elements.Add(new HttpTransportBindingElement());
            var handler = _services.GetService<IAspNetCoreHandler>();
            var factory = _services.GetService<IMessageFactory>();
            var loggerFactory = _services.GetService<ILoggerFactory>();
            var scheme = GetScheme(current);
            var aspNetCore = new AspNetCoreTransportBindingElement(scheme, handler, factory, loggerFactory);

            aspNetCore.AllowCookies = current.AllowCookies;
            aspNetCore.AuthenticationScheme = current.AuthenticationScheme;
            aspNetCore.BypassProxyOnLocal = current.BypassProxyOnLocal;
            aspNetCore.DecompressionEnabled = current.DecompressionEnabled;
            aspNetCore.ExtendedProtectionPolicy = current.ExtendedProtectionPolicy;
            aspNetCore.HostNameComparisonMode = current.HostNameComparisonMode;
            aspNetCore.KeepAliveEnabled = current.KeepAliveEnabled;
            aspNetCore.ManualAddressing = current.ManualAddressing;
            aspNetCore.MaxBufferPoolSize = current.MaxBufferPoolSize;
            aspNetCore.MaxBufferSize = current.MaxBufferSize;
            aspNetCore.MaxPendingAccepts = current.MaxPendingAccepts;
            aspNetCore.MaxReceivedMessageSize = current.MaxReceivedMessageSize;
            aspNetCore.MessageHandlerFactory = current.MessageHandlerFactory;
            aspNetCore.ProxyAddress = current.ProxyAddress;
            aspNetCore.ProxyAuthenticationScheme = current.ProxyAuthenticationScheme;
            aspNetCore.Realm = current.Realm;
            aspNetCore.RequestInitializationTimeout = current.RequestInitializationTimeout;
            aspNetCore.TransferMode = current.TransferMode;
            aspNetCore.UnsafeConnectionNtlmAuthentication = current.UnsafeConnectionNtlmAuthentication;
            aspNetCore.UseDefaultWebProxy = current.UseDefaultWebProxy;
            aspNetCore.WebSocketSettings = current.WebSocketSettings;

            custom.Elements.Add(aspNetCore);
        }

        private string GetScheme(HttpTransportBindingElement original)
        {
            var feature = _server.Features[typeof(IServerAddressesFeature)] as IServerAddressesFeature;
            var addresses = feature.Addresses.Select(s => new Uri(s));

            if (addresses.Any(u => u.Scheme == "https")) return original.Scheme;
            return "http";
        }
    }
}
