using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using Solid.AspNetCore.Extensions.Wcf.Channels.AspNetCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Behaviors
{
    internal class AspNetCoreServiceMetadataBehavior : IServiceBehavior
    {
        private ConcurrentDictionary<Uri, BindingParameterCollection> _bindingParameters = new ConcurrentDictionary<Uri, BindingParameterCollection>();

        private IMessageFactory _messageFactory;
        private IAspNetCoreHandler _handler;
        private IBindingSanitizer _sanitizer;

        public AspNetCoreServiceMetadataBehavior(IMessageFactory messageFactory, IAspNetCoreHandler handler, IBindingSanitizer sanitizer)
        {
            _messageFactory = messageFactory;
            _handler = handler;
            _sanitizer = sanitizer;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            foreach(var endpoint in endpoints)
            {
                var parameters = _bindingParameters.GetOrAdd(endpoint.Address.Uri, url => new BindingParameterCollection());
                foreach (var parameter in bindingParameters)
                    parameters.Add(parameter);
            }
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            for(var i = serviceHostBase.ChannelDispatchers.Count - 1; i >= 0; i--)
            {
                var dispatcher = serviceHostBase.ChannelDispatchers[i] as ChannelDispatcher;
                if (dispatcher == null) continue;
                if (!dispatcher.BindingName.StartsWith("ServiceMetadata")) continue;

                //var url = dispatcher.Listener.Uri;
                //var binding = null as Binding;
                //if (url.Scheme == "http")
                //    binding = new BasicHttpBinding();
                //else if (url.Scheme == "https")
                //    binding = new BasicHttpsBinding();

                serviceHostBase.ChannelDispatchers.RemoveAt(i);

                //if (binding == null) continue;

                //binding.Name = $"AspNetCore{dispatcher.BindingName}";
                //var custom = _sanitizer.SanitizeBinding(binding) as CustomBinding;
                //var encoding = custom.Elements.OfType<MessageEncodingBindingElement>().First();
                //encoding.MessageVersion = MessageVersion.None;
                //var transport = custom.Elements.OfType<TransportBindingElement>().First();
                //var parameters = _bindingParameters.GetOrAdd(url, key => new BindingParameterCollection());
                //var context = new BindingContext(custom, parameters, url, string.Empty, ListenUriMode.Explicit);
                //var listener = transport.BuildChannelListener<IReplyChannel>(context) as AspNetCoreChannelListener;
                //listener.IsGet = true;
                
                //var replacement = new ChannelDispatcher(listener, custom.Name, custom);

                //foreach(var endpoint in dispatcher.Endpoints)
                //    replacement.Endpoints.Add(Copy(endpoint));

                //replacement.MessageVersion = encoding.MessageVersion;
                //serviceHostBase.ChannelDispatchers.Add(replacement);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        //private EndpointDispatcher Copy(EndpointDispatcher endpoint)
        //{
        //    var copied = new EndpointDispatcher(endpoint.EndpointAddress, endpoint.ContractName, endpoint.ContractNamespace, endpoint.IsSystemEndpoint);

        //    copied.FilterPriority = endpoint.FilterPriority;
        //    copied.ContractFilter = endpoint.ContractFilter;
        //    copied.AddressFilter = endpoint.AddressFilter;

        //    var field = typeof(EndpointDispatcher).GetField("dispatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic);
        //    field.SetValue(copied, endpoint.DispatchRuntime);

        //    return copied;
        //}
    }
}
