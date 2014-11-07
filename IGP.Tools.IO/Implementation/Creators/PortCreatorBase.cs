namespace IGP.Tools.IO.Implementation.Creators
{
    using System.Text.RegularExpressions;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal abstract class PortCreatorBase
    {
        protected PortCreatorBase([NotNull] string type)
        {
            Contract.ArgumentIsNotNull(type, () => type);

            Type = type;
        }

        protected string Type { get; private set; }

        [NotNull]
        public abstract IPort CreatePort([NotNull] string portName, [CanBeNull] string parameters);

        public bool CanBeCreatedFrom([NotNull] string portName)
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
                throw new FactoryException(this.GetType(), typeof (T), message);
            }
        }
    }
}