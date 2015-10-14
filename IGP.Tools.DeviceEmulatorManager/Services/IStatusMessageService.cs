namespace IGP.Tools.DeviceEmulatorManager.Services
{
    using System;
    using System.Reactive.Subjects;

    public interface IStatusMessageService
    {
        void ShowStatusMessage(string message, TimeSpan timeout);

        IObservable<string> StatusMessageFeed { get; }
    }

    internal sealed class StatusMessageService : IStatusMessageService
    {
        private readonly ISubject<string> _messageSubject = new BehaviorSubject<string>(string.Empty);

        public void ShowStatusMessage(string message, TimeSpan timeout)
        {
            _messageSubject.OnNext(message);
        }

        public IObservable<string> StatusMessageFeed => _messageSubject;
    }
}