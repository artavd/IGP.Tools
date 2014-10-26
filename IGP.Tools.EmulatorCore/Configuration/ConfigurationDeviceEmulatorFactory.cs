namespace IGP.Tools.EmulatorCore.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using IGP.Tools.EmulatorCore.Contracts;
    using IGP.Tools.EmulatorCore.Implementation;
    using Microsoft.Practices.ObjectBuilder2;

    internal class ConfigurationDeviceEmulatorFactory : IDeviceFactory
    {
        public IDevice Create(string filename)
        {
            var file = new FileStream(filename, FileMode.Open);
            return Create(new StreamReader(file));
        }

        public IDevice Create(TextReader text)
        {
            var configElement = LoadConfiguration(text);

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

        private DeviceEmulatorConfigurationElement LoadConfiguration(TextReader text)
        {
            var serializer = new XmlSerializer(typeof(DeviceEmulatorConfigurationElement));
            return (DeviceEmulatorConfigurationElement)serializer.Deserialize(text);
        }
    }
}