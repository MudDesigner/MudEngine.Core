using MudDesigner.MudEngine.Game.Character;
using MudDesigner.MudEngine.Game.Components;

namespace MudDesigner.MudEngine.Game
{
    public class CommandRequestMessage : ComponentRequest
    {
        public CommandRequestMessage(ICharacter sender, string commandToProcess, string[] args)
            : base(commandToProcess, sender)
        {
            this.Arguments = args;
        }

        public string[] Arguments { get; private set; }
    }
}
