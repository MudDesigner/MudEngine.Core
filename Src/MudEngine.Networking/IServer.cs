namespace MudDesigner.MudEngine.Adapter
{
    public interface IServer : IAdapter
    {
        string Owner { get; set; }

        int RunningPort { get; }

        ServerStatus Status { get; }

        void Configure(IServerConfiguration configuration);
    }
}