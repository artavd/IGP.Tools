namespace IGP.Tools.DeviceEmulatorManager.ViewModels.Implementation
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using Prism.Commands;
    using Prism.Events;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class DeviceListViewModel : IDeviceListViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public DeviceListViewModel(
            [NotNull] IEmulatorRepository emulators,
            [NotNull] IEventAggregator eventAggregator)
        {
            Contract.ArgumentIsNotNull(emulators, () => emulators);
            Contract.ArgumentIsNotNull(eventAggregator, () => eventAggregator);

            _eventAggregator = eventAggregator;

            Devices = new ObservableCollection<IDeviceViewModel>(
                emulators.Emulators.Select(x => new DeviceViewModel(x)));

            DeviceSelectionChangedCommand = new DelegateCommand<IList>(PublishSelectionChangedEvent);
        }

        public ObservableCollection<IDeviceViewModel> Devices { get; }

        public ICommand DeviceSelectionChangedCommand { get; }

        private void PublishSelectionChangedEvent(IList selectedItems)
        {
            var items = selectedItems.Cast<IDeviceViewModel>().ToArray();
            _eventAggregator
                .GetEvent<DeviceSelectionChangedEvent>()
                .Publish(items);
        }
    }

    internal sealed class DeviceSelectionChangedEvent : PubSubEvent<IDeviceViewModel[]>
    {
    }
}