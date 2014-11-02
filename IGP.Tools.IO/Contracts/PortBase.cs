namespace IGP.Tools.IO
{
    using System;
    using System.Reactive.Linq;
    using IGP.Tools.IO.Implementation;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public abstract class PortBase : IPort
    {
        private readonly FilterChain _inFilter = new FilterChain();
        private readonly FilterChain _outFilter = new FilterChain();

        public abstract string Type { get; }

        public abstract string Name { get; }

        public abstract bool IsOpened { get; }

        public IObservable<byte[]> Received
        {
            get
            {
                CheckOnDisposed();
                return ReceivedImplementation.Select(_inFilter.Filter).Where(x => x != null);
            }
        }

        public void Transmit(byte[] data)
        {
            Contract.ArgumentIsNotNull(data, () => data);
            
            CheckOnDisposed();

            if (!IsOpened)
            {
                throw new InvalidOperationException("Only opened port can transmit data.");
            }

            var dataForTransmit = _outFilter.Eval(x => x.Filter(data), () => data);
            if (dataForTransmit == null)
            {
                return;
            }

            TransmitImplementation(dataForTransmit);
        }

        public virtual void Open()
        {
            CheckOnDisposed();
            if (!IsOpened)
            {
                OpenImplementation();
            }
        }

        public virtual void Close()
        {
            CheckOnDisposed();
            if (IsOpened)
            {
                CloseImplementation();
            }
        }

        public void AddInputFilter(IPortFilter filter)
        {
            Contract.ArgumentIsNotNull(filter, () => filter);

            CheckOnDisposed();
            _inFilter.AddFilter(filter);
        }

        public void AddOutputFilter(IPortFilter filter)
        {
            Contract.ArgumentIsNotNull(filter, () => filter);

            CheckOnDisposed();
            _outFilter.AddFilter(filter);
        }

        protected abstract void OpenImplementation();

        protected abstract void CloseImplementation();

        [NotNull]
        protected abstract IObservable<byte[]> ReceivedImplementation { get; }

        protected abstract void TransmitImplementation([NotNull] byte[] data);

        #region IDisposable
        private bool _isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            _isDisposed = true;

            GC.SuppressFinalize(this);
        }

        ~PortBase()
        {
            Dispose(false);
            _isDisposed = true;
        }

        protected abstract void Dispose(bool disposing);

        private void CheckOnDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(Name);
            }
        }
        #endregion
    }
}