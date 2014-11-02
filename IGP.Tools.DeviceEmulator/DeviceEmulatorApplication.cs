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
    using SBL.Common.Utils;

    internal sealed class DeviceEmulatorApplication
    {
        private readonly ApplicationOptions _options;

        private readonly IDeviceFactory _deviceFactory;
        private readonly IPortFactory _portFactory;
        private readonly IEncoder _encoder;

        private IPort _port;
        private IDevice _device;

        private readonly DisposableChain _finisher = new DisposableChain();

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
            _port = _portFactory.CreatePort(_options.Port, _options.PortParameters);
            _port.Open();

            _port.Transmit(_encoder.Encode(GetHelloString()));

            _finisher.AddToChain(_port.Received.Select(data => (char)data[0]).Subscribe(Control));

            _device = _deviceFactory.CreateDevice(_options.DeviceType);
            _device.Messages.Foreach(m => _finisher.AddToChain(m.Subscribe(_port.Transmit)));

            _finisher.AddToChain(_port);
            _finisher.AddToChain(_device);
        }

        private void Control(char symbol)
        {
            var controller = CheckControl(_device);
            switch (symbol)
            {
                case 'q': case 'Q':
                    lock (Program.ExitLock)
                    {
                        _port.Transmit(_encoder.Encode(GetGoodbyeString()));
                        _finisher.Dispose();
                        Monitor.Pulse(Program.ExitLock);
                    }
                    break;

                case 's': case 'S':
                    if (controller.IsStarted)
                    {
                        controller.Stop();
                    }
                    else
                    {
                        controller.Start();
                    }
                    break;

                case 't': case 'T':
                    controller.IsTimeIncluded = !controller.IsTimeIncluded;
                    break;
            }
        }

        private IDeviceEmulator CheckControl(IDevice device)
        {
            var controller = device as IDeviceEmulator;
            if (controller == null)
            {
                _port.Transmit(_encoder.Encode("Error! Can't control device."));
            }

            return controller;
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

        private string GetGoodbyeString()
        {
            return "Device emulator work finished.";
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