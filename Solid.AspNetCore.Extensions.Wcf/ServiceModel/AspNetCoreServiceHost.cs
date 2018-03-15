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
                Binding = binding, 
                Path = path
            });
        }

        protected override void InitializeRuntime()
        {
            if (Initializing != null)
                Initializing(this, EventArgs.Empty);
            foreach (var endpoint in _endpoints)
                AddServiceEndpoint(endpoint.Contract, endpoint.Binding, endpoint.Path);
            base.InitializeRuntime();
        }
    }

    internal abstract class AspNetCoreServiceHost : ServiceHost
    {
        public abstract void Initialize(IEnumerable<Uri> baseAddresses);
    }
}
