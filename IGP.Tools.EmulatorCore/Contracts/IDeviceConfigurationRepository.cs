namespace IGP.Tools.EmulatorCore
{
    using System.Collections.Generic;
    using System.IO;
    using SBL.Common.Annotations;

    public interface IDeviceConfigurationRepository
    {
        [NotNull]
        IEnumerable<string> KnownDeviceTypes { get; }

        [NotNull]
        Stream GetDeviceConfigurationStream([NotNull] string deviceType);
    }
}