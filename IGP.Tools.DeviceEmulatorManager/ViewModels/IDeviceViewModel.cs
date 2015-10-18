namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using IGP.Tools.IO;
    using Prism;
    using Prism.Commands;
    using Prism.Mvvm;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    interface IDeviceViewModel
    {
        ICommand StartEmulatorCommand { [NotNull] get; }
        ICommand StopEmulatorCommand { [NotNull] get; }
        ICommand ConnectCommand { [NotNull] get; }
        ICommand DisconnectCommand { [NotNull] get; }

        bool IsEmulatorStarted { get; }
        bool IsPortConnected { get; }

        string DeviceName { get; }
        string PortName { get; }

        bool IsActive { get; set; }
    }

    internal sealed class DeviceViewModel : BindableBase, IDeviceViewModel
    {
        private readonly IActiveAware[] _commands;

        private bool _isActive;

        public DeviceViewModel([NotNull] DeviceEmulatorInfo info)
        {
            Contract.ArgumentIsNotNull(info, () => info);

            Info = info;

            StartEmulatorCommand = new DelegateCommand(() =>
            {
                Info.Emulator.Start();
                OnPropertyChanged(nameof(IsEmulatorStarted));
            });

            StopEmulatorCommand = new DelegateCommand(() =>
            {
                Info.Emulator.Stop();
                OnPropertyChanged(nameof(IsEmulatorStarted));
            });

            ConnectCommand = new DelegateCommand(() => Info.OutputPort.Connect());
            DisconnectCommand = new DelegateCommand(() => Info.OutputPort.Disconnect());

            _commands = new[] { StartEmulatorCommand, StopEmulatorCommand, ConnectCommand, DisconnectCommand }
                .Cast<IActiveAware>()
                .ToArray();

            // TODO: AA: Add unsubscribe
            info.OutputPort.StateFeed.Subscribe(x => OnPropertyChanged(nameof(IsPortConnected)));

            BindEmulatorWithPort();
        }

        public ICommand StartEmulatorCommand { get; }
        public ICommand StopEmulatorCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }

        public bool IsEmulatorStarted => Info.Emulator.IsStarted;
        public bool IsPortConnected => Info.OutputPort.CurrentState == PortStates.Connected;

        public string DeviceName => Info.Device.Name;
        public string PortName => Info.OutputPort.Name;

        public DeviceEmulatorInfo Info { get; }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
                _commands.Foreach(x => x.IsActive = _isActive);
            }
        }

        private void BindEmulatorWithPort()
        {
            // TODO: AA: Unsubscribe
            Info.Device.Messages.Foreach(m => m.Subscribe(x =>
            {
                if (Info.OutputPort.CurrentState.CanTransmit)
                {
                    Info.OutputPort.Transmit(x);
                }
            }));
        }
    }
}
