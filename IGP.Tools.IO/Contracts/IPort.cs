namespace IGP.Tools.IO
{
    using System;
    using SBL.Common.Annotations;

    public interface IPort
    {
        [NotNull]
        string PortType { get; }

        bool IsOpened { get; }

        [NotNull]
        IObservable<byte[]> Received { get; } 

        void Transmit([NotNull] byte[] data);

        void Open();

        void Close();

        void AddInputFilter([NotNull] IPortFilter filter);

        void AddOutputFilter([NotNull] IPortFilter filter);
    }
}