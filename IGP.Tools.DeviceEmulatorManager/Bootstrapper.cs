namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Windows;
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using IGP.Tools.EmulatorCore.Module;
    using IGP.Tools.IO.Module;
    using Microsoft.Practices.Prism.UnityExtensions;
    using Microsoft.Practices.Unity;
    using SBL.Common.Extensions;

    internal sealed class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = Shell.As<MainWindow>();
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // View models
            Container.RegisterType<IMainWindowViewModel, MainWindowViewModel>();
            Container.RegisterType<IRibbonViewModel, RibbonViewModel>();
            Container.RegisterType<IDeviceListViewModel, DeviceListViewModel>();
            Container.RegisterType<IPortConfiguratorViewModel, PortConfiguratorViewModel>();
            Container.RegisterType<IStatusBarViewModel, StatusBarViewModel>();

            // External extensions
            Container.AddExtension(new EmulatorCoreExtension("D:\\"));
            Container.AddExtension(new IOExtension());
        }
    }
}