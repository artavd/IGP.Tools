namespace IGP.Tools.DeviceEmulator
{
    using SBL.Common;

    internal sealed class DeviceEmulatorApplication
    {
        private readonly ApplicationOptions _options;

        public DeviceEmulatorApplication(ApplicationOptions options)
        {
            Contract.ArgumentIsNotNull(options, () => options);

            _options = options;
        }

        public void Start()
        {
        }
    }
}