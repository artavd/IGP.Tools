namespace IGP.Tools.IO.Tests
{
    using System.IO;
    using IGP.Tools.IO.Implementation;
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
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            factory.CreatePort(portName);
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
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            factory.CreatePort("FILE", incorrectPath);
        }

        [Test]
        public void CreateFilePortShouldWorkCorrectlyForAbsolutePaths()
        {
            const string AbsolutePath = "D:\\absolute_file_name.txt";
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            var port = factory.CreatePort("FILE", AbsolutePath);

            Assert.IsInstanceOf<FilePort>(port);
            Assert.AreEqual(WellKnownPortTypes.FilePort, port.Type);
            Assert.AreEqual(false, port.IsOpened);

            Assert.AreEqual(AbsolutePath, port.As<FilePort>().OutputFilePath);
        }

        [TestCase("just_file_name.txt")]
        [TestCase("..\\..\\relative_file_name.txt")]
        public void CreateFilePortShouldWorkCorrectlyForRelativePaths(string relativePath)
        {
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            var port = factory.CreatePort("FILE", relativePath);

            Assert.IsInstanceOf<FilePort>(port);
            Assert.AreEqual(WellKnownPortTypes.FilePort, port.Type);
            Assert.AreEqual(false, port.IsOpened);

            string expectedPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath));
            Assert.AreEqual(expectedPath, port.As<FilePort>().OutputFilePath);
        }

        [TestCase("FILE")]
        [TestCase("file")]
        [TestCase("File")]
        [TestCase("fIlE")]
        public void CreateFilePortShouldBeCaseInsensitive(string portType)
        {
            const string FilePath = "D:\\absolute_file_name.txt";
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            var port = factory.CreatePort(portType, FilePath);

            Assert.IsInstanceOf<FilePort>(port);
            Assert.AreEqual(WellKnownPortTypes.FilePort, port.Type);
            Assert.AreEqual(false, port.IsOpened);
            Assert.AreEqual(FilePath, port.As<FilePort>().OutputFilePath);
        }

        [TestCase("console")]
        [TestCase("Console")]
        [TestCase("CONSOLE")]
        [TestCase("cOnSoLe")]
        public void CreateConsolePortShouldBeCaseInsensitive(string portType)
        {
            IPortFactory factory = new WellKnownPortTypesPortFactory();

            var port = factory.CreatePort(portType);

            Assert.IsInstanceOf<ConsolePort>(port);
            Assert.AreEqual(WellKnownPortTypes.ConsolePort, port.Type);
            Assert.AreEqual(false, port.IsOpened);
        }
    }
}