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
                () => $"Wrong XML element for device emulator: {element}");

            var result = new DeviceEmulatorConfigurationElement
            {
                DeviceName = element.Attribute(DeviceNameAttribute).Value,

                IsTimeIncluded = bool.Parse(element.Attribute(IsTimeIncludedAttribute).Eval(
                    x => x.Value,
                    () => "false")),

                Messages = element
                    .Descendants(MessageRootName)
                    .Select(x => x.DeserializeMessage())
                    .ToArray()
            };

            return result;
        }

        [NotNull]
        private static MessageConfigurationElement DeserializeMessage([NotNull] this XElement element)
        {
            Contract.ArgumentIsNotNull(element, () => element);
            Contract.IsTrue(
                element.Name == MessageRootName,
                () => $"Wrong XML element for message: {element}.");

            var result = new MessageConfigurationElement
            {
                FormatString = element.Attribute(FormatStringAttribute).Value,

                TimeInterval = uint.Parse(element.Attribute(TimeIntervalAttribute).Value),

                ValuesSets = element
                    .Descendants(ValueSetRootName)
                    .Select(x => x.DeserializeValueSet())
                    .ToArray()
            };

            return result;
        }

        [NotNull]
        private static ValueSetConfigurationElement DeserializeValueSet([NotNull] this XElement element)
        {
            Contract.ArgumentIsNotNull(element, () => element);
            Contract.IsTrue(
                element.Name == ValueSetRootName,
                () => $"Wrong XML element for value set: {element}.");

            var result = new ValueSetConfigurationElement { Name = element.Attribute(NameAttribute).Value };

            var values = element.Descendants(ValueSetValueName);
            Contract.IsTrue(values.Any(), () => "Value set must contain at least one value.");
            result.Values = values.Select(x => x.Value).ToArray();

            return result;
        }
    }
}