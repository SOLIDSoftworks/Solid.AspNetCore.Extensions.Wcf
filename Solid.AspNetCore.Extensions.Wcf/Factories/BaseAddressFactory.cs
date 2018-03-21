using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Solid.AspNetCore.Extensions.Wcf.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Factories
{
    internal class BaseAddressFactory : IBaseAddressFactory
    {
        private IServer _server;

        public BaseAddressFactory(IServer server)
        {
            _server = server;
        }

        public IEnumerable<Uri> Create(string path)
        {
            return Create(path, false);
        }

        public IEnumerable<Uri> Create(string path, bool direct)
        {
            if (direct)
                return GetHostAddresses().Select(u => new Uri(u, path));

            var host = new Uri($"http://localhost:{GetFreePort()}");
            if (!path.EndsWith("/"))
                path = path + "/";
            return new[] { new Uri(host, path) };
        }

        private IEnumerable<Uri> GetHostAddresses()
        {
            var addresses = _server.Features[typeof(IServerAddressesFeature)] as IServerAddressesFeature;

            if (addresses == null)
                return Enumerable.Empty<Uri>();

            return addresses.Addresses.Select(a => new Uri(a));
        }

        private int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
