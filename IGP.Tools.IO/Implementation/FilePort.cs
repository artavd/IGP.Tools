namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.IO;
    using System.Reactive.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

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

        public override string PortType
        {
            get { return WellKnownPortTypes.FilePort; }
        }

        public override bool IsOpened
        {
            get { return _transmitStream.Eval(s => s.CanWrite, () => false); }
        }

        protected override void OpenImplementation()
        {
            _transmitStream = new FileStream(OutputFilePath, FileMode.OpenOrCreate);
        }

        protected async override void CloseImplementation()
        {
            await _transmitStream.FlushAsync();
            _transmitStream.Close();
        }

        protected override IObservable<byte[]> ReceivedImplementation
        {
            get { return Observable.Empty<byte[]>(); }
        }

        protected override void TransmitImplementation(byte[] data)
        {
            _transmitStream.WriteAsync(data, 0, data.Length);
        }

        protected override void Dispose(bool disposing)
        {
            Close();
        }
    }
}