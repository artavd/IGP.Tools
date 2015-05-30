namespace IGP.Tools.IO.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using IGP.Tools.IO.Implementation.Creators;

    using SBL.Common;
    using SBL.Common.Annotations;

    public sealed class WellKnownPortTypesPortFactory : IPortFactory
    {
        private readonly IDictionary<string, IPortCreator> _portCreators =
            new Dictionary<string, IPortCreator>();

        public WellKnownPortTypesPortFactory()
        {
            RegisterWellKnownPortType(WellKnownPortTypes.FilePort, new FilePortCreator());
            RegisterWellKnownPortType(WellKnownPortTypes.ConsolePort, new ConsolePortCreator());
            RegisterWellKnownPortType(WellKnownPortTypes.TcpClientPort, new TcpClientPortCreator());
        }

        public void RegisterWellKnownPortType([NotNull] string portType, [NotNull] IPortCreator creator)
        {
            Contract.ArgumentIsNotNull(portType, () => portType);
            Contract.ArgumentIsNotNull(creator, () => creator);

            Contract.IsTrue(
                !_portCreators.Keys.Select(k => k.ToLower()).Contains(portType.ToLower()),
                () => string.Format("WellKnownPortTypesFactory already contains creator for '{0}' port type", portType));

            _portCreators.Add(portType, creator);
        }

        public IPort CreatePort(string portName, string parameters)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);

            var creator = _portCreators.Values.SingleOrDefault(x => x.CanBeCreatedFrom(portName));
            if (creator == null)
            {
                throw new FactoryException(
                    typeof (WellKnownPortTypesPortFactory),
                    typeof (IPort),
                    string.Format("Unable to find a creator for port with '{0}' name", portName),
                    parameters);
            }

            return creator.CreatePort(portName, parameters);
        }
    }
}