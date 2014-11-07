﻿namespace IGP.Tools.IO.Implementation
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
            _portCreators.Add(WellKnownPortTypes.ConsolePort, new ConsolePortCreator());
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