namespace IGP.Tools.DeviceEmulatorManager.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using IGP.Tools.EmulatorCore;
    using SBL.Common.Annotations;

    internal interface IEmulatorRepository
    {
        IEnumerable<DeviceEmulatorEndPoint> Emulators { get; }

        void AddDevice(DeviceEmulatorEndPoint info);

        void RemoveDevice(DeviceEmulatorEndPoint info);
    }

    internal sealed class EmulatorRepository : IEmulatorRepository
    {
        private readonly IList<DeviceEmulatorEndPoint> _devices = new List<DeviceEmulatorEndPoint>();

        public EmulatorRepository([NotNull] IDeviceFactory deviceFactory, [NotNull] IPortRepository ports)
        {
            AddDevice(new DeviceEmulatorEndPoint(deviceFactory.CreateDevice("CL31"), ports.Ports.ElementAt(0)));
            AddDevice(new DeviceEmulatorEndPoint(deviceFactory.CreateDevice("FD12P_mes7"), ports.Ports.ElementAt(1)));
            AddDevice(new DeviceEmulatorEndPoint(deviceFactory.CreateDevice("LT31"), ports.Ports.ElementAt(2)));
        }

        public IEnumerable<DeviceEmulatorEndPoint> Emulators
        {
            get { return _devices; }
        }

        public void AddDevice(DeviceEmulatorEndPoint info)
        {
            _devices.Add(info);
        }

        public void RemoveDevice(DeviceEmulatorEndPoint info)
        {
            _devices.Remove(info);
        }
    }
}