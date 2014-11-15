namespace IGP.Tools.DeviceEmulatorManager.Models
{
    using System.Collections.Generic;
    using IGP.Tools.IO;
    using SBL.Common.Annotations;

    public interface IPortRepository
    {
        IEnumerable<IPort> Ports { get; }

        void AddPort([NotNull] IPort port);

        void RemovePort([NotNull] IPort port);
    }

    class PortRepository : IPortRepository
    {
        private readonly IList<IPort> _ports = new List<IPort>();

        public IEnumerable<IPort> Ports
        {
            get { return _ports; }
        }

        public PortRepository([NotNull] IPortFactory portFactory)
        {
            AddPort(portFactory.CreatePort("FILE", "D:\\output1.txt"));
            AddPort(portFactory.CreatePort("FILE", "D:\\output2.txt"));
            AddPort(portFactory.CreatePort("FILE", "D:\\output3.txt"));
        }

        public void AddPort(IPort port)
        {
            _ports.Add(port);
        }

        public void RemovePort(IPort port)
        {
            _ports.Remove(port);
        }
    }
}