namespace IGP.Tools.EmulatorCore
{
    public interface IDeviceEmulator
    {
        bool IsTimeIncluded { get; set; }

        bool IsStarted { get; }

        void Start();

        void Stop();
    }
}