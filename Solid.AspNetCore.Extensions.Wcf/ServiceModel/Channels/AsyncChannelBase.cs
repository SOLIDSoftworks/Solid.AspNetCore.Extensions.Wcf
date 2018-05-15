using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel.Channels
{
    internal abstract class AsyncChannelBase : ChannelBase
    {
        protected AsyncChannelBase(ChannelManagerBase manager) 
            : base(manager)
        {
        }

        protected override void OnAbort()
        {
        }

        protected abstract Task OnOpenAsync(TimeSpan timeout);
        protected abstract Task OnCloseAsync(TimeSpan timeout);
        
        protected override void OnClose(TimeSpan timeout)
        {
            OnCloseAsync(timeout).Wait();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            OnOpenAsync(timeout).Wait();
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return OnCloseAsync(timeout).ToAsyncResult(callback, state);
        }

        protected override void OnEndClose(IAsyncResult result)
        {
            var task = result as Task;
            // TODO: do something here?
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return OnOpenAsync(timeout).ToAsyncResult(callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            var task = result as Task;
            // TODO: do something here?
        }
    }
}
