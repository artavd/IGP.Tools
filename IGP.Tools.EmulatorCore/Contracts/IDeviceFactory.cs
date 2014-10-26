namespace IGP.Tools.EmulatorCore.Contracts
{
    internal interface IDeviceFactory
    {
        IDevice Create(string filename);
    }
}