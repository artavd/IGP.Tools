namespace IGP.Tools.IO
{
    using System;
    using System.Threading.Tasks;
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

        Task<bool> Transmit([NotNull] byte[] data);

        void Connect();

        void Disconnect();
    }
}