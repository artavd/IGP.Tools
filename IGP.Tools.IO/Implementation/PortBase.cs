namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.Reactive.Linq;
    using IGP.Tools.IO.Contracts;
    using SBL.Common;
    using SBL.Common.Extensions;

    internal abstract class PortBase : IPort, IDisposable
    {
        protected PortBase()
        {
            InFilter = new FilterChain();
            OutFilter = new FilterChain();
        }

        public abstract bool IsOpened { get; }

        public IObservable<byte[]> Received
        {
            get { return ReceivedImplementation.Select(InFilter.Filter).Where(x => x != null); }
        }

        public void Transmit(byte[] data)
        {
            Contract.ArgumentIsNotNull(data, () => data);

            if (!IsOpened)
            {
                throw new InvalidOperationException("Only opened port can transmit data.");
            }

            byte[] dataForTransmit = OutFilter.Eval(x => x.Filter(data), () => data);
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

            InFilter.AddFilter(filter);
        }

        public void AddOutputFilter(IPortFilter filter)
        {
            Contract.ArgumentIsNotNull(filter, () => filter);

            OutFilter.AddFilter(filter);
        }

        protected FilterChain InFilter { get; private set; }

        protected FilterChain OutFilter { get; private set; }

        protected abstract void OpenImplementation();

        protected abstract void CloseImplementation();

        protected abstract IObservable<byte[]> ReceivedImplementation { get; }

        protected abstract void TransmitImplementation(byte[] data);

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