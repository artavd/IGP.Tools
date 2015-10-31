namespace IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using Prism.Commands;
    using Prism.Events;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;
    using SBL.Common.Utils;

    internal sealed class RibbonViewModel : IRibbonViewModel
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

        private IDeviceViewModel[] _lastSelectedDevices;

        private AggregatedCommand _startEmulatorsCommand;
        private AggregatedCommand _stopEmulatorsCommand;

        public RibbonViewModel([NotNull] IStatusMessageService status, [NotNull] IEventAggregator eventAggregator)
        {
            Contract.ArgumentIsNotNull(status, () => status);
            Contract.ArgumentIsNotNull(eventAggregator, () => eventAggregator);

            _startEmulatorsCommand = new AggregatedCommand { CanExecuteMode = CanExecuteMode.IfAny };
            _stopEmulatorsCommand = new AggregatedCommand { CanExecuteMode = CanExecuteMode.IfAny };

            AddDeviceCommand = new DelegateCommand(() => status.ShowStatusMessage("Device added", DefaultTimeout));
            RemoveDeviceCommand = new DelegateCommand(() => status.ShowStatusMessage("Device removed", DefaultTimeout));
            SaveConfigCommand = new DelegateCommand(() => status.ShowStatusMessage("Config saved", DefaultTimeout));
            LoadConfigCommand = new DelegateCommand(() => status.ShowStatusMessage("Config loaded", DefaultTimeout));

            eventAggregator
                .GetEvent<DeviceSelectionChangedEvent>()
                .Subscribe(UpdateDevicesCommands);
        }

        public ICommand StartEmulatorsCommand => _startEmulatorsCommand;
        public ICommand StopEmulatorsCommand => _stopEmulatorsCommand;
        public ICommand AddDeviceCommand { get; }
        public ICommand RemoveDeviceCommand { get; }
        public ICommand SaveConfigCommand { get; }
        public ICommand LoadConfigCommand { get; }

        private void UpdateDevicesCommands(IDeviceViewModel[] selectedDevices)
        {
            _lastSelectedDevices?.Except(selectedDevices).Foreach(x =>
            {
                _startEmulatorsCommand.UnregisterCommand(x.StartEmulatorCommand);
                _stopEmulatorsCommand.UnregisterCommand(x.StopEmulatorCommand);
            });

            _lastSelectedDevices = selectedDevices;
            _lastSelectedDevices.Foreach(x =>
            {
                _startEmulatorsCommand.RegisterCommand(x.StartEmulatorCommand);
                _stopEmulatorsCommand.RegisterCommand(x.StopEmulatorCommand);
            });
        }
    }
}