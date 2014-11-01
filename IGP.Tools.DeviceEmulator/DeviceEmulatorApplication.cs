namespace IGP.Tools.DeviceEmulator
{
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    internal sealed class DeviceEmulatorApplication
    {
        public void Start(ApplicationOptions options)
        {
            InitializeContainer();
        }

        private void InitializeContainer()
        {
            var container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            // TODO: add unity extension in EmulatorCore and IO projects and add it here
        }
    }
}