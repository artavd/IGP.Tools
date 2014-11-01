namespace IGP.Tools.DeviceEmulator
{
    using System;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            var options = ApplicationOptions.Parse(args);
            var application = new DeviceEmulatorApplication();

            application.Start(options);

            Console.ReadLine();
        }
    }
}
