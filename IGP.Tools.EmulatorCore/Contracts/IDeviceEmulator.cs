namespace IGP.Tools.EmulatorCore
{
    using System;

    public interface IDeviceEmulator
    {
        event EventHandler StateChanged;

        bool IsTimeIncluded { get; set; }

        bool IsStarted { get; }

        void Start();

        void Stop();
    }
}