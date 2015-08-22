using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
