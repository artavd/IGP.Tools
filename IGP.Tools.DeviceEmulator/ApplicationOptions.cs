﻿namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Linq;
    using CommandLine;
    using CommandLine.Text;
    using SBL.Common;
    using SBL.Common.Extensions;

    internal sealed class ApplicationOptions
    {
        // TODO: #11 Move all UI strings to Resources
        private const string PortHelpText = "Output port (i.e. COM1, TCP4001, TCPIN3021).";
        private const string DeviceHelpText = "Device emulator type which will be looked up from repository.";
        private const string RepoHelpText = "Path to device emulator types repository.";
        private const string ComHelpText = "If output port is Serial Port (COM) then this option contains it's parameters (i.e. '9600-8-N-1', '1200-7-E-1')";
        private const string AddressHelpText = "If output port is TCP Port then this option contains destination IP address (i.e. '127.0.0.1', '192.168.12.14')";

        private const string SplitterString = "----------------------------------------------------------------------";
        private const string ErrorsHeaderText = "Argument parsing error(s):";

        private static readonly string UsageText = string.Format("Usage:{0}  IGP.Tools.DeviceEmulator [OPTIONS] -p [PORT] -d [DEVICE_TYPE]", Environment.NewLine);
        private static readonly string ExampleText = string.Format("Example:{0}  IGP.Tools.DeviceEmulator -p COM10 -d CL31", Environment.NewLine);

        public bool HasError
        {
            get { return LastParserState.Eval(x => x.Errors.Any(), () => false); }
        }

        public bool Help { get; private set; }

        [Option('p', "port", Required = true, HelpText = PortHelpText)]
        public string Port { get; set; }

        [Option('d', "device", Required = true, HelpText = DeviceHelpText)]
        public string DeviceType { get; set; }

        [Option("repository", DefaultValue = "devices", HelpText = RepoHelpText)]
        public string DeviceRepository { get; set; }

        [Option("com", MutuallyExclusiveSet = "serial", HelpText = ComHelpText)]
        public string Address { get; set; }

        [Option("address", MutuallyExclusiveSet = "network", HelpText = AddressHelpText)]
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
                    help.AddPreOptionsLine(SplitterString);
                    help.AddPreOptionsLine(ErrorsHeaderText);
                    help.AddPreOptionsLine(errors);

                    return help;
                }
            }

            help.AddPreOptionsLine(SplitterString);
            help.AddPreOptionsLine(UsageText);
            help.AddOptions(this);
            help.AddPostOptionsLine(ExampleText);

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