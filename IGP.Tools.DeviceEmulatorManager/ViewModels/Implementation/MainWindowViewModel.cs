namespace IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation
{
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class MainWindowViewModel : IMainWindowViewModel
    {
        public IRibbonViewModel Ribbon { get; private set; }

        public IDeviceListViewModel DeviceList { get; private set; }

        public IPortConfiguratorViewModel PortConfigurator { get; private set; }

        public IStatusBarViewModel StatusBar { get; private set; }

        public MainWindowViewModel(
            [NotNull] IRibbonViewModel ribbon,
            [NotNull] IDeviceListViewModel deviceList,
            [NotNull] IPortConfiguratorViewModel portConfigurator,
            [NotNull] IStatusBarViewModel statusBar)
        {
            Contract.ArgumentIsNotNull(ribbon, () => ribbon);
            Contract.ArgumentIsNotNull(deviceList, () => deviceList);
            Contract.ArgumentIsNotNull(portConfigurator, () => portConfigurator);
            Contract.ArgumentIsNotNull(statusBar, () => statusBar);

            Ribbon = ribbon;
            DeviceList = deviceList;
            PortConfigurator = portConfigurator;
            StatusBar = statusBar;
        }
    }
}