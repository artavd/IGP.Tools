namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += HandleException;

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void HandleException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat("Exception of '{0}' type:", e.Exception.GetType());
            messageBuilder.AppendLine();
            messageBuilder.AppendLine(e.Exception.Message);

            MessageBox.Show(messageBuilder.ToString(), "Unhandled exception occured", MessageBoxButton.OK, MessageBoxImage.Error);

            Shutdown(1);
        }
    }
}
