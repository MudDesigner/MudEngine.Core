using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine;
using MudDesigner.MudEngine.Commanding;
using MudDesigner.MudEngine.MessageBrokering;

namespace MudEngine.Game.Commanding
{
    public class CommandManager : AdapterBase<ICommandingConfiguration>
    {
        public override string Name { get; } = "Command Manager";

        public IActorCommand[] AvailableCommands { get; private set; }

        public IGame Game { get; private set; }

        public override void Configure(ICommandingConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), $"The {typeof(CommandManager).Name} Type requires an instance of {typeof(ICommandingConfiguration).Name} in order to be configured.");
            }

            this.AvailableCommands = configuration.GetCommands();
        }

        public override Task Delete()
        {
            throw new NotImplementedException();
        }

        public override Task Initialize()
        {
            // Nothing to initialize here
            return Task.FromResult(0);
        }

        public override Task Start(IGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), $"The {typeof(CommandManager).Name} requires an instance of {typeof(IGame).Name} in order to start.");
            }

            this.Game = game;
            MessageBrokerFactory.Instance.Subscribe<CommandRequestedMessage>(
                (msg, sub) => this.ProcessCommand(msg));

            return Task.FromResult(0);
        }

        private void ProcessCommand(CommandRequestedMessage command)
        {
            string[] commandAndArgs = command.Content.Split(' ');
            if (commandAndArgs.Length == 0)
            {
                return;
            }

            IActorCommand commandToExecute = this.AvailableCommands.FirstOrDefault(cmd => cmd.Name.ToLower() == commandAndArgs.First());
            commandToExecute.CanProcessCommand(command.Target, commandAndArgs.Skip(1).ToArray());
        }
    }
}
