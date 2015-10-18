namespace IGP.Tools.DeviceEmulatorManager.Models
{
    using System;
    using System.Linq;
    using System.Reactive.Disposables;
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class DeviceEmulatorEndPoint : IDisposable
    {
        private readonly IDisposable _subscription;
        private IPort _outputPort;

        public DeviceEmulatorEndPoint([NotNull] IDevice device, [CanBeNull] IPort port = null)
        {
            Contract.ArgumentIsNotNull(device, () => device);
            Contract.OfType<IDeviceEmulator>(device);

            Device = device;
            Emulator = device.As<IDeviceEmulator>();
            OutputPort = port;

            _subscription = new CompositeDisposable(Device.Messages
                .Select(m => m.Subscribe(data => OutputPort?.Transmit(data))));
        }

        public event EventHandler<PortChangedEventArgs> PortChanged;

        public IDevice Device { [NotNull] get; }

        public IDeviceEmulator Emulator { [NotNull] get; }

        [CanBeNull]
        public IPort OutputPort
        {
            get { return _outputPort; }
            set
            {
                IPort oldValue = _outputPort;
                _outputPort = value;

                RaisePortChanged(oldValue);
            }
        }

        private void RaisePortChanged(IPort oldPort)
        {
            PortChanged?.Invoke(this, new PortChangedEventArgs(OutputPort, oldPort));
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }

    internal sealed class PortChangedEventArgs : EventArgs
    {
        public PortChangedEventArgs([CanBeNull] IPort newPort, [CanBeNull] IPort oldPort)
        {
            NewPort = newPort;
            OldPort = oldPort;
        }

        public IPort NewPort { get; }
        public IPort OldPort { get; }
    }
}