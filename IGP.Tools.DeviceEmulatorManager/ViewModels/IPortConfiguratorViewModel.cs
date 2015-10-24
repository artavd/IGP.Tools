namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IGP.Tools.IO;
    using SBL.Common.Annotations;

    internal interface IPortConfiguratorViewModel
    {
        ICommand BindPortCommand {[NotNull] get; }

        ObservableCollection<IPortViewModel> AvailablePorts { [NotNull] get; }

        [NotNull]
        IPortViewModel SelectedPort { get; set; }
    }

    internal interface IPortViewModel
    {
        IPort Port { [NotNull] get; }

        bool IsConnected { get; }
    }
}