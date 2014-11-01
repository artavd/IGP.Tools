namespace IGP.Tools.EmulatorCore.Contracts
{
    using System;
    using System.Collections.Generic;
    using SBL.Common.Annotations;

    public interface IDevice
    {
        [NotNull]
        string Name { get; }

        [NotNull] 
        IList<IObservable<byte[]>> Messages { get; }
    }
}