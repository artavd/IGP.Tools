namespace IGP.Tools.DeviceEmulatorManager
{
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += HandleException;

            BindSystemCommands();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void BindSystemCommands()
        {
            var minimizeCommandBinding = new CommandBinding(
                SystemCommands.MinimizeWindowCommand,
                (s, e) => SystemCommands.MinimizeWindow(s as Window));

            var restoreCommandBinding = new CommandBinding(
                SystemCommands.RestoreWindowCommand,
                (s, e) => SystemCommands.RestoreWindow(s as Window));

            var maximizeCommandBinding = new CommandBinding(
                SystemCommands.MaximizeWindowCommand,
                (s, e) => SystemCommands.MaximizeWindow(s as Window));

            var closeCommandBinding = new CommandBinding(
                SystemCommands.CloseWindowCommand,
                (s, e) => SystemCommands.CloseWindow(s as Window));

            CommandManager.RegisterClassCommandBinding(typeof(Window), minimizeCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), restoreCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), maximizeCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(Window), closeCommandBinding);
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
