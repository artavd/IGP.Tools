namespace IGP.Tools.EmulatorCore.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using IGP.Tools.EmulatorCore.Configuration;
    using NUnit.Framework;
    using SBL.Common.Extensions;

    [TestFixture]
    internal sealed class ConfigurationDeviceEmulatorFactoryFixture
    {
        private MockDeviceConfigurationRepository _mockRepository = new MockDeviceConfigurationRepository();

        private const string EmptyDevice = "Empty";
        private const string EmptyDeviceConfig = @"<DeviceEmulator DeviceName=""Empty"" />";

        private const string DeviceWithoutName = "Without name";
        private const string DeviceWithoutNameConfig = @"<DeviceEmulator />";

        private const string FilledDeviceWithoutName = "Filled without name";
        private const string FilledDeviceWithoutNameConfig = @"<DeviceEmulator IsTimeIncluded=""False"">
  <Message TimeInterval=""1000"" FormatString=""{0}"">
    <ValueSet Name=""Pressure"">
      <Value>976.5</Value>
      <Value>976.5</Value>
    </ValueSet>
  </Message>
</DeviceEmulator>";

        private const string DeviceWithIncorrectValueSetNumber = "with incorrect value sets number";
        private const string DeviceWithIncorrectValueSetNumberConfig =
            @"<DeviceEmulator DeviceName=""PTB220"" IsTimeIncluded=""False"">
  <Message TimeInterval=""200"" FormatString=""{0}{1}"">
    <ValueSet Name=""Pressure"">
      <Value>976.5</Value>
      <Value>976.5</Value>
    </ValueSet>
  </Message>
</DeviceEmulator>";

        private const string DeviceWithMessageWithoutFormatString = "with message without format string";
        private const string DeviceWithMessageWithoutFormatStringConfig =
            @"<DeviceEmulator DeviceName=""PTB220"" IsTimeIncluded=""False"">
  <Message TimeInterval=""200"">
    <ValueSet Name=""Pressure"">
      <Value>976.5</Value>
      <Value>976.5</Value>
    </ValueSet>
  </Message>
</DeviceEmulator>";

        private const string DeviceWithMessageWithoutTimeInterval = "with message without time interval";
        private const string DeviceWithMessageWithoutTimeIntervalConfig =
            @"<DeviceEmulator DeviceName=""PTB220"" IsTimeIncluded=""False"">
  <Message FormatString=""{0}"">
    <ValueSet Name=""Pressure"">
      <Value>976.5</Value>
      <Value>976.5</Value>
    </ValueSet>
  </Message>
</DeviceEmulator>";

        private const string GoodDevice = "Good";
        private const string GoodDeviceConfig =
            @"<DeviceEmulator DeviceName=""Good"" IsTimeIncluded=""True"">
  <Message TimeInterval=""200"" FormatString=""{0}{1}"">
    <ValueSet Name=""Pressure"">
      <Value>976.5</Value>
      <Value>976.5</Value>
    </ValueSet>
    <ValueSet Name=""Temperature"">
      <Value>21.5</Value>
      <Value>21.6</Value>
    </ValueSet>
  </Message>
  <Message TimeInterval=""1000"" FormatString=""status ok""/>
</DeviceEmulator>";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // Configs with errors
            _mockRepository.AddDeviceConfigurationEntry(DeviceWithoutName, DeviceWithoutNameConfig);
            _mockRepository.AddDeviceConfigurationEntry(FilledDeviceWithoutName, FilledDeviceWithoutNameConfig);
            _mockRepository.AddDeviceConfigurationEntry(DeviceWithIncorrectValueSetNumber, DeviceWithIncorrectValueSetNumberConfig);
            _mockRepository.AddDeviceConfigurationEntry(DeviceWithMessageWithoutFormatString, DeviceWithMessageWithoutFormatStringConfig);
            _mockRepository.AddDeviceConfigurationEntry(DeviceWithMessageWithoutTimeInterval, DeviceWithMessageWithoutTimeIntervalConfig);

            // Normal configs
            _mockRepository.AddDeviceConfigurationEntry(EmptyDevice, EmptyDeviceConfig);
            _mockRepository.AddDeviceConfigurationEntry(GoodDevice, GoodDeviceConfig);
        }

        [TestCase(DeviceWithoutName)]
        [TestCase(FilledDeviceWithoutName)]
        [ExpectedException(typeof(FormatException))]
        public void CreatingOfDeviceWithoutNameShouldThrowException(string deviceType)
        {
            var factory = new ConfigurationDeviceEmulatorFactory(_mockRepository);

            factory.CreateDevice(deviceType);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void CreatingOfDeviceWithValueSetsNumberInappropriateToFormatStringShouldThrowException()
        {
            var factory = new ConfigurationDeviceEmulatorFactory(_mockRepository);

            factory.CreateDevice(DeviceWithIncorrectValueSetNumber);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void CreatingOfDeviceWithMessageWithoutFormatStringOrTimeIntervalShouldThrowException()
        {
            var factory = new ConfigurationDeviceEmulatorFactory(_mockRepository);

            factory.CreateDevice(DeviceWithIncorrectValueSetNumber);
        }

        [Test]
        public void CreatingOfEmptyDeviceShouldBeCorrect()
        {
            var factory = new ConfigurationDeviceEmulatorFactory(_mockRepository);

            var device = factory.CreateDevice(EmptyDevice);

            Assert.AreEqual(EmptyDevice, device.Name);
            Assert.IsEmpty(device.Messages);
        }

        [Test]
        public void CreatingOfNormallyFilledMessageShouldBeCorrect()
        {
            var factory = new ConfigurationDeviceEmulatorFactory(_mockRepository);

            var device = factory.CreateDevice(GoodDevice);

            Assert.AreEqual(GoodDevice, device.Name);
            Assert.AreEqual(2, device.Messages.Count);

            // TODO: check other information
            // (blocked by #13 Implement device info class and appropriate property in IDevice
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