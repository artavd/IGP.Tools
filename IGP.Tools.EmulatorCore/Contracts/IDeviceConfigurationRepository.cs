namespace IGP.Tools.EmulatorCore.Contracts
{
    using System.Collections.Generic;
    using System.IO;

    public interface IDeviceConfigurationRepository
    {
        IEnumerable<string> KnownDeviceTypes { get; }

        Stream GetDeviceConfigurationStream(string deviceType);
    }
}