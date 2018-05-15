using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels
{
    internal abstract class AsyncRequestContextBase : RequestContext
    {
        protected AsyncRequestContextBase()
            : base()
        {
        }

        public abstract Task ReplyAsync(Message message);
        public abstract Task ReplyAsync(Message message, TimeSpan timeout);

        public override void Reply(Message message)
        {
            ReplyAsync(message).Wait();
        }

        public override void Reply(Message message, TimeSpan timeout)
        {
            ReplyAsync(message, timeout).Wait();
        }

        public override IAsyncResult BeginReply(Message message, AsyncCallback callback, object state)
        {
            return ReplyAsync(message).ToAsyncResult(callback, state);
        }

        public override IAsyncResult BeginReply(Message message, TimeSpan timeout, AsyncCallback callback, object state)
        {
            return ReplyAsync(message, timeout).ToAsyncResult(callback, state);
        }

        public override void EndReply(IAsyncResult result)
        {
            var task = result as Task;
            // TODO: do something here?
        }
    }
}
