using MudDesigner.MudEngine.Actors;
using MudDesigner.MudEngine.Commanding;

namespace MudEngine.Game.Actors
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