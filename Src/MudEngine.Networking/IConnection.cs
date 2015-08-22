using MudDesigner.MudEngine.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Adapter
{
    public interface IConnection : IInitializableComponent
    {
        event EventHandler<ConnectionClosedArgs> Disconnected;

        bool IsConnectionValid();
    }
}
