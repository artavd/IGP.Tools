namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using IGP.Tools.IO;
    using Prism.Commands;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class DeviceViewModel : ViewModelBase, IDeviceViewModel
    {
        private readonly DelegateCommand[] _commands;
        private IDisposable _portStateSubscription;

        public DeviceViewModel([NotNull] DeviceEmulatorEndPoint endPoint)
        {
            Contract.ArgumentIsNotNull(endPoint, () => endPoint);

            EndPoint = endPoint;

            StartEmulatorCommand = new DelegateCommand(
                () => EndPoint.Emulator.Start(),
                () => !EndPoint.Emulator.IsStarted);

            StopEmulatorCommand = new DelegateCommand(
                () => EndPoint.Emulator.Stop(),
                () => EndPoint.Emulator.IsStarted);

            ConnectCommand = new DelegateCommand(
                () => EndPoint.OutputPort?.Connect(),
                () => EndPoint.OutputPort?.CurrentState == PortStates.Disconnected);

            DisconnectCommand = new DelegateCommand(
                () => EndPoint.OutputPort?.Disconnect(),
                () => EndPoint.OutputPort?.CurrentState != PortStates.Disconnected);

            _commands = new[] { StartEmulatorCommand, StopEmulatorCommand, ConnectCommand, DisconnectCommand }
                .Cast<DelegateCommand>()
                .ToArray();

            _portStateSubscription = SubscribeOnPortStateFeed(EndPoint.OutputPort);

            RegisterEventSubscription(
                () => EndPoint.Emulator.StateChanged += UpdateCommandState,
                () => EndPoint.Emulator.StateChanged -= UpdateCommandState);

            RegisterEventSubscription(
                () => EndPoint.PortChanged += BindPort,
                () => EndPoint.PortChanged -= BindPort);
        }

        public ICommand StartEmulatorCommand { get; }
        public ICommand StopEmulatorCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public string DeviceName => EndPoint.Device.Name;
        public string PortName => EndPoint.OutputPort?.Name ?? "Not set";

        private DeviceEmulatorEndPoint EndPoint { get; }

        private void UpdateCommandState(object sender = null, EventArgs args = null)
        {
            _commands.Foreach(c => c.RaiseCanExecuteChanged());
        }

        private void BindPort(object sender, PortChangedEventArgs e)
        {
            _portStateSubscription?.Dispose();
            _portStateSubscription = SubscribeOnPortStateFeed(e.NewPort);

            OnPropertyChanged(() => PortName);
        }

        private IDisposable SubscribeOnPortStateFeed(IPort port)
        {
            return port?.StateFeed.Subscribe(_ => UpdateCommandState());
        }
    }
}