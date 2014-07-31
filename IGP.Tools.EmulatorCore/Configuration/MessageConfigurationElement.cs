namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.Xml.Serialization;

    internal class MessageConfigurationElement
    {
        [XmlAttribute]
        public uint TimeInterval { get; set; }

        [XmlAttribute]
        public string FormatString { get; set; }

        [XmlElement("ValueSet")]
        public ValueSetConfigurationElement[] ValuesSets { get; set; }
    }
}