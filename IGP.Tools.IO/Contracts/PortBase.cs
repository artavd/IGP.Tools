namespace IGP.Tools.IO.Contracts
{
    using System;
    using System.Reactive.Linq;
    using IGP.Tools.IO.Implementation;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public abstract class PortBase : IPort, IDisposable
    {
        private readonly FilterChain _inFilter = new FilterChain();
        private readonly FilterChain _outFilter = new FilterChain();

        public abstract bool IsOpened { get; }

        public IObservable<byte[]> Received
        {
            get { return ReceivedImplementation.Select(_inFilter.Filter).Where(x => x != null); }
        }

        public void Transmit(byte[] data)
        {
            Contract.ArgumentIsNotNull(data, () => data);

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
            if (!IsOpened)
            {
                OpenImplementation();
            }
        }

        public virtual void Close()
        {
            if (IsOpened)
            {
                CloseImplementation();
            }
        }

        public void AddInputFilter(IPortFilter filter)
        {
            Contract.ArgumentIsNotNull(filter, () => filter);

            _inFilter.AddFilter(filter);
        }

        public void AddOutputFilter(IPortFilter filter)
        {
            Contract.ArgumentIsNotNull(filter, () => filter);

            _outFilter.AddFilter(filter);
        }

        protected abstract void OpenImplementation();

        protected abstract void CloseImplementation();

        [NotNull]
        protected abstract IObservable<byte[]> ReceivedImplementation { get; }

        protected abstract void TransmitImplementation([NotNull] byte[] data);

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PortBase()
        {
            Dispose(false);
        }

        protected abstract void Dispose(bool disposing);
        #endregion
    }
}