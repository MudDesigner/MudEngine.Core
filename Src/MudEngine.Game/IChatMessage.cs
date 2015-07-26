using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Game
{
    public interface IChatMessage : IMessage<string>
    {
        IGameComponent Sender { get; }
    }
}
