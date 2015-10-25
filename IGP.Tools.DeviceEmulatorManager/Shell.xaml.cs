namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Windows;

    internal partial class Shell : Window
    {
        static Shell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Shell), new FrameworkPropertyMetadata(typeof(Shell)));
        }

        public Shell()
        {
            InitializeComponent();
        }
    }
}
