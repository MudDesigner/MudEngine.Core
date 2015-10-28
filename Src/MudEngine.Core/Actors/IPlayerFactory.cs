using MudDesigner.MudEngine.Commanding;

namespace MudDesigner.MudEngine.Actors
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(IActorCommand initialCommand);
    }
}