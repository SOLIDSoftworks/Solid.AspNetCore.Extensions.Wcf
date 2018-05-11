using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Channels
{
    internal abstract class AsyncChannelListenerBase<TChannel> : AsyncCommunicationObject, IChannelListener<TChannel>, IDefaultCommunicationTimeouts
        where TChannel : class, IChannel
    {
        public Uri Uri { get; }
        
        protected AsyncChannelListenerBase(IDefaultCommunicationTimeouts timeouts, Uri uri) 
            : base(timeouts)
        {
            Uri = uri;
        }

        protected virtual Task<TChannel> OnAcceptChannelAsync()
        {
            return AcceptChannelAsync(ReceiveTimeout);
        }

        protected abstract Task<TChannel> OnAcceptChannelAsync(TimeSpan timeout);
        public abstract T GetProperty<T>() where T : class;

        public TChannel AcceptChannel()
        {
            return AcceptChannelAsync().Result;
        }

        public TChannel AcceptChannel(TimeSpan timeout)
        {
            return AcceptChannelAsync(timeout).Result;
        }

        public IAsyncResult BeginAcceptChannel(AsyncCallback callback, object state)
        {
            return AcceptChannelAsync().ToAsyncResult(callback, state);
        }

        public IAsyncResult BeginAcceptChannel(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return AcceptChannelAsync(timeout).ToAsyncResult(callback, state);
        }

        public TChannel EndAcceptChannel(IAsyncResult result)
        {
            var task = result as Task<TChannel>;
            return task?.Result;
        }

        private Task<TChannel> AcceptChannelAsync()
        {
            return AcceptChannelAsync(ReceiveTimeout);
        }
        private Task<TChannel> AcceptChannelAsync(TimeSpan timeout)
        {
            return OnAcceptChannelAsync(timeout);
        }

    }
}
