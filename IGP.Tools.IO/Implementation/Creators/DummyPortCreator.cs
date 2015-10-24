namespace IGP.Tools.IO.Implementation.Creators
{
    using System.Text.RegularExpressions;
    using SBL.Common;

    public class DummyPortCreator : PortCreatorBase
    {
        public DummyPortCreator() : base(WellKnownPortTypes.DummyPort)
        {
        }

        public override IPort CreatePort(string portName, string parameters)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);

            CheckPortName<DummyPort>(portName);

            return DummyPort.Instance;
        }

        protected override Regex GetMatchingRegex()
        {
            return new Regex($"({WellKnownPortTypes.DummyPort}|null)", RegexOptions.IgnoreCase);
        }
    }
}