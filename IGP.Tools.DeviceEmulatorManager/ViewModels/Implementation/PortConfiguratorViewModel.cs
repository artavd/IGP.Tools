namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using IGP.Tools.DeviceEmulatorManager.Models;
    using IGP.Tools.IO;
    using Prism.Commands;
    using Prism.Events;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class PortConfiguratorViewModel : ViewModelBase, IPortConfiguratorViewModel
    {
        private readonly DelegateCommand _bindPortCommand;
        private DeviceEmulatorEndPoint[] _targetEndPoints;

        public PortConfiguratorViewModel(
            [NotNull] IEventAggregator eventAggregator, 
            [NotNull] IPortRepository portRepository)
        {
            Contract.ArgumentIsNotNull(eventAggregator, () => eventAggregator);

            RegisterForDisposing(eventAggregator
                .GetEvent<DeviceSelectionChangedEvent>()
                .Subscribe(UpdateTargetEndPoints));

            _bindPortCommand = new DelegateCommand(Bind, () => (_targetEndPoints?.Length ?? 0) > 0);

            AvailablePorts = new ObservableCollection<IPortViewModel>(portRepository.Ports.Select(p => new PortViewModel(p)));
        }

        public ICommand BindPortCommand => _bindPortCommand;
        public ObservableCollection<IPortViewModel> AvailablePorts { get; }
        public IPortViewModel SelectedPort { get; set; }

        private void UpdateTargetEndPoints(DeviceEmulatorEndPoint[] targetEndPoints)
        {
            _targetEndPoints = targetEndPoints;
            _bindPortCommand.RaiseCanExecuteChanged();
        }

        private void Bind()
        {
            _targetEndPoints.Foreach(ep => ep.OutputPort = SelectedPort.Port);
        }

        private class PortViewModel : ViewModelBase, IPortViewModel
        {
            public PortViewModel([NotNull] IPort port)
            {
                Contract.ArgumentIsNotNull(port, () => port);

                Port = port;

                RegisterForDisposing(Port.StateFeed.Subscribe(_ => OnPropertyChanged(() => IsConnected)));
            }

            public IPort Port { get; }

            public bool IsConnected => Port.CurrentState.CanTransmit;
        }
    }
}