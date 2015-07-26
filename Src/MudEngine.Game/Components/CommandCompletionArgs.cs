using System.Collections.Generic;

namespace MudDesigner.MudEngine.Game.Components
{
    public class CommandCompletionArgs
    {
        public CommandCompletionArgs(string command, ICharacter owner, IEnumerable<string> args, InputCommandResult result)
        {
            this.Command = command;
            this.Arguments = args;
            this.Owner = owner;
            this.CommandResult = result;
        }

        public string Command { get; private set; }

        public IEnumerable<string> Arguments { get; private set; }

        public InputCommandResult CommandResult { get; private set; }

        public ICharacter Owner { get; set; }
    }
}