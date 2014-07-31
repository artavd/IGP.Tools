namespace IGP.Tools.EmulatorCore.Configuration
{
    using System.IO;

    internal interface IDeviceFactory
    {
        IDevice Create(string filename);
    }
}