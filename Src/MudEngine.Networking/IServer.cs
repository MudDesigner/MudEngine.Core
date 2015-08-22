namespace MudDesigner.MudEngine.Networking
{
    public interface IServer : IAdapter
    {
        string Owner { get; set; }

        int RunningPort { get; }

        ServerStatus Status { get; }

        void Configure(IServerConfiguration configuration);
    }
}