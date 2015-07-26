using MudDesigner.MudEngine.Core.Mediation;

namespace MudDesigner.MudEngine.Game
{
    public class CommandMessage : IMessage<string>
    {
        public CommandMessage(string command, string content)
        {
            this.Command = command;
            this.Content = content;
        }

        public string Content { get; private set; }

        public string Command { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
