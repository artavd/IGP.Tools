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

        PortState CurrentState { get; }

        [NotNull]
        IObservable<PortState> StateFeed { get; }

        [NotNull]
        IObservable<byte> ReceivedFeed { get; }

        void Transmit([NotNull] byte[] data);

        void Connect();

        void Disconnect();
    }
}