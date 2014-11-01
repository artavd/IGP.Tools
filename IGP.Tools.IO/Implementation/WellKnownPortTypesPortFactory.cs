namespace IGP.Tools.IO.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IGP.Tools.IO.Implementation.Creators;
    using SBL.Common;

    internal class WellKnownPortTypesPortFactory : IPortFactory
    {
        private readonly IDictionary<string, PortCreatorBase> _portCreators = 
            new Dictionary<string, PortCreatorBase>();

        public WellKnownPortTypesPortFactory()
        {
            _portCreators.Add(WellKnownPortTypes.FilePort, new FilePortCreator());
        }

        public IPort CreatePort(string portName, string parameters)
        {
            Contract.ArgumentIsNotNull(portName, () => portName);

            var creator = _portCreators.Values.SingleOrDefault(x => x.CanBeCreatedFrom(portName));
            if (creator == null)
            {
                // TODO: #12 Own factory exception type
                throw new ArgumentException(
                    string.Format("Unable to creator port with '{0}' name", portName),
                    portName);
            }

            return creator.CreatePort(portName, parameters);
        }
    }
}