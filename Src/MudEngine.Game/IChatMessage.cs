namespace MudDesigner.MudEngine.Game
{
    using MudDesigner.MudEngine.Core.Mediation;

    public interface IChatMessage : IMessage<string>
    {
        IGameComponent Sender { get; }
    }
}
