namespace IGP.Tools.DeviceEmulatorManager.Views
{
    using System.Windows;
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using SBL.Common;
    using SBL.Common.Annotations;

    public partial class MainWindow : Window
    {
        public MainWindow([NotNull] IMainWindowViewModel viewModel)
        {
            Contract.ArgumentIsNotNull(viewModel, () => viewModel);

            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
