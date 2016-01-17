using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandRequestedMessage : MessageBase<string>
    {
        public CommandRequestedMessage(string commandData, IActor target)
        {
            base.Content = commandData;
            this.Target = target;
        }

        public IActor Target { get; }
    }
}
