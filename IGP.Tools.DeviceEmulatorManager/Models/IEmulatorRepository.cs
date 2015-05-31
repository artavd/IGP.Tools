namespace IGP.Tools.DeviceEmulatorManager.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using IGP.Tools.EmulatorCore;
    using SBL.Common.Annotations;

    internal interface IEmulatorRepository
    {
        IEnumerable<DeviceEmulatorInfo> Emulators { get; }

        void AddDevice(DeviceEmulatorInfo info);

        void RemoveDevice(DeviceEmulatorInfo info);
    }

    internal sealed class EmulatorRepository : IEmulatorRepository
    {
        private readonly IList<DeviceEmulatorInfo> _devices = new List<DeviceEmulatorInfo>();

        public EmulatorRepository([NotNull] IDeviceFactory deviceFactory, [NotNull] IPortRepository ports)
        {
            AddDevice(new DeviceEmulatorInfo(deviceFactory.CreateDevice("CL31"), ports.Ports.ElementAt(0)));
            AddDevice(new DeviceEmulatorInfo(deviceFactory.CreateDevice("FD12P_mes7"), ports.Ports.ElementAt(1)));
            AddDevice(new DeviceEmulatorInfo(deviceFactory.CreateDevice("LT31"), ports.Ports.ElementAt(2)));
        }

        public IEnumerable<DeviceEmulatorInfo> Emulators
        {
            get { return _devices; }
        }

        public void AddDevice(DeviceEmulatorInfo info)
        {
            _devices.Add(info);
        }

        public void RemoveDevice(DeviceEmulatorInfo info)
        {
            _devices.Remove(info);
        }
    }
}