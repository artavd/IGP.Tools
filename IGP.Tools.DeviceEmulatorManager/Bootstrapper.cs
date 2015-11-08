namespace IGP.Tools.DeviceEmulatorManager
{
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

            IThemeService theme = Container.Resolve<IThemeService>();
            theme.Initialize(Application, DefaultTheme, DefaultAccents);

            Application.MainWindow = Shell.As<Shell>();
            Application.MainWindow.Show();
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
            Register<IRibbonService, RibbonService>();
            Register<IRibbonCommandsProvider, RibbonService>();
            Register<IThemeService, ThemeService>();

            Register<IEmulatorRepository, EmulatorRepository>();
            Register<IPortRepository, PortRepository>();
        }

        private void Register<TTo, TFrom>(bool isSingleton = true)
        {
            RegisterTypeIfMissing(typeof(TTo), typeof(TFrom), isSingleton);
        }

        private Application Application => Application.Current;
        private ResourceDictionary DefaultTheme => WellKnownThemes.DarkTheme;
        private ResourceDictionary DefaultAccents => WellKnownThemes.BlueAccents;
    }
}