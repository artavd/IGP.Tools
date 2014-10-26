namespace IGP.Tools.EmulatorCore.Configuration
{
    internal class MessageConfigurationElement
    {
        public uint TimeInterval { get; set; }

        public string FormatString { get; set; }

        public ValueSetConfigurationElement[] ValuesSets { get; set; }
    }
}