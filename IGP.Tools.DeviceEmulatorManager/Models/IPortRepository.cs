namespace IGP.Tools.DeviceEmulatorManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using IGP.Tools.DeviceEmulatorManager.Services;
    using IGP.Tools.EmulatorCore;
    using IGP.Tools.IO;
    using SBL.Common.Annotations;

    internal interface IPortRepository
    {
        IEnumerable<IPort> Ports { get; }

        void AddPort([NotNull] IPort port);

        void RemovePort([NotNull] IPort port);
    }

    internal sealed class PortRepository : IPortRepository
    {
        private readonly IList<IPort> _ports = new List<IPort>();

        public IEnumerable<IPort> Ports => _ports;

        public PortRepository(
            [NotNull] IPortFactory portFactory,
            [NotNull] IStatusMessageService status,
            [NotNull] IEncoder encoder)
        {
            //AddPort(portFactory.CreatePort("FILE", "D:\\output1.txt"));
            //AddPort(portFactory.CreatePort("FILE", "D:\\output2.txt"));
            //AddPort(portFactory.CreatePort("FILE", "D:\\output3.txt"));

            AddPort(new StatusMessagePort(1, status, encoder));
            AddPort(new StatusMessagePort(2, status, encoder));
            AddPort(new StatusMessagePort(3, status, encoder));
            AddPort(new StatusMessagePort(4, status, encoder));
        }

        public void AddPort(IPort port)
        {
            _ports.Add(port);
        }

        public void RemovePort(IPort port)
        {
            _ports.Remove(port);
        }

        private sealed class StatusMessagePort : PortBase
        {
            private readonly int _portNumber;
            private readonly IStatusMessageService _statusService;
            private readonly IEncoder _encoder;

            public StatusMessagePort(int portNumber, IStatusMessageService statusService, IEncoder encoder)
            {
                _portNumber = portNumber;
                _statusService = statusService;
                _encoder = encoder;
            }

            public override string Type => "STATUS";
            public override string Name => $"Status port [{_portNumber}]";
            
            protected override IObservable<byte> ReceivedImplementation => Observable.Never<byte>();

            protected override void ConnectImplementation()
            {
                ChangeState(PortStates.Connected);
            }

            protected override async void DisconnectImplementation()
            {
                ChangeState(PortStates.Disconnecting);
                await Task.Delay(TimeSpan.FromSeconds(1));

                ChangeState(PortStates.Disconnected);
            }

            protected override Task<bool> TransmitImplementation(byte[] data)
            {
                string time = DateTime.Now.ToLongTimeString();
                string message = $"[{time}] Data from {Name}:{Environment.NewLine}{_encoder.Decode(data)}";
                _statusService.ShowStatusMessage(message, TimeSpan.Zero);

                return Task.FromResult(true);
            }
        }
    }
}