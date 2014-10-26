namespace IGP.Tools.EmulatorCore.Contracts
{
    using System.IO;

    internal interface IDeviceFactory
    {
        IDevice Create(string deviceType);
    }
}