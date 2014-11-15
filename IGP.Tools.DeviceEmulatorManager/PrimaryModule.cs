namespace IGP.Tools.DeviceEmulatorManager
{
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using IGP.Tools.DeviceEmulatorManager.Views;
    using IGP.Tools.EmulatorCore.Module;
    using IGP.Tools.IO.Module;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class PrimaryModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public PrimaryModule(
            [NotNull] IUnityContainer container,
            [NotNull] IRegionManager regionManager)
        {
            Contract.ArgumentIsNotNull(container, () => container);
            Contract.ArgumentIsNotNull(regionManager, () => regionManager);

            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            // View models
            _container.RegisterType<IRibbonViewModel, RibbonViewModel>();
            _container.RegisterType<IDeviceListViewModel, DeviceListViewModel>();
            _container.RegisterType<IPortConfiguratorViewModel, PortConfiguratorViewModel>();
            _container.RegisterType<IStatusBarViewModel, StatusBarViewModel>();

            // External extensions
            _container.AddExtension(new EmulatorCoreExtension("D:\\"));
            _container.AddExtension(new IOExtension());

            // Views
            _regionManager.RegisterViewWithRegion(WellKnownRegions.Ribbon, typeof(RibbonView));
            _regionManager.RegisterViewWithRegion(WellKnownRegions.DeviceList, typeof(DeviceListView));
            _regionManager.RegisterViewWithRegion(WellKnownRegions.PortConfigurator, typeof(PortConfiguratorView));
            _regionManager.RegisterViewWithRegion(WellKnownRegions.StatusBar, typeof(StatusBarView));
        }
    }
}