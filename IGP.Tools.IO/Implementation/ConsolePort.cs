namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Threading.Tasks;
    using System.Threading;
    using System.Threading.Tasks;
    using SBL.Common.Extensions;

    public class ConsolePort : PortBase
    {
        private static readonly object ConsolePortLock = new object();
        private static readonly CancellationTokenSource PortStopper = new CancellationTokenSource();

        private static Lazy<IObservable<byte>> s_ReceivedStream = null;

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
                    s_ReceivedStream = new Lazy<IObservable<byte>>(CreateConsoleReadObservable);
                }
            }
        }

        public override string Type => WellKnownPortTypes.ConsolePort;
        public override string Name => $"Console Port [{_portNumber}]";

        protected override void ConnectImplementation()
        {
            ChangeState(PortStates.Connected);
        }

        protected override void DisconnectImplementation()
        {
            ChangeState(PortStates.Disconnected);
        }

        protected override Task<bool> TransmitImplementation(byte[] data)
        {
            data.Select(x => (char)x).Foreach(Console.Write);
            return Task.FromResult(true);
        }

        protected override IObservable<byte> ReceivedImplementation
        {
            get { return s_ReceivedStream.Value.Where(_ => CurrentState == PortStates.Connected); }
        }

        protected override void Dispose(bool disposing)
        {
            Disconnect();

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

        private static IObservable<byte> CreateConsoleReadObservable()
        {
            Func<object, ConsoleKeyInfo> consoleReadFunc = o => Console.ReadKey((bool)o);
            Func<IObservable<byte>> consoleDataProvider = () => consoleReadFunc
                .StartInTask(true, PortStopper.Token)
                .ToObservable()
                .Select(key => (byte)key.KeyChar);

            var published = consoleDataProvider.DeferRepeat().Publish();
            published.Connect();

            return published;
        }
    }
}