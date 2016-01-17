using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine;
using MudDesigner.MudEngine.Actors;
using MudDesigner.MudEngine.Commanding;
using MudDesigner.MudEngine.MessageBrokering;

namespace MudEngine.Game.Commanding
{
    public class CommandManager : AdapterBase<ICommandingConfiguration>
    {
        private ConcurrentDictionary<IPlayer, Stack<PlayerCommandHistoryItem>> playerCommandsPendingCompletion
            = new ConcurrentDictionary<IPlayer, Stack<PlayerCommandHistoryItem>>();

        private ISubscription commandRequestedSubscription;

        public override string Name { get; } = "Command Manager";

        public IActorCommand[] AvailableCommands { get; private set; }

        public ICommandFactory CommandFactory { get; private set; }

        public IGame Game { get; private set; }

        public override void Configure(ICommandingConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), $"The {typeof(CommandManager).Name} Type requires an instance of {typeof(ICommandingConfiguration).Name} in order to be configured.");
            }

            this.AvailableCommands = configuration.GetCommands();
            this.CommandFactory = configuration.CommandFactory;
        }

        public override Task Delete()
        {
            this.commandRequestedSubscription.Unsubscribe();
            return Task.FromResult(0);
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
            this.commandRequestedSubscription = MessageBrokerFactory.Instance.Subscribe<CommandRequestedMessage>(
                (msg, sub) => this.ProcessCommand(msg));

            return Task.FromResult(0);
        }

        public void ExecuteCommand(IActorCommand command)
        {
        }

        private async Task ProcessCommand(CommandRequestedMessage requestedCommand)
        {
            // Graba  refernce to the player and split up the player command input data.
            IPlayer player = requestedCommand.Content.Target;
            string[] commandAndArgs = requestedCommand.Content.CommandData.Split(' ');
            if (commandAndArgs.Length == 0)
            {
                // TODO: Determine how to present "invalid command" back to the player.
                return;
            }

            string command = commandAndArgs.First();
            if (this.CommandFactory.IsCommandAvailable(command))
            {

            }

            // Check if we command already underway, if so attempt to resume it.
            Stack<PlayerCommandHistoryItem> existingCommandsStack = null;
            if (this.playerCommandsPendingCompletion.TryGetValue(player, out existingCommandsStack))
            {
                PlayerCommandHistoryItem previousCommandItem = existingCommandsStack.Peek();

                // Check if the previous command
                if (await previousCommandItem.Command.CanProcessCommand(player, command))
                {
                    existingCommandsStack.Pop();
                    await this.RunCommand(player, command, previousCommandItem.Command, requestedCommand);
                    return;
                }
            }

            // TODO: Check if we have any elements in the array first.
            IActorCommand potentialCommandToExecute = this.CommandFactory.CreateCommand(commandAndArgs.First());
            if (!(await potentialCommandToExecute.CanProcessCommand(player, command)))
            {
                // TODO: Determine how to notify player of invalid command.
                return;
            }

            await this.RunCommand(player, command, potentialCommandToExecute, requestedCommand);
        }

        private async Task RunCommand(IPlayer player, string command, IActorCommand commandToExecute, CommandRequestedMessage commandMessage)
        {
            CommandResult state = await commandToExecute.ProcessCommand(player, command);
            if (state.IsCompleted)
            {
                await this.CleanupPlayerHistory(player);
                return;
            }

            var historyItem = new PlayerCommandHistoryItem(commandToExecute, commandMessage);
            Stack<PlayerCommandHistoryItem> commandHistoryStack = null;
            if (this.playerCommandsPendingCompletion.TryGetValue(player, out commandHistoryStack))
            {
                commandHistoryStack.Push(historyItem);
                return;
            }

            commandHistoryStack = new Stack<PlayerCommandHistoryItem>();
            this.playerCommandsPendingCompletion.TryAdd(player, commandHistoryStack);
            commandHistoryStack.Push(historyItem);
            player.Deleting += this.CleanupPlayerHistory;
        }

        private Task CleanupPlayerHistory(IGameComponent playerComponent)
        {
            IPlayer player = (IPlayer)playerComponent;
            player.Deleting -= this.CleanupPlayerHistory;
            var historyDictionary = (IDictionary<IPlayer, Stack<PlayerCommandHistoryItem>>)this.playerCommandsPendingCompletion;
            historyDictionary.Remove(player);

            return Task.FromResult(0);
        }
    }
}
