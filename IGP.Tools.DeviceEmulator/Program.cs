namespace IGP.Tools.DeviceEmulator
{
    using System;
    using IGP.Tools.EmulatorCore.Module;
    using Microsoft.Practices.Unity;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var options = ApplicationOptions.Parse(args);
            if (options.HasError || options.Help)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
                return;
            }

            var container = InitializeContainer(options);
            var application = container.Resolve<DeviceEmulatorApplication>();

            application.Start();

            Console.WriteLine("Device Emulator work finished. Press any key to exit.");
            Console.ReadKey();
        }

        private static IUnityContainer InitializeContainer(ApplicationOptions options)
        {
            var container = new UnityContainer();

            container.AddExtension(new EmulatorCoreExtension(options.DeviceRepository));

            container.RegisterInstance(options);

            return container;
        }
    }
}
