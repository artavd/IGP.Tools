namespace IGP.Tools.DeviceEmulatorManager.Models
{
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class DeviceEmulatorInfo
    {
        public IDevice Device { get; private set; }

        public IDeviceEmulator Emulator { get; private set; }

        public IPort OutputPort { get; private set; }

        public DeviceEmulatorInfo([NotNull] IDevice device, [CanBeNull] IPort port)
        {
            Contract.ArgumentIsNotNull(device, () => device);
            Contract.OfType<IDeviceEmulator>(device);

            Device = device;
            Emulator = device.As<IDeviceEmulator>();
            OutputPort = port;
        }
    }
}