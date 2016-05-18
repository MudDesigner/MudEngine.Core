using MudDesigner.MudEngine.Commanding;

namespace MudDesigner.MudEngine.Actors
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer CreatePlayer(IActorCommand initialCommand)
        {
            var player = new MudPlayer(initialCommand);

            return player;
        }
    }
}