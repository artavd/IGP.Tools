namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using SBL.Common.Annotations;

    internal interface IStatusBarViewModel
    {
        string StatusMessage { [NotNull] get; }
    }
}