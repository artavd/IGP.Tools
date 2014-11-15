namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Collections.ObjectModel;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class DeviceListViewModel : IDeviceListViewModel
    {
        private readonly IEmulatorRepository _emulators;

        public ObservableCollection<DeviceEmulatorInfo> Devices { get; private set; } 

        public DeviceListViewModel([NotNull] IEmulatorRepository emulators)
        {
            Contract.ArgumentIsNotNull(emulators, () => emulators);

            _emulators = emulators;

            Devices = new ObservableCollection<DeviceEmulatorInfo>(_emulators.Emulators);
        }
    }
}