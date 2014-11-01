namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Text;
    using CommandLine;
    using SBL.Common;
    using SBL.Common.Extensions;

    internal sealed class ApplicationOptions
    {
        [Option('p', "port", Required = true, HelpText = "Specifies port where messages will be transmitted.")]
        public string Port { get; set; }

        [HelpOption]
        public static string GetUsage()
        {
            var usage = new StringBuilder();

            usage.Append("Device Emulator Version 0.1");

            return usage.ToString();
        }

        public static ApplicationOptions Parse(string[] args)
        {
            Contract.ArgumentIsNotNull(args, () => args);

            var options = new ApplicationOptions();
            if (!Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(GetUsage());
                throw new ApplicationException("Application start failed: incorrect arguments!");
            }

            return options.As<ApplicationOptions>();
        }
    }
}