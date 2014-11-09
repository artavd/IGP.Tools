namespace IGP.Tools.IO.Implementation.Creators
{
    internal class TcpClientPortCreator : PortCreatorBase
    {
        public TcpClientPortCreator() : base(WellKnownPortTypes.TcpClientPort) { }

        public override IPort CreatePort(string portName, string parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}