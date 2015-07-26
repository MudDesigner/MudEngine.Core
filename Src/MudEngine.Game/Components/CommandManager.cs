using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Core.Mediation;

namespace MudDesigner.MudEngine.Game.Components
{
    public class CommandManager : ICommandManager
    {
        private IEnumerable<IInputCommand> commandCollection;

        private IEnumerable<ISecurityRole> serverRoles;

        private Stack<IInputCommand> currentlyExecutingCommands;

        private INotificationCenter notificationCenter;

        private ISubscription commandRequestSubscription;

        public CommandManager(IEnumerable<ISecurityRole> availableRoles, IEnumerable<IInputCommand> commands, INotificationCenter notificationCenter)
        {
            this.serverRoles = availableRoles ?? Enumerable.Empty<ISecurityRole>();
            this.commandCollection = commands;
            this.notificationCenter = notificationCenter;
            this.currentlyExecutingCommands = new Stack<IInputCommand>();
        }

        public ICharacter Owner { get; private set; }

        public void SetOwner(ICharacter owningCharacter)
        {
            this.ResetOwner();
            if (owningCharacter == null)
            {
                return;
            }

            this.Owner = owningCharacter;
            this.Owner.Deleting += this.OnOwnerDeleting;
            this.commandRequestSubscription = this.notificationCenter.Subscribe<CommandRequestMessage>(
                (message, subscription) => this.ProcessCommandForCharacter(message.Content, message.Arguments),
                message => message.Sender == this.Owner);
        }

        private void ResetOwner()
        {
            this.Owner = null;
            if (this.commandRequestSubscription != null)
            {
                this.commandRequestSubscription.Unsubscribe();
            }
        }

        private async Task ExecuteCharacterCommand(IInputCommand currentCommand, string[] args)
        {
            if (this.Owner == null)
            {
                this.ResetOwner();
            }

            InputCommandResult commandResult = null;

            // Only enumerate over this collection once.
            bool hasCurrentlyExecutingCommands = this.currentlyExecutingCommands.Any();

            if (currentCommand == null && !hasCurrentlyExecutingCommands)
            {
                // No command was found and we have no state, so tell the user they've entered something invalid.
                commandResult = new InputCommandResult("Unknown Command.\r\n", true, null, this.Owner);
                this.CompleteProcessing(commandResult);
            }
            else if (currentCommand == null && hasCurrentlyExecutingCommands || hasCurrentlyExecutingCommands && this.currentlyExecutingCommands.Peek().ExclusiveCommand)
            {
                // If we have a command already being processed, we continue to process it
                currentCommand = this.currentlyExecutingCommands.Pop();
            }
            
            InputCommandResult result = await currentCommand.ExecuteAsync(this.Owner, args);
            this.CompleteProcessing(result);
        }

        public Task ProcessCommandForCharacter(string command, string[] args)
        {
            //IInputCommand currentCommand = CommandFactory.CreateCommandFromAlias(command);

            //string[] correctedArguments = null;
            //if (this.currentlyExecutingCommands.Any())
            //{
            //    correctedArguments = Enumerable.Concat(new string[] { command, }, args).ToArray();
            //}

            //return this.ProcessCommandForCharacter(currentCommand, correctedArguments);

            throw new NotImplementedException();
        }

        public Task ProcessCommandForCharacter(IInputCommand command, string[] args)
        {
            return this.ExecuteCharacterCommand(command, args);
        }

        private void CompleteProcessing(InputCommandResult result)
        {
            if (result == null)
            {
                throw new NullReferenceException($"{result.CommandExecuted.GetType().Name} returned a null InputCommandResult when it shouldn't have!");
            }

            this.EvaluateCommandState(result);
            this.notificationCenter.Publish(new InformationMessage(result.Result, this.Owner));
            if (result.IsCommandCompleted)
            {
                this.notificationCenter.Publish(new InformationMessage(">>:", this.Owner));
            }
        }

        private void EvaluateCommandState(InputCommandResult commandResult)
        {
            if (commandResult == null || commandResult.IsCommandCompleted)
            {
                return;
            }

            this.currentlyExecutingCommands.Push(commandResult.CommandExecuted);
        }

        private IEnumerable<IInputCommand> SetInitialCharacterCommands(ICharacter character)
        {
            // TODO: Build out command collection
            return Enumerable.Empty<IInputCommand>();
        }

        private Task OnOwnerDeleting(IGameComponent arg)
        {
            this.commandRequestSubscription.Unsubscribe();
            IPlayer player = (IPlayer)arg;
            player.Deleting -= this.OnOwnerDeleting;
            return Task.FromResult(0);
        }
    }
}
