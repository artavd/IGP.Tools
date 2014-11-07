namespace IGP.Tools.IO
{
    using System;
    using SBL.Common.Annotations;

    public interface IPort : IDisposable
    {
        [NotNull]
        string Type { get; }

        [NotNull]
        string Name { get; }

        bool IsOpened { get; }

        [NotNull]
        IObservable<bool> StateStream { get; }

        [NotNull]
        IObservable<byte[]> ReceivedStream { get; } 

        void Transmit([NotNull] byte[] data);

        void Open();

        void Close();

        void AddInputFilter([NotNull] IPortFilter filter);

        void AddOutputFilter([NotNull] IPortFilter filter);
    }
}