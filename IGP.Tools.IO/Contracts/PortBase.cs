namespace IGP.Tools.IO
{
    using System;
    using System.Reactive.Subjects;
    using SBL.Common;
    using SBL.Common.Annotations;

    public abstract class PortBase : IPort
    {
        private readonly BehaviorSubject<PortState> _stateSubject = 
            new BehaviorSubject<PortState>(WellKnownPortStates.Disconnected);

        public abstract string Type { get; }

        public abstract string Name { get; }

        public PortState CurrentState
        {
            get { return _stateSubject.Value; }
        }

        public IObservable<PortState> StateFeed
        {
            get
            {
                CheckOnDisposed();
                return _stateSubject;
            }
        }

        public IObservable<byte> ReceivedFeed
        {
            get
            {
                CheckOnDisposed();
                return ReceivedImplementation;
            }
        }

        public void Transmit(byte[] data)
        {
            Contract.ArgumentIsNotNull(data, () => data);
            
            CheckOnDisposed();

            if (!CurrentState.CanTransmit)
            {
                throw new InvalidOperationException("Only opened port can transmit data.");
            }

            TransmitImplementation(data);
        }

        public virtual void Connect()
        {
            CheckOnDisposed();
            if (CurrentState != WellKnownPortStates.Connected)
            {
                ConnectImplementation();
            }
        }

        public virtual void Disconnect()
        {
            CheckOnDisposed();
            if (CurrentState != WellKnownPortStates.Disconnected)
            {
                DisconnectImplementation();
            }
        }

        protected void ChangeState(PortState newState)
        {
            _stateSubject.OnNext(newState);
        }

        protected abstract void ConnectImplementation();

        protected abstract void DisconnectImplementation();

        [NotNull]
        protected abstract IObservable<byte> ReceivedImplementation { get; }

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