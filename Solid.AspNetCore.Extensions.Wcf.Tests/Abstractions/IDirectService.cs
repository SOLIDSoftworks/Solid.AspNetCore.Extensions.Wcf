using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Tests.Abstractions
{
    [ServiceContract]
    public interface IDirectService
    {
        [OperationContract]
        bool IsDirect();
    }
}
