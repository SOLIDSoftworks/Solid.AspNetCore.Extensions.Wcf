using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Abstractions
{
    internal interface IBindingSanitizer
    {
        Binding SanitizeBinding(Binding binding);
    }
}
