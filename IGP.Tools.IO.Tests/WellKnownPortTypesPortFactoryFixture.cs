namespace IGP.Tools.IO.Tests
{
    using System.IO;

    using IGP.Tools.IO.Implementation;

    using Moq;

    using NUnit.Framework;

    using SBL.Common;
    using SBL.Common.Extensions;

    [TestFixture]
    internal sealed class WellKnownPortTypesPortFactoryFixture
    {
        [TestCase("FIEL")]
        [TestCase("smb4500")]
        [TestCase("TPCIN231")]
        [ExpectedException(typeof(FactoryException))]
        public void CreatePortShouldThrowExceptionForUnknownPortTypes(string portName)
        {
            // Given
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            // When
            factory.CreatePort(portName);

            // Then
            // Exception
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("file|name")]
        [TestCase("file\"name")]
        [TestCase("file\0name")]
        [ExpectedException(typeof(FactoryException))]
        public void CreateFilePortWithIncorrectParametersShouldThrowException(string incorrectPath)
        {
            // Given
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            // When
            factory.CreatePort("FILE", incorrectPath);

            // Then
            // Exception
        }

        [Test]
        public void CreateFilePortShouldWorkCorrectlyForAbsolutePaths()
        {
            // Given
            const string AbsolutePath = "D:\\absolute_file_name.txt";
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            // When
            var port = factory.CreatePort("FILE", AbsolutePath);

            // Then
            Assert.IsInstanceOf<FilePort>(port);
            Assert.AreEqual(WellKnownPortTypes.FilePort, port.Type);
            Assert.AreEqual(PortStates.Disconnected, port.CurrentState);

            Assert.AreEqual(AbsolutePath, port.As<FilePort>().OutputFilePath);
        }

        [TestCase("just_file_name.txt")]
        [TestCase("..\\..\\relative_file_name.txt")]
        public void CreateFilePortShouldWorkCorrectlyForRelativePaths(string relativePath)
        {
            // Given
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            // When
            var port = factory.CreatePort("FILE", relativePath);

            // Then
            Assert.IsInstanceOf<FilePort>(port);
            Assert.AreEqual(WellKnownPortTypes.FilePort, port.Type);
            Assert.AreEqual(PortStates.Disconnected, port.CurrentState);

            string expectedPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath));
            Assert.AreEqual(expectedPath, port.As<FilePort>().OutputFilePath);
        }

        [TestCase("FILE")]
        [TestCase("file")]
        [TestCase("File")]
        [TestCase("fIlE")]
        public void CreateFilePortShouldBeCaseInsensitive(string portType)
        {
            // Given
            const string FilePath = "D:\\absolute_file_name.txt";
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            // When
            var port = factory.CreatePort(portType, FilePath);

            // Then
            Assert.IsInstanceOf<FilePort>(port);
            Assert.AreEqual(WellKnownPortTypes.FilePort, port.Type);
            Assert.AreEqual(PortStates.Disconnected, port.CurrentState);
            Assert.AreEqual(FilePath, port.As<FilePort>().OutputFilePath);
        }

        [TestCase("console")]
        [TestCase("Console")]
        [TestCase("CONSOLE")]
        [TestCase("cOnSoLe")]
        public void CreateConsolePortShouldBeCaseInsensitive(string portType)
        {
            // Given
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            // When
            var port = factory.CreatePort(portType);

            // Then
            Assert.IsInstanceOf<ConsolePort>(port);
            Assert.AreEqual(WellKnownPortTypes.ConsolePort, port.Type);
            Assert.AreEqual(PortStates.Disconnected, port.CurrentState);
        }

        // TODO: AA: Add tests for TCP port creators

        [TestCase("FILE")]
        [TestCase("FiLe")]
        [TestCase("CONSOLE")]
        [TestCase("CoNSoLE")]
        [TestCase("TCP")]
        [TestCase("tCP")]
        [ExpectedException(typeof(ContractException))]
        public void RegisterWellKnownPortTypeShouldThrowExceptionIfTypeAlreadyRegistered(string portType)
        {
            // Given
            var factory = new WellKnownPortTypesPortFactory();

            // When
            factory.RegisterWellKnownPortType(portType, Mock.Of<IPortCreator>());

            // Then
            // Exception
        }

        [Test]
        public void RegisteredPortTypeShouldBeUsedForCreation()
        {
            // Given
            const string portName = "abrakadabra";
            const string parameters = "chigrikimigriki";

            var port = Mock.Of<IPort>();
            var creatorMock = new Mock<IPortCreator>();
            creatorMock.Setup(p => p.CanBeCreatedFrom(portName)).Returns(true);
            creatorMock.Setup(p => p.CreatePort(portName, parameters)).Returns(port);

            var factory = new WellKnownPortTypesPortFactory();
            factory.RegisterWellKnownPortType("custom port type", creatorMock.Object);

            // When
            var createdPort = factory.CreatePort(portName, parameters);

            // Then
            creatorMock.Verify(p => p.CanBeCreatedFrom(portName), Times.Once);
            creatorMock.Verify(p => p.CreatePort(portName, parameters), Times.Once);

            Assert.AreEqual(port, createdPort);
        }
    }
}