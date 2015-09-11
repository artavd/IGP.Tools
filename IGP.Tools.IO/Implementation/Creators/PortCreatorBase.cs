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

        protected string Type { get; }

        public abstract IPort CreatePort(string portName, string parameters);

        public bool CanBeCreatedFrom(string portName)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);

            return GetMatchingRegex().IsMatch(portName.ToLower());
        }

        protected virtual Regex GetMatchingRegex() => new Regex($"^{Type.ToLower()}([0-9]+)$");

        protected void CheckPortName<T>([NotNull] string portName) where T : IPort
        {
            if (!CanBeCreatedFrom(portName))
            {
                string message = $"{Type} port cannot be created from {portName}";
                throw new FactoryException(GetType(), typeof (T), message);
            }
        }
    }
}