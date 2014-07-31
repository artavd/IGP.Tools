    namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.Xml.Serialization;

    internal class ValueSetConfigurationElement
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("Value")]
        public string[] Values { get; set; }
    }
}