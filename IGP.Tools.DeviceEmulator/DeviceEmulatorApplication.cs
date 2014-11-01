namespace IGP.Tools.DeviceEmulator
{
    using System;
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class DeviceEmulatorApplication
    {
        private readonly ApplicationOptions _options;

        private readonly IDeviceFactory _deviceFactory;
        private readonly IPortFactory _portFactory;
        private readonly IEncoder _encoder;

        public DeviceEmulatorApplication(
            [NotNull] ApplicationOptions options,
            [NotNull] IDeviceFactory deviceFactory,
            [NotNull] IPortFactory portFactory,
            [NotNull] IEncoder encoder)
        {
            Contract.ArgumentIsNotNull(options, () => options);
            Contract.ArgumentIsNotNull(deviceFactory, () => deviceFactory);
            Contract.ArgumentIsNotNull(portFactory, () => portFactory);
            Contract.ArgumentIsNotNull(encoder, () => encoder);

            _options = options;
            _deviceFactory = deviceFactory;
            _portFactory = portFactory;
            _encoder = encoder;
        }

        public void Start()
        {
            var port = _portFactory.CreatePort(_options.Port, _options.PortParameters);
            port.Open();

            var device = _deviceFactory.CreateDevice(_options.DeviceType);

            device.Messages.Foreach(m => m.Subscribe(port.Transmit));
        }
    }
}