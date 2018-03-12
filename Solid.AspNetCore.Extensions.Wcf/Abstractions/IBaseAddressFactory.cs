using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IBaseAddressFactory
    {
        IEnumerable<Uri> Create(string path);
        IEnumerable<Uri> Create(string path, bool direct);
    }
}
