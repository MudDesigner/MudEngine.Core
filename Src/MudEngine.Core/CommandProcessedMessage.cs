using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public class CommandProcessedMessage : MessageBase<string>
    {
        public CommandProcessedMessage(string message, IComponent target)
        {
            this.Content = message;
            this.Target = target;
        }

        public IComponent Target { get; private set; }
    }
}
