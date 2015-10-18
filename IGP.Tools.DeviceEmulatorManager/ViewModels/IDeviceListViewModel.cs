namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Collections.ObjectModel;
    using SBL.Common.Annotations;

    internal interface IDeviceListViewModel
    {
        ObservableCollection<IDeviceViewModel> Devices { [NotNull] get; }
    }
}