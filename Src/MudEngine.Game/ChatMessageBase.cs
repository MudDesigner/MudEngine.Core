namespace MudDesigner.MudEngine.Game
{
    public abstract class ChatMessageBase : IChatMessage
    {
        public ChatMessageBase(string message, IGameComponent sender)
        {
            this.Content = message;
            this.Sender = sender;
        }

        public string Content { get; protected set; }

        public IGameComponent Sender { get; protected set; }

        public object GetContent()
        {
            return this.GetContent();
        }
    }
}
