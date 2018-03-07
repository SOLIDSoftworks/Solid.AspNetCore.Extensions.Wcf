using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Models
{
    internal class ServiceEndpointDescription
    {
        public Type Contract { get; set; }
        public string Path { get; set; }
        public Binding Binding { get; set; }
    }
}
