namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using IGP.Tools.EmulatorCore.Contracts;
    using SBL.Common;

    internal sealed class FileSystemDeviceConfigurationRepository : IDeviceConfigurationRepository
    {
        private const string DeviceFileFormat = @".xml";

        private readonly string _repositoryPath;

        public FileSystemDeviceConfigurationRepository(string repositoryPath)
        {
            Contract.ArgumentIsNotNull(repositoryPath, () => repositoryPath);

            _repositoryPath = repositoryPath;
        }

        public IEnumerable<string> KnownDeviceTypes
        {
            get
            {
                return Directory
                    .GetFiles(
                        _repositoryPath,
                        string.Format("*{0}", DeviceFileFormat),
                        SearchOption.TopDirectoryOnly)
                    .Select(x => x.Replace(DeviceFileFormat, string.Empty));
            }
        }

        public Stream GetDeviceConfigurationStream(string deviceType)
        {
            Contract.ArgumentIsNotNull(deviceType, () => deviceType);
            Contract.ArgumentSatisfied(deviceType, () => deviceType, KnownDeviceTypes.Contains);

            return File.OpenRead(string.Format("{0}{1}", deviceType, DeviceFileFormat));
        }
    }
}