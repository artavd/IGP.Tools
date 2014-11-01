namespace IGP.Tools.EmulatorCore
{
    using SBL.Common.Annotations;

    public interface IDeviceFactory
    {
        [NotNull]
        IDevice CreateDevice([NotNull] string deviceType);
    }
}