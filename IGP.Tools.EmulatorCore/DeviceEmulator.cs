namespace IGP.Tools.EmulatorCore
{
    using System;

    internal class DeviceEmulator : IDevice
    {
        public IObservable<byte[]> Messages
        {
            get; private set;
        }


    }
}