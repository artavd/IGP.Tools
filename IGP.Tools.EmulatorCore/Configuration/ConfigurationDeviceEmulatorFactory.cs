﻿namespace IGP.Tools.EmulatorCore.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using IGP.Tools.EmulatorCore.Implementation;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class ConfigurationDeviceEmulatorFactory : IDeviceFactory
    {
        private readonly IDeviceConfigurationRepository _repository;
        private readonly IEncoder _encoder;

        public ConfigurationDeviceEmulatorFactory(
            [NotNull] IDeviceConfigurationRepository repository,
            [NotNull] IEncoder encoder)
        {
            Contract.ArgumentIsNotNull(repository, () => repository);
            Contract.ArgumentIsNotNull(encoder, () => encoder);

            _repository = repository;
            _encoder = encoder;
        }

        public IDevice CreateDevice(string deviceType)
        {
            Contract.ArgumentIsNotNull(deviceType, () => deviceType);

            var configElement = LoadConfiguration(deviceType);

            var messageProviders = new List<IMessageProvider>();
            foreach (var m in configElement.Messages)
            {
                var provider = new MessageProvider(m.FormatString)
                {
                    Interval = TimeSpan.FromMilliseconds(m.TimeInterval)
                };

                if (provider.Values.Length != m.ValuesSets.Length)
                {
                    throw new FactoryException(
                        typeof (ConfigurationDeviceEmulatorFactory),
                        typeof (DeviceEmulator),
                        "Configuration file format exception: wrong number of value sets",
                        deviceType);
                }

                m.ValuesSets
                    .Select((v, i) => new { Provider = v, Index = i })
                    .Foreach(x =>
                    {
                        var value = new CyclicValueProvider(x.Provider.Name);
                        value.AddValueRange(x.Provider.Values);
                        provider.Values[x.Index] = value;
                    });

                messageProviders.Add(provider);
            }

            var emulator = new DeviceEmulator(configElement.DeviceName, messageProviders, _encoder)
            {
                IsTimeIncluded = configElement.IsTimeIncluded
            };

            return emulator;
        }

        private DeviceEmulatorConfigurationElement LoadConfiguration(string deviceType)
        {
            try
            {
                using (var configStream = _repository.GetDeviceConfigurationStream(deviceType))
                {
                    var xmlElement = LoadXElementWithInvalidCharacters(configStream);
                    return xmlElement.DeserializeDeviceEmulator();
                }
            }
            catch (Exception ex)
            {
                throw new FactoryException(
                    typeof (ConfigurationDeviceEmulatorFactory),
                    typeof (DeviceEmulator),
                    $"Unable to create device of {deviceType} type.",
                    deviceType,
                    ex);
            }
        }

        private static XElement LoadXElementWithInvalidCharacters(Stream stream)
        {
            var xmlReaderSettings = new XmlReaderSettings { CheckCharacters = false };
            using (var xmlReader = XmlReader.Create(stream, xmlReaderSettings))
            {
                xmlReader.MoveToContent();
                return XElement.Load(xmlReader);
            }
        }
    }
}