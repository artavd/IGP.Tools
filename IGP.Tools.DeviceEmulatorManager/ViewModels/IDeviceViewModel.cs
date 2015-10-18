namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Windows.Input;
    using SBL.Common.Annotations;

    interface IDeviceViewModel
    {
        ICommand StartEmulatorCommand { [NotNull] get; }
        ICommand StopEmulatorCommand { [NotNull] get; }
        ICommand ConnectCommand { [NotNull] get; }
        ICommand DisconnectCommand { [NotNull] get; }

        string DeviceName { [NotNull] get; }
        string PortName { [CanBeNull] get; }
    }
}
