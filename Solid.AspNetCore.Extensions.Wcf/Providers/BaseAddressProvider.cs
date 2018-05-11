using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Providers
{
    internal class BaseAddressProvider : IBaseAddressProvider
    {
        private IServer _server;
        private ConcurrentDictionary<Type, List<PathString>> _baseAddresses;

        public BaseAddressProvider(IServer server)
        {
            _server = server;
            _baseAddresses = new ConcurrentDictionary<Type, List<PathString>>();
        }

        public void AddBaseAddressFor<TService>(PathString path)
        {
            var paths = GetPathsFor<TService>();
            paths.Add(path);
        }

        public Uri[] GetBaseAddressesFor<TService>()
        {
            var paths = GetPathsFor<TService>();
            var root = GenerateRootAddresses();
            return paths
                .Select(p => p.ToString())
                .Select(s =>
                {
                    if (!s.EndsWith("/"))
                        s = s + "/";
                    return s;
                })
                .SelectMany(s => root.Select(r => new Uri(r, s)))
                .ToArray();
        }

        private IEnumerable<Uri> GenerateRootAddresses()
        {
            return _server.Features.Get<IServerAddressesFeature>().Addresses.Select(u => new Uri(u));
        }

        private List<PathString> GetPathsFor<TService>()
        {
            return _baseAddresses.GetOrAdd(typeof(TService), t => new List<PathString>());
        }

        //private int GetFreePort()
        //{
        //    var listener = new TcpListener(IPAddress.Loopback, 0);
        //    listener.Start();
        //    var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        //    listener.Stop();
        //    return port;
        //}
    }
}
