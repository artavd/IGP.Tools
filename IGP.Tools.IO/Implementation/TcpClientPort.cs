namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public class TcpClientPort : PortBase
    {
        private static readonly TimeSpan ReconnectTimeout = TimeSpan.FromSeconds(5);
        private const int MaxReconnectTryCount = 3;
        private const int ReadDataChunkSize = 1024;

        private readonly ManualResetEventSlim _readEvent = new ManualResetEventSlim(false);
        
        private readonly IPEndPoint _remoteHost;
        private TcpClient _client = null;
        private NetworkStream _dataStream = null;

        private readonly IConnectableObservable<byte> _dataFeed;

        private int _reconnectTryCount = 0;

        public TcpClientPort([NotNull] IPEndPoint remoteHost)
        {
            Contract.ArgumentIsNotNull(remoteHost, () => remoteHost);

            _remoteHost = remoteHost;

            EnablePort(false);

            Func<Task<IObservable<byte>>> dataProvider = ReadData;
            _dataFeed = dataProvider.DeferRepeat().Publish();
            _dataFeed.Connect();
        }

        public override string Type => WellKnownPortTypes.TcpClientPort;
        public override string Name => $"TCP Client Port [{_remoteHost}]";

        protected override IObservable<byte> ReceivedImplementation => _dataFeed;

        protected override async void ConnectImplementation()
        {
            if (CurrentState == PortStates.Connecting) return;

            try
            {
                ChangeState(PortStates.Connecting);

                _client = new TcpClient();
                await _client.ConnectAsync(_remoteHost.Address, _remoteHost.Port);

                _dataStream = _client.GetStream();
                _reconnectTryCount = 0;

                ChangeState(PortStates.Connected);
                EnablePort(true);
            }
            catch (SocketException ex)
            {
                ChangeState(PortStates.UnableToConnect);
                Reconnect(ex);
            }
        }

        protected override void DisconnectImplementation()
        {
            ChangeState(PortStates.Disconnecting);
            EnablePort(false);
            ChangeState(PortStates.Disconnected);
        }

        protected override async Task<bool> TransmitImplementation(byte[] data)
        {
            if (!CurrentState.CanTransmit)
            {
                throw new InvalidOperationException(string.Format(
                    "Cannot transmit in port '{0}' while its state is '{1}'", Name, CurrentState));
            }

            try
            {
                await _dataStream.WriteAsync(data, 0, data.Length);
                return true;
            }
            catch (IOException ex)
            {
                if (ex.InnerException.GetType() == typeof (SocketException))
                {
                    ChangeState(PortStates.ConnectionLost);
                    Reconnect(ex.InnerException);
                }

                ChangeState(PortStates.UnknownErrorOccur.WithDescription(
                    "Unknown error occur while transmitting"));

                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            _readEvent.Dispose();
            _client.DisposeIfPossible();
            _dataFeed.DisposeIfPossible();
        }

        private async Task<IObservable<byte>> ReadData()
        {
            await WaitReadEnabled();

            try
            {
                var buffer = new byte[ReadDataChunkSize];
                int readBytes = await _dataStream.ReadAsync(buffer, 0, buffer.Length);

                if (readBytes == 0)
                {
                    ChangeState(PortStates.ConnectionLost);
                    Reconnect();
                }

                return buffer.Take(readBytes).ToObservable();
            }
            catch (ObjectDisposedException)
            {
                // To not break observable collection when port closed
                return Observable.Empty<byte>();
            }
        }

        private void Reconnect(Exception reason = null)
        {
            if (CurrentState == PortStates.Connecting ||
                CurrentState == PortStates.Disconnecting ||
                CurrentState == PortStates.Disconnected)
            {
                return;
            }

            ChangeState(PortStates.ConnectionLost.WithData(reason));
            EnablePort(false);

            if (++_reconnectTryCount > MaxReconnectTryCount)
            {
                ChangeState(PortStates.Disconnected);
                return;
            }

            Thread.Sleep(ReconnectTimeout);
            Connect();
        }

        private void EnablePort(bool enable)
        {
            if (enable)
            {
                _readEvent.Set();
            }
            else
            {
                _readEvent.Reset();
                _client.DisposeIfPossible();
                _client = null;
                _dataStream = null;
            }
        }

        private async Task WaitReadEnabled()
        {
            await Task.Factory.StartNew(() => _readEvent.Wait());
        }
    }
}