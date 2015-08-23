using MudDesigner.MudEngine.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Networking
{
    public class ConnectionClosedArgs : EventArgs
    {
        public ConnectionClosedArgs(IPlayer player, IConnection connection)
        {
            this.Player = player;
            this.Connection = connection;
        }

        public IPlayer Player { get; }

        public IConnection Connection { get; }
    }
}
