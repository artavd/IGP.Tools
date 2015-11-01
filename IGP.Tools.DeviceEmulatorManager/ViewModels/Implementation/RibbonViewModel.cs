namespace IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using Prism.Events;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;
    using SBL.Common.Utils;

    internal sealed class RibbonViewModel : IRibbonViewModel
    {
        private readonly IRibbonCommandsProvider _ribbonCommandsProvider;
        private IDeviceViewModel[] _lastSelectedDevices;

        private AggregatedCommand _startEmulatorsCommand;
        private AggregatedCommand _stopEmulatorsCommand;

        public RibbonViewModel(
            [NotNull] IRibbonService ribbonService,
            [NotNull] IRibbonCommandsProvider ribbonCommandsProvider,
            [NotNull] IEventAggregator eventAggregator)
        {
            Contract.ArgumentIsNotNull(ribbonService, () => ribbonService);
            Contract.ArgumentIsNotNull(ribbonCommandsProvider, () => ribbonCommandsProvider);
            Contract.ArgumentIsNotNull(eventAggregator, () => eventAggregator);

            _ribbonCommandsProvider = ribbonCommandsProvider;

            _startEmulatorsCommand = new AggregatedCommand { CanExecuteMode = CanExecuteMode.IfAny };
            _stopEmulatorsCommand = new AggregatedCommand { CanExecuteMode = CanExecuteMode.IfAny };

            eventAggregator
                .GetEvent<ActiveDevicesChanged>()
                .Subscribe(UpdateDevicesCommands);

            ribbonService.RegisterRibbonCommand(
                new RibbonCommandInfo("Start emulators") { Description = "Start active device emulators", Icon = "StartEmulatorsIcon"},
                _startEmulatorsCommand);

            ribbonService.RegisterRibbonCommand(
                new RibbonCommandInfo("Stop emulators") { Description = "Stop active device emulators", Icon = "StopEmulatorsIcon" },
                _stopEmulatorsCommand);
        }

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

        public IEnumerable<RibbonCommand> Commands => _ribbonCommandsProvider.Commands;
    }
}