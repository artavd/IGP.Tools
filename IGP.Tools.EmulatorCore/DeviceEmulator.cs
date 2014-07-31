namespace IGP.Tools.EmulatorCore
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Text;

    internal class DeviceEmulator : IDevice
    {
        public IList<IObservable<byte[]>> Messages { get; private set; }

        public string Name { get; set; }

        public bool IsTimeIncluded { get; set; }

        public DeviceEmulator(string name, IEnumerable<IMessageProvider> messageProviders)
        {
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

        // TODO: AA Move encoder to service
        private static byte[] Encode(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }
    }
}
