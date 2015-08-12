using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public class InvalidAdapterStateException : Exception
    {
        public InvalidAdapterStateException(IAdapter adapter, string message) : base(message)
        {
            this.Adapter = adapter;
        }

        public IAdapter Adapter { get; private set; }
    }
}
