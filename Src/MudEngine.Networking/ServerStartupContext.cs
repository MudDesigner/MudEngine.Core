using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Adapter
{
    public class ServerContext
    {
        private WindowsServer windowsServer;

        internal ServerContext(WindowsServer server, IServerConfiguration configuration, Socket serverSocket)
        {
            this.ListeningSocket = serverSocket;
            this.Server = server;
            this.windowsServer = server;
            this.Configuration = configuration;
        }

        public Socket ListeningSocket { get; set; }

        public bool IsHandled { get; set; }

        public IServer Server { get; private set; }

        public IServerConfiguration Configuration { get; private set; }

        public void SetServerState(ServerStatus status)
        {
            this.windowsServer.Status = status;
        }
    }
}
