using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels
{
    internal abstract class AsyncReplyChannelBase : AsyncChannelBase, IReplyChannel
    {
        protected AsyncReplyChannelBase(ChannelManagerBase manager) 
            : base(manager)
        {
        }

        public abstract EndpointAddress LocalAddress { get; }
        public abstract Task<RequestContext> ReceiveRequestAsync();
        public abstract Task<RequestContext> ReceiveRequestAsync(TimeSpan timeout);
        public abstract Task<bool> WaitForRequestAsync(TimeSpan timeout);


        public RequestContext ReceiveRequest()
        {
            return ReceiveRequestAsync().Result;
        }

        public RequestContext ReceiveRequest(TimeSpan timeout)
        {
            return ReceiveRequestAsync(timeout).Result;
        }

        public IAsyncResult BeginReceiveRequest(AsyncCallback callback, object state)
        {
            return ReceiveRequestAsync().ToAsyncResult(callback, state);
        }

        public IAsyncResult BeginReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return ReceiveRequestAsync(timeout).ToAsyncResult(callback, state);
        }

        public RequestContext EndReceiveRequest(IAsyncResult result)
        {
            var task = result as Task<RequestContext>;
            return task?.Result;
        }        

        public bool TryReceiveRequest(TimeSpan timeout, out RequestContext context)
        {
            var c = ReceiveRequestAsync(timeout).Result;
            context = c;
            return c != null;
        }

        public IAsyncResult BeginTryReceiveRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return ReceiveRequestAsync(timeout).ToAsyncResult(callback, state);
        }

        public bool EndTryReceiveRequest(IAsyncResult result, out RequestContext context)
        {
            var task = result as Task<RequestContext>;
            context = task?.Result;
            return task?.Result != null;
        }


        public bool WaitForRequest(TimeSpan timeout)
        {
            return WaitForRequestAsync(timeout).Result;
        }

        public IAsyncResult BeginWaitForRequest(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return WaitForRequestAsync(timeout).ToAsyncResult(callback, state);
        }

        public bool EndWaitForRequest(IAsyncResult result)
        {
            var task = result as Task<bool>;
            return task?.Result ?? false;
        }
    }
}
