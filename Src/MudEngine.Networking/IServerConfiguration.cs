using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Networking
{
    public interface IServerConfiguration : IConfiguration
    {
        Action<IServerContext> OnServerStartup { get; set; }

        Action<IServerContext> OnServerShutdown { get; set; }

        int Port { get; set; }

        int MaxQueuedConnections { get; set; }

        int MinimumPasswordSize { get; }

        int MaximumPasswordSize { get; }

        int PreferedBufferSize { get; }

        string[] MessageOfTheDay { get; set; }

        string ConnectedMessage { get; set; }
    }
}
