namespace IGP.Tools.DeviceEmulator
{
    using System;
    using System.Reactive.Disposables;
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

        private IPort _port;
        private IDevice _device;

        private readonly CompositeDisposable _finisher = new CompositeDisposable();

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
            _port.Connect();

            _port.Transmit(_encoder.Encode(GetHelloString()));

            _finisher.Add(_port.ReceivedFeed.Select(data => (char)data).Subscribe(Control));

            _device = _deviceFactory.CreateDevice(_options.DeviceType);
            _device.Messages.Foreach(m => _finisher.Add(m.Subscribe(_port.Transmit)));

            _finisher.Add(_port);
            _finisher.Add(_device);
        }

        public void Stop(bool isError = false)
        {
            _port.Transmit(_encoder.Encode(GetGoodbyeString()));
            _finisher.Dispose();

            if (!isError)
            {
                lock (Program.ExitLock)
                {
                    Monitor.Pulse(Program.ExitLock);
                }
            }
        }

        private void Control(char symbol)
        {
            var controller = CheckControl(_device);
            switch (symbol)
            {
                case 'q': case 'Q':
                    Stop();
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

        private static string GetGoodbyeString() => $"Device emulator work finished.{Environment.NewLine}";

        private string GetHeader()
        {
            var assembly = Assembly.GetEntryAssembly().GetName();

            var productName = $"= {assembly.Name} Version {assembly.Version.ToString(2)} =";
            var dashLine = string.Empty.PadRight(productName.Length, '=');

            return string.Format("{0}{1}{2}{1}{0}", dashLine, Environment.NewLine, productName);
        }
    }
}