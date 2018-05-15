//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using System.Text;
//using System.Threading.Tasks;

//namespace Solid.AspNetCore.Extensions.Wcf.Channels
//{
//    internal abstract class AsyncCommunicationObject : ICommunicationObject, IDefaultCommunicationTimeouts
//    {
//        private IDefaultCommunicationTimeouts _timeouts;
//        protected AsyncCommunicationObject(IDefaultCommunicationTimeouts timeouts)
//        {
//            _timeouts = timeouts;
//        }

//        public CommunicationState State { get; private set; }

//        public TimeSpan CloseTimeout => _timeouts.CloseTimeout;

//        public TimeSpan OpenTimeout => _timeouts.OpenTimeout;

//        public TimeSpan ReceiveTimeout => _timeouts.ReceiveTimeout;

//        public TimeSpan SendTimeout => _timeouts.SendTimeout;

//        public event EventHandler Closed;
//        public event EventHandler Closing;
//        public event EventHandler Faulted;
//        public event EventHandler Opened;
//        public event EventHandler Opening;

//        protected abstract Task<bool> OnWaitForChannelAsync(TimeSpan timeout);
//        protected abstract Task OnOpenAsync(TimeSpan timeout);
//        protected abstract Task OnCloseAsync(TimeSpan timeout);
//        protected abstract void OnAbort();

//        public bool WaitForChannel(TimeSpan timeout)
//        {
//            return WaitForChannelAsync(timeout).Result;
//        }

//        public IAsyncResult BeginWaitForChannel(TimeSpan timeout, AsyncCallback callback, object state)
//        {
//            return WaitForChannelAsync(timeout).ToAsyncResult(callback, state);
//        }

//        public bool EndWaitForChannel(IAsyncResult result)
//        {
//            var task = result as Task<bool>;
//            return task?.Result ?? false;
//        }

//        public void Close()
//        {
//            CloseAsync().Wait();
//        }

//        public IAsyncResult BeginClose(AsyncCallback callback, object state)
//        {
//            return CloseAsync().ToAsyncResult(callback, state);
//        }

//        public void Close(TimeSpan timeout)
//        {
//            CloseAsync(timeout).Wait();
//        }

//        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
//        {
//            return CloseAsync(timeout).ToAsyncResult(callback, state);
//        }

//        public void EndClose(IAsyncResult result)
//        {
//            var task = result as Task;
//            // TODO: Do something?
//        }

//        public void Open()
//        {
//            OpenAsync().Wait();
//        }

//        public void Open(TimeSpan timeout)
//        {
//            OpenAsync(timeout).Wait();
//        }

//        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
//        {
//            return OpenAsync().ToAsyncResult(callback, state);
//        }

//        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
//        {
//            return OpenAsync(timeout).ToAsyncResult(callback, state);
//        }

//        public void EndOpen(IAsyncResult result)
//        {
//            var task = result as Task;
//            // TODO: Do something?
//        }

//        //public T GetProperty<T>() where T : class
//        //{
//        //    throw new NotImplementedException();
//        //}

//        private Task<bool> WaitForChannelAsync(TimeSpan timeout)
//        {
//            return OnWaitForChannelAsync(timeout);
//        }
//        private Task OpenAsync()
//        {
//            return OpenAsync(OpenTimeout);
//        }
//        private async Task OpenAsync(TimeSpan timeout)
//        {
//            State = CommunicationState.Opening;
//            if (Opening != null)
//                Opening(this, EventArgs.Empty);
//            await OnOpenAsync(timeout);
//            State = CommunicationState.Opened;
//            if (Opened != null)
//                Opened(this, EventArgs.Empty);
//        }
//        private Task CloseAsync()
//        {
//            return CloseAsync(CloseTimeout);
//        }
//        private async Task CloseAsync(TimeSpan timeout)
//        {
//            State = CommunicationState.Closing;
//            if (Closing != null)
//                Closing(this, EventArgs.Empty);
//            await OnCloseAsync(timeout);
//            State = CommunicationState.Closed;
//            if (Closed != null)
//                Closed(this, EventArgs.Empty);

//        }

//        public void Abort()
//        {
//            OnAbort();
//        }
//    }
//}
