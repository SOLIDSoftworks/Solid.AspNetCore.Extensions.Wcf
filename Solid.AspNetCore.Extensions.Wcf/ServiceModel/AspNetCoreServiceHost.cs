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

        public event EventHandler DescriptionInitializing;
        public event EventHandler DescriptionInitialized;

        public override void Initialize(IEnumerable<Uri> baseAddresses)
        {
            if (DescriptionInitializing != null)
                DescriptionInitializing(this, EventArgs.Empty);

            foreach (var baseAddress in baseAddresses)
            {
                if (_instance == null)
                    InitializeDescription(typeof(TService), new UriSchemeKeyedCollection(baseAddress));
                else
                    InitializeDescription(_instance, new UriSchemeKeyedCollection(baseAddress));
            }

            if (DescriptionInitialized != null)
                DescriptionInitialized(this, EventArgs.Empty);

            foreach (var endpoint in _endpoints)
                AddServiceEndpoint(endpoint.Contract, endpoint.Binding, endpoint.Path);
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
    }

    internal abstract class AspNetCoreServiceHost : ServiceHost
    {
        public abstract void Initialize(IEnumerable<Uri> baseAddresses);
    }
}
