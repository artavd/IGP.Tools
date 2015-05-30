namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Threading;

    using IGP.Tools.EmulatorCore.Module;
    using IGP.Tools.IO.Module;

    using Microsoft.Practices.Unity;

    using SBL.Common.Extensions;

    internal static class Program
    {
        public static readonly object ExitLock = new object();

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnException;

            var options = ApplicationOptions.Parse(args);
            if (options.HasError || options.Help)
            {
                Exit(null, 1);
            }

            var container = InitializeContainer(options);
            var application = container.Resolve<DeviceEmulatorApplication>();

            application.Start();

            WaitForExitSignal();
            Exit();
        }

        private static void WaitForExitSignal()
        {
            lock (ExitLock)
            {
                Monitor.Wait(ExitLock);
            }
        }

        private static void Exit(string message = null, int exitCode = 0)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Environment.Exit(exitCode);
        }

        private static IUnityContainer InitializeContainer(ApplicationOptions options)
        {
            var container = new UnityContainer();

            container.AddExtension(new EmulatorCoreExtension(options.DeviceRepository));
            container.AddExtension(new IOExtension());

            container.RegisterInstance(options);

            return container;
        }

        private static void OnException(object sender, UnhandledExceptionEventArgs e)
        {
            Exit(string.Format(
                "Application error occured:{0}{1}",
                Environment.NewLine, e.ExceptionObject.As<Exception>().Message), 1);
        }
    }
}