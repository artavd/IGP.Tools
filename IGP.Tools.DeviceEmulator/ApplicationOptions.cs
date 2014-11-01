namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Linq;
    using CommandLine;
    using CommandLine.Text;
    using SBL.Common;
    using SBL.Common.Extensions;

    internal sealed class ApplicationOptions
    {
        public bool HasError
        {
            get { return LastParserState.Eval(x => x.Errors.Any(), () => false); }
        }

        public bool Help { get; private set; }

        [Option('p', "port", Required = true, HelpText = "Output port (i.e.: 'COM1', 'TCP4000', 'TCPIN3021').")]
        public string Port { get; set; }

        [Option('d', "device", Required = true, HelpText = "Device emulator type which will be looked up from repository.")]
        public string DeviceType { get; set; }

        [Option("repository", DefaultValue = "devices", HelpText = "Path to device emulator types repository.")]
        public string DeviceRepository { get; set; }

        [Option("com", MutuallyExclusiveSet = "serial", HelpText = "If output port is Serial Port (COM) then this option contains it's parameters (i.e. '9600-8-N-1', '1200-7-E-1')")]
        public string Address { get; set; }

        [Option("address", MutuallyExclusiveSet = "network", HelpText = "If output port is TCP Port then this option contains destination IP address (i.e. '127.0.0.1', '192.168.12.14')")]
        public string ComParameters { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("Device Emulator", "Version 0.1"),
                Copyright = new CopyrightInfo("Artem Avdosev", 2014),
                AddDashesToOption = true
            };

            if (HasError)
            {
                string errors = help.RenderParsingErrorsText(this, 2);

                if (!string.IsNullOrWhiteSpace(errors))
                {
                    help.AddPreOptionsLine(string.Concat(Environment.NewLine, "Argument parsing error(s):"));
                    help.AddPreOptionsLine(errors);

                    return help;
                }
            }

            help.AddPreOptionsLine(string.Empty.PadRight(70, '-'));
            help.AddPreOptionsLine("Usage: IGP.Tools.DeviceEmulator [OPTIONS] -p [PORT] -d [DEVICE_TYPE]");
            
            help.AddOptions(this);
            
            help.AddPostOptionsLine("Example: IGP.Tools.DeviceEmulator -p COM10 -d CL31");
            help.AddPostOptionsLine(Environment.NewLine);

            Help = true;

            return help;
        }

        public static ApplicationOptions Parse(string[] args)
        {
            Contract.ArgumentIsNotNull(args, () => args);

            var options = new ApplicationOptions();
            Parser.Default.ParseArguments(args, options);

            return options.As<ApplicationOptions>();
        }
    }
}