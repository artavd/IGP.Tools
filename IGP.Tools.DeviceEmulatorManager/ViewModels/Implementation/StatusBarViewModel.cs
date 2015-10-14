namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using IGP.Tools.DeviceEmulatorManager.Services;

    internal sealed class StatusBarViewModel : IStatusBarViewModel, INotifyPropertyChanged
    {
        private string _statusMessage;

        public StatusBarViewModel([SBL.Common.Annotations.NotNull] IStatusMessageService status)
        {
            status.StatusMessageFeed.Subscribe(x => StatusMessage = x);
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            private set
            {
                if (value == _statusMessage) return;
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}