namespace SBL.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public class AggregatedCommand : ICommand, IDisposable
    {
        private readonly HashSet<ICommand> _commands = new HashSet<ICommand>();

        public CanExecuteMode CanExecuteMode { get; set; } = CanExecuteMode.IfAll;

        [NotNull]
        public IEnumerable<ICommand> RegisteredCommands => _commands;

        public event EventHandler CanExecuteChanged;

        public void RegisterCommand([NotNull] ICommand command)
        {
            Contract.ArgumentIsNotNull(command, () => command);
            if (_commands.Add(command))
            {
                command.CanExecuteChanged += CanExecuteChanged;
                RaiseCanExecuteChanged();
            }
        }

        public void UnregisterCommand([NotNull] ICommand command)
        {
            Contract.ArgumentIsNotNull(command, () => command);
            Contract.IsTrue(_commands.Contains(command));

            _commands.Remove(command);

            command.CanExecuteChanged -= CanExecuteChanged;
            RaiseCanExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteMode == CanExecuteMode.IfAll
                ? RegisteredCommands.All(cmd => cmd.CanExecute(parameter))
                : RegisteredCommands.Any(cmd => cmd.CanExecute(parameter));
        }

        public void Execute(object parameter)
        {
            RegisteredCommands
                .Where(cmd => cmd.CanExecute(parameter))
                .Foreach(cmd => cmd.Execute(parameter));
        }

        public void Dispose()
        {
            RegisteredCommands.Foreach(UnregisterCommand);
        }

        private void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum CanExecuteMode
    {
        IfAll,
        IfAny
    }
}