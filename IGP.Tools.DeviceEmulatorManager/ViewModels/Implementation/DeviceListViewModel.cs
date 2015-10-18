namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class DeviceListViewModel : IDeviceListViewModel
    {
        private readonly IEmulatorRepository _emulators;

        public DeviceListViewModel([NotNull] IEmulatorRepository emulators)
        {
            Contract.ArgumentIsNotNull(emulators, () => emulators);

            _emulators = emulators;

            Devices = new ObservableCollection<IDeviceViewModel>(_emulators.Emulators.Select(x => new DeviceViewModel(x)));
        }

        public ObservableCollection<IDeviceViewModel> Devices { get; }
    }
}