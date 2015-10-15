namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Reflection.Emit;
    using System.Windows;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Unity;
    using SBL.Common.Extensions;

    internal sealed class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = Shell.As<Shell>();
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog.As<ModuleCatalog>().AddModule(typeof(PrimaryModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Register<IStatusMessageService, StatusMessageService>();

            Register<IEmulatorRepository, EmulatorRepository>();
            Register<IPortRepository, PortRepository>();
        }

        private void Register<TTo, TFrom>(bool isSingleton = true)
        {
            RegisterTypeIfMissing(typeof(TTo), typeof(TFrom), isSingleton);
        }
    }
}