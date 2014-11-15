namespace IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation
{
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class MainWindowViewModel : IMainWindowViewModel
    {
        public MainWindowViewModel([NotNull] IPortFactory portFactory, [NotNull] IDeviceFactory deviceFactory)
        {
            Contract.ArgumentIsNotNull(portFactory, () => portFactory);
            Contract.ArgumentIsNotNull(deviceFactory, () => deviceFactory);
        }
    }
}