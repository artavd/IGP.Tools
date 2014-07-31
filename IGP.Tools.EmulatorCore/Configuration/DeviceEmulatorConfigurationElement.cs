namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.Xml.Serialization;

    [XmlRoot("DeviceEmulator")]
    internal class DeviceEmulatorConfigurationElement
    {
        [XmlAttribute]
        public string DeviceName { get; set; }

        [XmlAttribute]
        public bool IsTimeIncluded { get; set; }

        [XmlElement("Message")]
        public MessageConfigurationElement[] Messages { get; set; }
    }
}