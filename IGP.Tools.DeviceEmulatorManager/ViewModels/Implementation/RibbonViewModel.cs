namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using Prism.Commands;
    using SBL.Common.Annotations;

    internal sealed class RibbonViewModel : IRibbonViewModel
    {
        private static readonly TimeSpan s_DefaultTimeout = TimeSpan.FromSeconds(5);

        public RibbonViewModel([NotNull] IStatusMessageService status)
        {
            StartEmulatorCommand = new DelegateCommand(() => status.ShowStatusMessage("Emulator started", s_DefaultTimeout));
            StopEmulatorCommand = new DelegateCommand(() => status.ShowStatusMessage("Emulator stopped", s_DefaultTimeout));
            AddDeviceCommand = new DelegateCommand(() => status.ShowStatusMessage("Device added", s_DefaultTimeout));
            RemoveDeviceCommand = new DelegateCommand(() => status.ShowStatusMessage("Device removed", s_DefaultTimeout));
            SaveConfigCommand = new DelegateCommand(() => status.ShowStatusMessage("Config saved", s_DefaultTimeout));
            LoadConfigCommand = new DelegateCommand(() => status.ShowStatusMessage("Config loaded", s_DefaultTimeout));
        }

        public ICommand StartEmulatorCommand { get; }

        public ICommand StopEmulatorCommand { get; }

        public ICommand AddDeviceCommand { get; }

        public ICommand RemoveDeviceCommand { get; }

        public ICommand SaveConfigCommand { get; }

        public ICommand LoadConfigCommand { get; }
    }
}