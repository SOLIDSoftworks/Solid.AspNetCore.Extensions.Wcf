using Solid.AspNetCore.Extensions.Wcf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel
{
    internal class AspNetCoreServiceHost<TService> : AspNetCoreServiceHost
    {
        private List<ServiceEndpointDescription> _endpoints = new List<ServiceEndpointDescription>();
        private TService _instance;

        public AspNetCoreServiceHost(TService instance)
            : this()
        {
            _instance = instance;
        }
        public AspNetCoreServiceHost()
            : base()
        {
        }

        public event EventHandler Initializing;

        public override void Initialize(IEnumerable<Uri> baseAddresses)
        {
            foreach (var baseAddress in baseAddresses)
            {
                if (_instance == null)
                    InitializeDescription(typeof(TService), new UriSchemeKeyedCollection(baseAddress));
                else
                    InitializeDescription(_instance, new UriSchemeKeyedCollection(baseAddress));
            }
        }

        public void AddEndpoint<TContract>(Binding binding, string path)
        {
            _endpoints.Add(new ServiceEndpointDescription
            {
                Contract = typeof(TContract),
                Binding = SanitizeBinding(binding), 
                Path = path
            });
        }

        private Binding SanitizeBinding(Binding dirty)
        {
            var custom = dirty as CustomBinding;
            if (custom == null)
                custom = new CustomBinding(dirty);
            
            var security = custom.Elements.OfType<SecurityBindingElement>();
            foreach (var element in security)
                element.AllowInsecureTransport = true;

            var https = custom.Elements.OfType<TransportBindingElement>().Where(e => e.Scheme == "https").FirstOrDefault();
            if (https != null)
            {
                var http = new HttpTransportBindingElement();
                custom.Elements.Remove(https);
                custom.Elements.Add(http);
            }
            return custom;
        }

        protected override void InitializeRuntime()
        {
            if (Initializing != null)
                Initializing(this, EventArgs.Empty);
            foreach (var endpoint in _endpoints)
                AddServiceEndpoint(endpoint.Contract, endpoint.Binding, endpoint.Path);
            base.InitializeRuntime();
        }

        private string Sanitize(string relative)
        {
            while (relative.StartsWith("/"))
                relative = relative.Substring(1);
            return relative;
        }

        private string GetAbsolute(string relative)
        {
            while (relative.StartsWith("/"))
                relative = relative.Substring(1);
            var baseAddress = BaseAddresses.First().ToString();
            if (string.IsNullOrEmpty(relative))
                return baseAddress;

            if (!baseAddress.EndsWith("/"))
                baseAddress = baseAddress + "/";
            var baseUri = new Uri(baseAddress);
            var absolute = new Uri(baseUri, relative);
            return absolute.ToString();
        }
    }

    internal abstract class AspNetCoreServiceHost : ServiceHost
    {
        public abstract void Initialize(IEnumerable<Uri> baseAddresses);
    }
}
