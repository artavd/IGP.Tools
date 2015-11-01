namespace IGP.Tools.DeviceEmulatorManager.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Microsoft.Practices.Unity.Configuration;
    using SBL.Common;
    using SBL.Common.Annotations;

    public interface IRibbonService
    {
        void RegisterRibbonCommand(RibbonCommandInfo commandInfo, ICommand command);
    }

    internal interface IRibbonCommandsProvider
    {
        IEnumerable<RibbonCommand> Commands { get; }
    }

    public struct RibbonCommandInfo
    {
        private string _name;

        public RibbonCommandInfo([NotNull] string name)
        {
            _name = null;

            Description = name;
            Icon = null;
            IsVisibleWhenCannotExecute = true;

            Name = name;
        }

        [NotNull]
        public string Name
        {
            get { return _name; }
            set
            {
                Contract.IsTrue(!string.IsNullOrWhiteSpace(value));
                _name = value;
            }
        }

        public string Description { get; set; }

        public string Icon { get; set; }

        public bool IsVisibleWhenCannotExecute { get; set; }
    }

    internal class RibbonService : IRibbonService, IRibbonCommandsProvider
    {
        private readonly IList<RibbonCommand> _commands = new List<RibbonCommand>();

        public IEnumerable<RibbonCommand> Commands => _commands;

        public void RegisterRibbonCommand(RibbonCommandInfo commandInfo, [NotNull] ICommand command)
        {
            Contract.ArgumentIsNotNull(command, () => command);
            if (!_commands.Select(x => x.Command).Contains(command))
            {
                _commands.Add(new RibbonCommand(commandInfo, command));
            }
        }
    }

    internal sealed class RibbonCommand
    {
        public RibbonCommand(RibbonCommandInfo info, [NotNull] ICommand command)
        {
            Contract.ArgumentIsNotNull(command, () => command);
            Contract.ArgumentIsNotNull(info.Name, () => info.Name);

            Info = info;
            Command = command;
        }

        public RibbonCommandInfo Info { get; }

        public ICommand Command { get; }
    }
}