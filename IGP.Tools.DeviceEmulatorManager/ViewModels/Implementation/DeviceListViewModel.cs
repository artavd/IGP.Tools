namespace IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using Prism.Commands;
    using Prism.Events;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Utils;

    internal sealed class DeviceListViewModel : IDeviceListViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public DeviceListViewModel(
            [NotNull] IEmulatorRepository emulators,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] IRibbonService ribbonService)
        {
            Contract.ArgumentIsNotNull(emulators, () => emulators);
            Contract.ArgumentIsNotNull(eventAggregator, () => eventAggregator);
            Contract.ArgumentIsNotNull(ribbonService, () => ribbonService);

            _eventAggregator = eventAggregator;

            Devices = new ObservableCollection<IDeviceViewModel>(
                emulators.Emulators.Select(x => new DeviceViewModel(x)));

            ActiveDevicesChangedCommand = new DelegateCommand<IList>(PublishSelectionChangedEvent);

            var startAllEmulatorsCommand = new AggregatedCommand(Devices.Select(x => x.StartEmulatorCommand)) { CanExecuteMode = CanExecuteMode.IfAny};
            var stopAllEmulatorsCommand = new AggregatedCommand(Devices.Select(x => x.StopEmulatorCommand)) { CanExecuteMode = CanExecuteMode.IfAny};

            ribbonService.RegisterRibbonCommand(new RibbonCommandInfo("Start all emulators"), startAllEmulatorsCommand);
            ribbonService.RegisterRibbonCommand(new RibbonCommandInfo("Stop all emulators"), stopAllEmulatorsCommand);
        }

        public ObservableCollection<IDeviceViewModel> Devices { get; }

        public ICommand ActiveDevicesChangedCommand { get; }

        private void PublishSelectionChangedEvent(IList selectedItems)
        {
            var items = selectedItems.Cast<IDeviceViewModel>().ToArray();
            _eventAggregator
                .GetEvent<ActiveDevicesChanged>()
                .Publish(items);
        }
    }

    internal sealed class ActiveDevicesChanged : PubSubEvent<IDeviceViewModel[]>
    {
    }
}