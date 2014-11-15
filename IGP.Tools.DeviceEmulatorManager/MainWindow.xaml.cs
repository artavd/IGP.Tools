namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Windows;
    using IGP.Tools.DeviceEmulatorManager.ViewModels;
    using SBL.Common.Annotations;

    internal partial class MainWindow : Window
    {
        public MainWindow([NotNull] IMainWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
