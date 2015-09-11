namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.IO;
    using System.Reactive.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;

    public class FilePort : PortBase
    {
        private Stream _transmitStream;

        public FilePort([NotNull] string filename)
        {
            Contract.ArgumentIsNotNull(filename, () => filename);
            Contract.ArgumentSatisfied(filename, () => filename, x => !string.IsNullOrWhiteSpace(x));

            OutputFilePath = Path.GetFullPath(filename);
        }

        public string OutputFilePath { get; }

        public override string Type => WellKnownPortTypes.FilePort;
        public override string Name => $"File Port [{OutputFilePath}]";

        protected override void ConnectImplementation()
        {
            _transmitStream = new FileStream(OutputFilePath, FileMode.OpenOrCreate);

            if (_transmitStream.CanWrite)
            {
                ChangeState(PortStates.Connected);
            }
            else
            {
                var error = new PortState(
                    name: "file connecting error",
                    description: $"{Name} cannot open file to write",
                    isError: true,
                    canTransmit: false);

                ChangeState(error);
            }
        }

        protected async override void DisconnectImplementation()
        {
            await _transmitStream.FlushAsync();
            _transmitStream.Close();

            ChangeState(PortStates.Disconnected);
        }

        protected override IObservable<byte> ReceivedImplementation => Observable.Empty<byte>();

        protected async override void TransmitImplementation(byte[] data)
        {
            await _transmitStream.WriteAsync(data, 0, data.Length);
            await _transmitStream.FlushAsync();
        }

        protected override void Dispose(bool disposing)
        {
            Disconnect();
        }
    }
}