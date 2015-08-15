using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Actors
{
    public class PlayerCreatedMessage : MessageBase<IPlayer>
    {
        public PlayerCreatedMessage(IPlayer player)
        {
            this.Content = player;
        }
    }
}
