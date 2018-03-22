using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IBaseAddressProvider
    {
        void AddBaseAddressFor<TService>(PathString path);
        Uri[] GetBaseAddressesFor<TService>();
    }
}
