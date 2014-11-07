namespace IGP.Tools.IO.Implementation.Creators
{
    using System;
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

            CheckPortName<FilePort>(portName);

            try
            {
                return new FilePort(parameters);
            }
            catch (ArgumentException e)
            {
                throw new FactoryException(this.GetType(), typeof (FilePort), "Cannot create file port", parameters);
            }
        }

        protected override Regex GetMatchingRegex()
        {
            return new Regex(Type.ToLower());
        }
    }
}