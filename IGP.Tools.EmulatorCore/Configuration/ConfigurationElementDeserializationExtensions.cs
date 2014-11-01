namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.Linq;
    using System.Xml.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal static class ConfigurationElementDeserializationExtensions
    {
        private const string NameAttribute = "Name";

        private const string DeviceEmulatorRootName = "DeviceEmulator";
        private const string DeviceNameAttribute = "DeviceName";
        private const string IsTimeIncludedAttribute = "IsTimeIncluded";

        private const string MessageRootName = "Message";
        private const string TimeIntervalAttribute = "TimeInterval";
        private const string FormatStringAttribute = "FormatString";

        private const string ValueSetRootName = "ValueSet";
        private const string ValueSetValueName = "Value";

        [NotNull]
        public static DeviceEmulatorConfigurationElement DeserializeDeviceEmulator(
            [NotNull] this XElement element)
        {
            Contract.ArgumentIsNotNull(element, () => element);
            Contract.IsTrue(
                element.Name == DeviceEmulatorRootName,
                () => string.Format("Wrong XML element for device emulator: {0}", element));

            var result = new DeviceEmulatorConfigurationElement();
            
            result.DeviceName = element.Attribute(DeviceNameAttribute).Value;
            
            result.IsTimeIncluded = bool.Parse(element.Attribute(IsTimeIncludedAttribute).Eval(
                x => x.Value,
                () => "false"));
            
            result.Messages = element
                .Descendants(MessageRootName)
                .Select(x => x.DeserializeMessage())
                .ToArray();

            return result;
        }

        [NotNull]
        public static MessageConfigurationElement DeserializeMessage([NotNull] this XElement element)
        {
            Contract.ArgumentIsNotNull(element, () => element);
            Contract.IsTrue(
                element.Name == MessageRootName,
                () => string.Format("Wrong XML element for message: {0}.", element));

            var result = new MessageConfigurationElement();
            result.FormatString = element.Attribute(FormatStringAttribute).Value;
            result.TimeInterval = uint.Parse(element.Attribute(TimeIntervalAttribute).Value);
            result.ValuesSets = element
                .Descendants(ValueSetRootName)
                .Select(x => x.DeserializeValueSet())
                .ToArray();

            return result;
        }

        [NotNull]
        public static ValueSetConfigurationElement DeserializeValueSet([NotNull] this XElement element)
        {
            Contract.ArgumentIsNotNull(element, () => element);
            Contract.IsTrue(
                element.Name == ValueSetRootName,
                () => string.Format("Wrong XML element for value set: {0}.", element));

            var result = new ValueSetConfigurationElement();
            result.Name = element.Attribute(NameAttribute).Value;

            var values = element.Descendants(ValueSetValueName);
            Contract.IsTrue(values.Any(), () => "Value set must contain at least one value.");
            result.Values = values.Select(x => x.Value).ToArray();

            return result;
        }
    }
}