namespace IGP.Tools.IO
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using IGP.Tools.IO.Implementation;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public abstract class PortBase : IPort
    {
        private readonly FilterChain _inFilter = new FilterChain();
        private readonly FilterChain _outFilter = new FilterChain();

        private readonly BehaviorSubject<bool> _stateSubject = new BehaviorSubject<bool>(false);

        public abstract string Type { get; }

        public abstract string Name { get; }

        public bool IsOpened
        {
            get { return _stateSubject.Value; }
        }

        public IObservable<bool> StateStream
        {
            get
            {
                CheckOnDisposed();
                return _stateSubject;
            }
        }

        public IObservable<byte[]> ReceivedStream
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

        protected void ChangeState(bool isOpened)
        {
            _stateSubject.OnNext(isOpened);
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stateSubject.Dispose();
            }
        }

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