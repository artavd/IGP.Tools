namespace IGP.Tools.DeviceEmulatorManager.Views
{
    using System.Windows.Controls;
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using Microsoft.Practices.Prism.Mvvm;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class PortConfiguratorView : Control, IView
    {
        public PortConfiguratorView([NotNull] IPortConfiguratorViewModel viewModel)
        {
            Contract.ArgumentIsNotNull(viewModel, () => viewModel);

            DataContext = viewModel;
        }
    }
}
