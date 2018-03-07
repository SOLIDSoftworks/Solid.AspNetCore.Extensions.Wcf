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
        public Uri Create(string path)
        {
            var host = new Uri($"http://localhost:{GetFreePort()}");
            return new Uri(host, path);
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
