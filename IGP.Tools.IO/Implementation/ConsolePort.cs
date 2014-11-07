namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading;
    using SBL.Common.Extensions;

    public class ConsolePort : PortBase
    {
        private static readonly object ConsolePortLock = new object();
        private static readonly CancellationTokenSource PortStopper = new CancellationTokenSource();

        private static Lazy<IObservable<byte[]>> s_ReceivedStream = null;

        private static readonly IList<int> OpenedPortNumbers = new List<int>();
        private static int s_LastPortNumber = 0;

        private readonly int _portNumber = s_LastPortNumber + 1;

        public ConsolePort()
        {
            lock (ConsolePortLock)
            {
                OpenedPortNumbers.Add(_portNumber);
                s_LastPortNumber = _portNumber;
                if (s_ReceivedStream == null)
                {
                    s_ReceivedStream = new Lazy<IObservable<byte[]>>(CreateConsoleReadObservable);
                }
            }
        }

        public override string Type
        {
            get { return WellKnownPortTypes.ConsolePort; }
        }

        public override string Name
        {
            get { return string.Format("Console Port [{0}]", _portNumber); }
        }

        protected override void OpenImplementation()
        {
            ChangeState(true);
        }

        protected override void CloseImplementation()
        {
            ChangeState(false);
        }

        protected override void TransmitImplementation(byte[] data)
        {
            data.Select(x => (char)x).Foreach(Console.Write);
        }

        protected override IObservable<byte[]> ReceivedImplementation
        {
            get { return s_ReceivedStream.Value.Where(_ => IsOpened); }
        }

        protected override void Dispose(bool disposing)
        {
            Close();

            lock (ConsolePortLock)
            {
                OpenedPortNumbers.Remove(_portNumber);
                if (s_ReceivedStream != null)
                {
                    PortStopper.Cancel();
                    s_ReceivedStream.DisposeIfPossible();
                    s_ReceivedStream = null;
                }
            }
        }

        private static IObservable<byte[]> CreateConsoleReadObservable()
        {
            Func<object, ConsoleKeyInfo> consoleReadFunc = o => Console.ReadKey((bool)o);
            var published = Observable
                .Defer(() => consoleReadFunc.StartInTask(true, PortStopper.Token).ToObservable())
                .Select(key => new[] { (byte)key.KeyChar })
                .Repeat()
                .Publish();

            published.Connect();

            return published;
        }
    }
}