using MudDesigner.MudEngine.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Adapter
{
    public sealed class ClientData
    {
        public ClientData(IPlayer player, IConnection connection, string data)
        {
            this.Player = player;
            this.Connection = connection;
            this.Data = data;
        }

        public IPlayer Player { get; }

        public IConnection Connection { get; }

        public string Data { get; }
    }
}
