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

        public string OutputFilePath { get; private set; }

        public override string Type
        {
            get { return WellKnownPortTypes.FilePort; }
        }

        public override string Name
        {
            get { return string.Format("File Port [{0}]", OutputFilePath); }
        }

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
                    description: string.Format("{0} cannot open file to write", Name),
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

        protected override IObservable<byte> ReceivedImplementation
        {
            get { return Observable.Empty<byte>(); }
        }

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