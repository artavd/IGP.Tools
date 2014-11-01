namespace IGP.Tools.EmulatorCore.Module
{
    using IGP.Tools.EmulatorCore.Configuration;
    using Microsoft.Practices.Unity;
    using SBL.Common;
    using SBL.Common.Annotations;

    public sealed class EmulatorCoreExtension : UnityContainerExtension
    {
        private readonly string _configRepoPath;

        public EmulatorCoreExtension([NotNull] string configRepoPath)
        {
            Contract.ArgumentIsNotNull(configRepoPath, () => configRepoPath);

            _configRepoPath = configRepoPath;
        }

        protected override void Initialize()
        {
            Container.RegisterType<IDeviceConfigurationRepository, FileSystemDeviceConfigurationRepository>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(_configRepoPath));

            Container.RegisterType<IDeviceFactory, ConfigurationDeviceEmulatorFactory>(
                new ContainerControlledLifetimeManager());
        }
    }
}