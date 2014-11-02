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
            Contract.ArgumentSatisfied(portName, () => portName, CanBeCreatedFrom);

            return new ConsolePort();
        }

        protected override Regex GetMatchingRegex()
        {
            return new Regex(Type.ToLower());
        }
    }
}