namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.IO;
    using System.Reactive.Linq;
    using SBL.Common;
    using SBL.Common.Extensions;

    internal class FilePort : PortBase
    {
        private readonly string _filename;

        private Stream _transmitStream;

        public FilePort(string filename)
        {
            Contract.ArgumentIsNotNull(filename, () => filename);

            _filename = filename;
        }

        public override bool IsOpened
        {
            get { return _transmitStream.Eval(s => s.CanWrite, () => false); }
        }

        protected override void OpenImplementation()
        {
            _transmitStream = new FileStream(_filename, FileMode.OpenOrCreate);
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