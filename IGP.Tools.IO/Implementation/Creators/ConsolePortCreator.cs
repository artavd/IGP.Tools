namespace IGP.Tools.IO.Implementation.Creators
{
    using System.Text.RegularExpressions;
    using SBL.Common;

    internal class ConsolePortCreator : PortCreatorBase
    {
        public ConsolePortCreator() : base(WellKnownPortTypes.ConsolePort)
        {
        }

        public override IPort CreatePort(string portName, string parameters)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);
            
            CheckPortName<ConsolePort>(portName);

            return new ConsolePort();
        }

        protected override Regex GetMatchingRegex() => new Regex(Type.ToLower());
    }
}