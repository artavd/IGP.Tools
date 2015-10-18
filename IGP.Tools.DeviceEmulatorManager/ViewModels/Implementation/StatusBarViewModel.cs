namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using Prism.Mvvm;
    using SBL.Common.Annotations;

    internal sealed class StatusBarViewModel : BindableBase, IStatusBarViewModel
    {
        private string _statusMessage;

        public StatusBarViewModel([NotNull] IStatusMessageService status)
        {
            // TODO: AA: Unsubscribe
            status.StatusMessageFeed.Subscribe(x => StatusMessage = x);
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            private set { SetProperty(ref _statusMessage, value); }
        }
    }
}