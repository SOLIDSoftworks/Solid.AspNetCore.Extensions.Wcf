using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Channels
{
    internal abstract class AsyncChannelListenerBase<TChannel> : ChannelListenerBase<TChannel>
        where TChannel : class, IChannel
    {
        public override Uri Uri { get; }
        
        protected AsyncChannelListenerBase(IDefaultCommunicationTimeouts timeouts, Uri uri) 
            : base(timeouts)
        {
            Uri = uri;
        }

        protected override void OnAbort()
        {
        }

        protected abstract Task<TChannel> OnAcceptChannelAsync(TimeSpan timeout);
        protected abstract Task OnCloseAsync(TimeSpan timeout);
        protected abstract Task OnOpenAsync(TimeSpan timeout);
        protected abstract Task<bool> OnWaitForChannelAsync(TimeSpan timeout);

        protected override TChannel OnAcceptChannel(TimeSpan timeout)
        {
            return OnAcceptChannelAsync(timeout).Result;
        }

        protected override void OnClose(TimeSpan timeout)
        {
            OnCloseAsync(timeout).Wait();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
            OnOpenAsync(timeout).Wait();
        }

        protected override bool OnWaitForChannel(TimeSpan timeout)
        {
            return OnWaitForChannelAsync(timeout).Result;
        }

        protected override IAsyncResult OnBeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return OnAcceptChannelAsync(timeout).ToAsyncResult(callback, state);
        }

        protected override TChannel OnEndAcceptChannel(IAsyncResult result)
        {
            var task = result as Task<TChannel>;
            return task?.Result;
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

        protected override IAsyncResult OnBeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return OnWaitForChannelAsync(timeout).ToAsyncResult(callback, state);
        }

        protected override bool OnEndWaitForChannel(IAsyncResult result)
        {
            var task = result as Task<bool>;
            return task?.Result ?? false;
        }
    }
}
