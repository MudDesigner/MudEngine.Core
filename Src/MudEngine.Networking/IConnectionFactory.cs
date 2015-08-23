using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Networking
{
    public interface IConnectionFactory<TServer> where TServer : class, IServer
    {
        IConnection CreateConnection(IPlayer player, TServer server);
    }
}
