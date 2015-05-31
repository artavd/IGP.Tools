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

        public IEnumerable<IObservable<byte[]>> Messages { get; private set; }

        public string Name { get; private set; }

        public bool IsTimeIncluded { get; set; }

        public bool IsStarted { get; private set; }

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

        private string Decorate(string source, long number)
        {
            return IsTimeIncluded ?
                string.Format("{0} | {1:D4} | {2}", DateTime.Now.ToLocalTime(), number, source) :
                source;
        }

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