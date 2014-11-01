namespace IGP.Tools.IO.Implementation.Creators
{
    using System.Text.RegularExpressions;
    using SBL.Common;

    internal class FilePortCreator : PortCreatorBase
    {
        public FilePortCreator() : base(WellKnownPortTypes.FilePort)
        {
        }

        public override IPort CreatePort(string portName, string parameters)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);
            Contract.ArgumentSatisfied(portName, () => portName, CanBeCreatedFrom);

            Contract.IsNotNull(parameters, () => "Unable to create FilePort with unspecified file name.");

            return new FilePort(parameters);
        }

        protected override Regex GetMatchingRegex()
        {
            return new Regex(Type.ToLower());
        }
    }
}