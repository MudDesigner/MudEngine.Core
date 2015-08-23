using MudDesigner.MudEngine.Actors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MudDesigner.MudEngine.Networking
{
    public interface IConnection : IInitializableComponent
    {
        event EventHandler<ConnectionClosedArgs> Disconnected;

        bool IsConnectionValid();
    }
}
