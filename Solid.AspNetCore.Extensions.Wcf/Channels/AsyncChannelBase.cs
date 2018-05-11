using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Channels
{
    internal abstract class AsyncChannelBase : AsyncCommunicationObject, IChannel, ICommunicationObject, IDefaultCommunicationTimeouts
    {
        protected AsyncChannelBase(IDefaultCommunicationTimeouts timeouts) 
            : base(timeouts)
        {
        }

        public T GetProperty<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
