namespace IGP.Tools.IO.Implementation
{
    using System;

    internal class ConsolePort : PortBase
    {
        public override bool IsOpened
        {
            get { throw new NotImplementedException(); }
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        protected override IObservable<byte[]> ReceivedImplementation
        {
            get { throw new NotImplementedException(); }
        }

        protected override void TransmitImplementation(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}