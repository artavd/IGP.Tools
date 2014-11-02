namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using SBL.Common.Extensions;

    public class ConsolePort : PortBase
    {
        private static readonly Lazy<IObservable<byte[]>> ReceivedStream =
            new Lazy<IObservable<byte[]>>(() =>
            {
                Func<bool, ConsoleKeyInfo> consoleReadFunc = Console.ReadKey;
                var published = Observable
                    .Defer(() => consoleReadFunc.ToTask(true).ToObservable())
                    .Select(key => new[] { (byte)key.KeyChar })
                    .Repeat()
                    .Publish();

                published.Connect();

                return published;
            });

        private bool _isOpened = false;

        public override string PortType
        {
            get { return WellKnownPortTypes.ConsolePort; }
        }

        public override bool IsOpened
        {
            get { return _isOpened; }
        }

        protected override void OpenImplementation()
        {
            _isOpened = true;
        }

        protected override void CloseImplementation()
        {
            _isOpened = false;
        }

        protected override IObservable<byte[]> ReceivedImplementation
        {
            get { return ReceivedStream.Value.Where(_ => IsOpened); }
        }

        protected override void TransmitImplementation(byte[] data)
        {
            data.Select(x => (char)x).Foreach(Console.Write);
        }

        protected override void Dispose(bool disposing)
        {
            Close();
        }
    }
}