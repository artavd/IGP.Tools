namespace IGP.Tools.IO
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;

    public class DummyPort : IPort
    {
        private static readonly PortState DummyState = new PortState(
            name: "dummy",
            description: "Dummy port [/dev/null]",
            isError: false,
            canTransmit: false);

        private static readonly Lazy<DummyPort> _instance = new Lazy<DummyPort>(() => new DummyPort());

        public static DummyPort Instance => _instance.Value;

        private DummyPort() { }

        public string Type => WellKnownPortTypes.DummyPort;
        public string Name => "[not set]";

        public PortState CurrentState => DummyState;

        public IObservable<PortState> StateFeed { get; } = new BehaviorSubject<PortState>(DummyState);
        public IObservable<byte> ReceivedFeed { get; } = Observable.Never<byte>();

        public Task<bool> Transmit(byte[] data)
        {
            return Task.FromResult(true);
        }

        public void Connect() { }
        public void Disconnect() { }
        public void Dispose() { }
    }
}