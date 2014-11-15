namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    internal interface IMainWindowViewModel
    {
        IRibbonViewModel Ribbon { get; }

        IDeviceListViewModel DeviceList { get; }

        IPortConfiguratorViewModel PortConfigurator { get; }

        IStatusBarViewModel StatusBar { get; }
    }
}