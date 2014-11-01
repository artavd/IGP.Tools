namespace IGP.Tools.EmulatorCore.Module
{
    using IGP.Tools.EmulatorCore.Configuration;
    using IGP.Tools.EmulatorCore.Contracts;
    using Microsoft.Practices.Unity;
    using SBL.Common;

    public sealed class EmulatorCoreExtension : UnityContainerExtension
    {
        private readonly string _configRepoPath;

        public EmulatorCoreExtension(string configRepoPath)
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