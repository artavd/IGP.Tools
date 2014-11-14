namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Windows;
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation;
    using IGP.Tools.DeviceEmulatorManager.Views;
    using IGP.Tools.EmulatorCore.Module;
    using IGP.Tools.IO.Module;
    using Microsoft.Practices.Unity;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var container = InitializeContainer();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private static IUnityContainer InitializeContainer()
        {
            var container = new UnityContainer();

            // TODO: Ability to change repository path (extend IDeviceConfigurationRepository)
            container.AddExtension(new EmulatorCoreExtension("D:\\"));
            container.AddExtension(new IOExtension());

            container.RegisterType<IMainWindowViewModel, MainWindowViewModel>();

            return container;
        }
    }
}
