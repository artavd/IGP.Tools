namespace IGP.Tools.EmulatorCore.Configuration
{
    internal class DeviceEmulatorConfigurationElement
    {
        public string DeviceName { get; set; }

        public bool IsTimeIncluded { get; set; }

        public MessageConfigurationElement[] Messages { get; set; }
    }
}