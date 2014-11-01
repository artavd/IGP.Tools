namespace IGP.Tools.DeviceEmulator
{
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class DeviceEmulatorApplication
    {
        private readonly ApplicationOptions _options;

        private readonly IDeviceFactory _deviceFactory;
        private readonly IPortFactory _portFactory;

        public DeviceEmulatorApplication(
            [NotNull] ApplicationOptions options,
            [NotNull] IDeviceFactory deviceFactory,
            [NotNull] IPortFactory portFactory)
        {
            Contract.ArgumentIsNotNull(options, () => options);
            Contract.ArgumentIsNotNull(deviceFactory, () => deviceFactory);
            Contract.ArgumentIsNotNull(portFactory, () => portFactory);

            _options = options;
            _deviceFactory = deviceFactory;
            _portFactory = portFactory;
        }

        public void Start()
        {
        }
    }
}