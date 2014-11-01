namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class FileSystemDeviceConfigurationRepository : IDeviceConfigurationRepository
    {
        private const string DeviceFileFormat = @".xml";

        private readonly string _repositoryPath;

        public FileSystemDeviceConfigurationRepository([NotNull] string repositoryPath)
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
                    .Select(Path.GetFileNameWithoutExtension);
            }
        }

        public Stream GetDeviceConfigurationStream(string deviceType)
        {
            Contract.ArgumentIsNotNull(deviceType, () => deviceType);
            Contract.ArgumentSatisfied(deviceType, () => deviceType, KnownDeviceTypes.Contains);

            return File.OpenRead(Path.Combine(
                _repositoryPath, 
                string.Format("{0}{1}", deviceType, DeviceFileFormat)));
        }
    }
}