using MudDesigner.MudEngine.Core.Mediation;

namespace MudDesigner.MudEngine.Game.Components
{
    public abstract class CharacterMessage : IMessage<string>
    {
        public CharacterMessage(string message, ICharacter targetCharacter)
        {
            this.Content = message;
            this.Target = targetCharacter;
        }

        public string Content { get; private set; }

        public ICharacter Target { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
