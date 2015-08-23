namespace MudDesigner.MudEngine.Networking
{
    public interface IServerContext
    {
        bool IsHandled { get; set; }

        IServer Server { get; }

        IServerConfiguration Configuration { get; }

		void SetServerState(ServerStatus status);
    }
}
