namespace IGP.Tools.IO.Implementation.Creators
{
    using System.Text.RegularExpressions;

    using SBL.Common;
    using SBL.Common.Annotations;

    public abstract class PortCreatorBase : IPortCreator
    {
        protected PortCreatorBase([NotNull] string type)
        {
            Contract.ArgumentIsNotNull(type, () => type);

            Type = type;
        }

        protected string Type { get; private set; }

        public abstract IPort CreatePort(string portName, string parameters);

        public bool CanBeCreatedFrom(string portName)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);

            return GetMatchingRegex().IsMatch(portName.ToLower());
        }

        protected virtual Regex GetMatchingRegex()
        {
            return new Regex(string.Format("^{0}([0-9]+)$", Type.ToLower()));
        }

        protected void CheckPortName<T>([NotNull] string portName) where T : IPort
        {
            if (!CanBeCreatedFrom(portName))
            {
                string message = string.Format("{0} port cannot be created from {1}", Type, portName);
                throw new FactoryException(GetType(), typeof (T), message);
            }
        }
    }
}