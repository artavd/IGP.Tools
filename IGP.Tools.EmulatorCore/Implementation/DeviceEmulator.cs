namespace IGP.Tools.EmulatorCore.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class DeviceEmulator : IDevice
    {
        public IList<IObservable<byte[]>> Messages { get; private set; }

        public string Name { get; private set; }

        public bool IsTimeIncluded { get; set; }

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

            Messages = new List<IObservable<byte[]>>();
            foreach (var mp in messageProviders)
            {
                var messageFeed = Observable
                    .Interval(mp.Interval)
                    .Select(_ => encoder.Encode(mp.GetNextMessage()));

                Messages.Add(messageFeed);
            }
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
            if (disposing) { }
        }
        #endregion
    }
}
