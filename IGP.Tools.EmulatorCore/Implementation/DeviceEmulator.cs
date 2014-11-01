namespace IGP.Tools.EmulatorCore.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Text;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal class DeviceEmulator : IDevice
    {
        public IList<IObservable<byte[]>> Messages { get; private set; }

        public string Name { get; private set; }

        public bool IsTimeIncluded { get; set; }

        public DeviceEmulator(
            [NotNull] string name,
            [NotNull] IEnumerable<IMessageProvider> messageProviders)
        {
            Contract.ArgumentIsNotNull(name, () => name);
            Contract.ArgumentIsNotNull(messageProviders, () => messageProviders);

            Name = name;
            IsTimeIncluded = false;

            Messages = new List<IObservable<byte[]>>();
            foreach (var mp in messageProviders)
            {
                var messageFeed = Observable
                    .Interval(mp.Interval)
                    .Select(_ => Encode(mp.GetNextMessage()));

                Messages.Add(messageFeed);
            }
        }

        // TODO: #9 Create service for encoding
        private static byte[] Encode(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }
    }
}
