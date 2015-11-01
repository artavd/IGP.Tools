namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using SBL.Common.Annotations;

    internal interface IDeviceListViewModel
    {
        ObservableCollection<IDeviceViewModel> Devices { [NotNull] get; }

        ICommand ActiveDevicesChangedCommand { [NotNull] get; }
    }
}