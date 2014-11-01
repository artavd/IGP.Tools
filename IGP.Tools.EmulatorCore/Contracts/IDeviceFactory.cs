namespace IGP.Tools.EmulatorCore.Contracts
{
    using SBL.Common.Annotations;

    public interface IDeviceFactory
    {
        [NotNull]
        IDevice Create([NotNull] string deviceType);
    }
}