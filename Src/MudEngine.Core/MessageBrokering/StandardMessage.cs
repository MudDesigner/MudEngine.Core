using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.MessageBrokering
{
    public sealed class StandardMessage : MessageBase<string>
    {
        public StandardMessage(string message)
        {
            this.Content = message;
        }
    }
}
