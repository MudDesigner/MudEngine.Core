using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public class AdapterDeletedMessage : MessageBase<IAdapter>
    {
        public AdapterDeletedMessage(IAdapter adapter)
        {
            this.Content = adapter;
        }
    }
}
