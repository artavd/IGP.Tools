namespace IGP.Tools.EmulatorCore.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IDevice
    {
        string Name { get; }

        IList<IObservable<byte[]>> Messages { get; }
    }
}