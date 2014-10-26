namespace IGP.Tools.EmulatorCore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using IGP.Tools.EmulatorCore.Configuration;
    using IGP.Tools.EmulatorCore.Contracts;
    using NUnit.Framework;
    using SBL.Common.Extensions;

    [TestFixture]
    internal class ConfigurationDeviceEmulatorFactoryFixture
    {
        private MockDeviceConfigurationRepository _mockRepository = new MockDeviceConfigurationRepository();

        private const string DeviceWithoutMessagesType = "PTB220";
        private const string DeviceWithoutMessagesConfig = @"<DeviceEmulator DeviceName=""PTB220"" />";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _mockRepository.AddDeviceConfigurationEntry(DeviceWithoutMessagesType, DeviceWithoutMessagesConfig);
        }

        [Test]
        public void CreatingOfEmptyDeviceShouldBeCorrect()
        {
            var factory = new ConfigurationDeviceEmulatorFactory(_mockRepository);

            var device = factory.Create(DeviceWithoutMessagesType);
        }

        private sealed class MockDeviceConfigurationRepository :
            IDeviceConfigurationRepository
        {
            private readonly IDictionary<string, string> _configTable;

            public MockDeviceConfigurationRepository()
            {
                _configTable = new Dictionary<string, string>();
            }

            public void AddDeviceConfigurationEntry(string deviceType, string config)
            {
                _configTable.Add(deviceType, config);
            }

            public IEnumerable<string> KnownDeviceTypes
            {
                get { return _configTable.Keys; }
            }

            public Stream GetDeviceConfigurationStream(string deviceType)
            {
                if (!_configTable.ContainsKey(deviceType))
                {
                    throw new ArgumentOutOfRangeException("deviceType",
                        "Unknown device type");
                }

                return _configTable[deviceType].ToStream();
            }
        }
    }
}