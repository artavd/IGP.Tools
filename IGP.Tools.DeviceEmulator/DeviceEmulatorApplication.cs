namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Reactive.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class DeviceEmulatorApplication
    {
        private readonly ApplicationOptions _options;

        private readonly IDeviceFactory _deviceFactory;
        private readonly IPortFactory _portFactory;
        private readonly IEncoder _encoder;

        public DeviceEmulatorApplication(
            [NotNull] ApplicationOptions options,
            [NotNull] IDeviceFactory deviceFactory,
            [NotNull] IPortFactory portFactory,
            [NotNull] IEncoder encoder)
        {
            Contract.ArgumentIsNotNull(options, () => options);
            Contract.ArgumentIsNotNull(deviceFactory, () => deviceFactory);
            Contract.ArgumentIsNotNull(portFactory, () => portFactory);
            Contract.ArgumentIsNotNull(encoder, () => encoder);

            _options = options;
            _deviceFactory = deviceFactory;
            _portFactory = portFactory;
            _encoder = encoder;
        }

        public void Start()
        {
            var port = _portFactory.CreatePort(_options.Port, _options.PortParameters);
            port.Open();

            port.Transmit(_encoder.Encode(GetHelloString()));

            port.Received.Select(data => (char)data[0]).Subscribe(Control);

            var device = _deviceFactory.CreateDevice(_options.DeviceType);
            device.Messages.Foreach(m => m.Subscribe(port.Transmit));
        }

        private void Control(char symbol)
        {
            switch (symbol)
            {
                case 'q': case 'Q':
                    lock (Program.ExitLock)
                    {
                        Monitor.Pulse(Program.ExitLock);
                    }
                    break;

                case 's': case 'S':
                    // TODO: Start/Stop emulator
                    break;

                case 't': case 'T':
                    // TODO: Include/Exclude time from message
                    break;
            }
        }

        private string GetHelloString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(GetHeader());
            
            sb.AppendLine();

            sb.AppendLine("For stopping / restarting emulator         --- press 'S'");
            sb.AppendLine("For close the program                      --- press 'Q'");
            sb.AppendLine("For turn on/off including time in message  --- press 'T'");

            sb.AppendLine();
            sb.AppendLine("Emulator started:");
            sb.AppendLine();

            return sb.ToString();
        }

        private string GetHeader()
        {
            var assembly = Assembly.GetEntryAssembly().GetName();

            var productName = string.Format("= {0} Version {1} =", assembly.Name, assembly.Version.ToString(2));
            var dashLine = string.Empty.PadRight(productName.Length, '=');

            return string.Format("{0}{1}{2}{1}{0}", dashLine, Environment.NewLine, productName);
        }
    }
}