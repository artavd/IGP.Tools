﻿namespace IGP.Tools.EmulatorCore.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using IGP.Tools.EmulatorCore.Contracts;
    using IGP.Tools.EmulatorCore.Implementation;
    using Microsoft.Practices.ObjectBuilder2;

    internal sealed class ConfigurationDeviceEmulatorFactory : IDeviceFactory
    {
        private readonly IDeviceConfigurationRepository _repository;

        public ConfigurationDeviceEmulatorFactory(IDeviceConfigurationRepository repository)
        {
            _repository = repository;
        }

        public IDevice Create(string deviceType)
        {
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
                    throw new FormatException("Configuration file format exception: wrong number of value sets");
                }

                m.ValuesSets
                 .Select((v, i) => new { Provider = v, Index = i })
                 .ForEach(x =>
                          {
                              var value = new CyclicValueProvider(x.Provider.Name);
                              value.AddValueRange(x.Provider.Values);
                              provider.Values[x.Index] = value;
                          });
                
                messageProviders.Add(provider);
            }
            var emulator = new DeviceEmulator(configElement.DeviceName, messageProviders)
                           {
                               IsTimeIncluded = configElement.IsTimeIncluded
                           };

            return emulator;
        }

        private DeviceEmulatorConfigurationElement LoadConfiguration(string deviceType)
        {
            try
            {
                var configStream = _repository.GetDeviceConfigurationStream(deviceType);
                var xmlElement = XElement.Load(configStream);
                return xmlElement.DeserializeDeviceEmulator();
            }
            catch (Exception ex)
            {
                // TODO: move all strings to Resources
                // TODO: create own factory exception type
                throw new FormatException(string.Format("Unable to create device of {0} type", deviceType), ex);
            }
        }
    }
}