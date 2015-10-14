namespace IGP.Tools.DeviceEmulatorManager.Views
{
    using System.Windows.Controls;
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using SBL.Common;
    using SBL.Common.Annotations;

    internal sealed class RibbonView : Control
    {
        public RibbonView([NotNull] IRibbonViewModel viewModel)
        {
            Contract.ArgumentIsNotNull(viewModel, () => viewModel);

            DataContext = viewModel;
        }
    }
}
