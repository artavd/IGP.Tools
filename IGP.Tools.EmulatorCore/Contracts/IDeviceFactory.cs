namespace IGP.Tools.EmulatorCore.Contracts
{
    public interface IDeviceFactory
    {
        IDevice Create(string deviceType);
    }
}