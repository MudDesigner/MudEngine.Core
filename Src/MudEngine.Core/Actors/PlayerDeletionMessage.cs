using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Actors
{
    public class PlayerDeletionMessage: MessageBase<IPlayer>
    {
        public PlayerDeletionMessage(IPlayer player)
        {
            this.Content = player;
        }
    }
}
