namespace IGP.Tools.IO.Contracts
{
    using System;

    public interface IPort
    {
        bool IsOpened { get; }

        IObservable<byte[]> Received { get; } 

        void Transmit(byte[] data);

        void Open();

        void Close();

        void AddInputFilter(IPortFilter filter);

        void AddOutputFilter(IPortFilter filter);
    }
}