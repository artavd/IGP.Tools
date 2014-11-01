namespace IGP.Tools.EmulatorCore
{
    using SBL.Common.Annotations;

    public interface IDeviceFactory
    {
        [NotNull]
        IDevice Create([NotNull] string deviceType);
    }
}