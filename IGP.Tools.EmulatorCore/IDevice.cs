namespace IGP.Tools.EmulatorCore
{
    using System;

    public interface IDevice
    {
        IObservable<byte[]> Messages { get; }
    }
}