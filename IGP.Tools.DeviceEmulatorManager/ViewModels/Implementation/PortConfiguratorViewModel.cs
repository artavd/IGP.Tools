namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System.Linq;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using Prism.Events;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class PortConfiguratorViewModel : ViewModelBase, IPortConfiguratorViewModel
    {
        private string _test;

        public PortConfiguratorViewModel([NotNull] IEventAggregator eventAggregator)
        {
            Contract.ArgumentIsNotNull(eventAggregator, () => eventAggregator);

            RegisterForDisposing(eventAggregator
                .GetEvent<DeviceSelectionChangedEvent>()
                .Subscribe(UpdateTargetEndPoints));
        }

        public string Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        private void UpdateTargetEndPoints(DeviceEmulatorEndPoint[] targetEndPoints)
        {
            Test = string.Join(", ", targetEndPoints.Select(x => x.Device.Name));
        }
    }
}