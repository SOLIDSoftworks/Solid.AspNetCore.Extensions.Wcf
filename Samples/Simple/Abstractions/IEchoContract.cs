using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Simple.Abstractions
{
    [ServiceContract]
    public interface IEchoContract
    {
        [OperationContract]
        string EchoString(string value);
    }
}
