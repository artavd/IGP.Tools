namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;
    using IGP.Tools.EmulatorCore.Module;
    using IGP.Tools.IO.Implementation;
    using IGP.Tools.IO.Module;
    using Microsoft.Practices.Unity;

    internal static class Program
    {
        public static readonly object ExitLock = new object();

        private static void Main(string[] args)
        {
            var port = new TcpClientPort(new IPEndPoint(IPAddress.Loopback, 4001));

            port.StateFeed.Subscribe(x => Console.WriteLine("state changed: {0}", x));
            port.ReceivedFeed.Subscribe(x => Console.WriteLine("received: {0}", x));

            Console.WriteLine("Enter to transmit...");
            bool work = true;
            while (work)
            {
                var text = Console.ReadLine();
                switch(text)
                {
                    case "quit":
                        work = false;
                        break;

                    case "open":
                        port.Connect();
                        break;

                    case "close":
                        port.Disconnect();
                        break;

                    default:
                        port.Transmit(Encoding.ASCII.GetBytes(text));
                        break;
                }
            }

            return;
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
                Environment.NewLine, e.ExceptionObject.ToString()), 1);
        }
    }
}