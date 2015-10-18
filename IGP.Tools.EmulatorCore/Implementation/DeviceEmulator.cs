namespace IGP.Tools.EmulatorCore.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class DeviceEmulator : IDevice, IDeviceEmulator
    {
        private readonly BehaviorSubject<bool> _switcher = new BehaviorSubject<bool>(true);

        private bool _isStarted;

        public DeviceEmulator(
            [NotNull] string name,
            [NotNull] IEnumerable<IMessageProvider> messageProviders,
            [NotNull] IEncoder encoder)
        {
            Contract.ArgumentIsNotNull(name, () => name);
            Contract.ArgumentIsNotNull(messageProviders, () => messageProviders);
            Contract.ArgumentIsNotNull(encoder, () => encoder);

            Name = name;
            IsTimeIncluded = false;
            IsStarted = true;

            Messages = messageProviders
                .Select(x => Observable
                    .Interval(x.Interval)
                    .Pausable(_switcher)
                    .Select(n => encoder.Encode(Decorate(x.GetNextMessage(), n))));
        }

        public IEnumerable<IObservable<byte[]>> Messages { get; }

        public string Name { get; }

        public bool IsTimeIncluded { get; set; }

        public bool IsStarted
        {
            get { return _isStarted; }
            private set
            {
                _isStarted = value; 
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Start()
        {
            _switcher.OnNext(true);
            IsStarted = true;
        }

        public void Stop()
        {
            _switcher.OnNext(false);
            IsStarted = false;
        }

        public event EventHandler StateChanged;

        private string Decorate(string source, long number) =>
            IsTimeIncluded ? $"{DateTime.Now.ToLocalTime()} | {number:D4} | {source}" : source;

        #region Disposing
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DeviceEmulator()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Messages.Foreach(m => m.DisposeIfPossible());
            }
        }
        #endregion
    }
}