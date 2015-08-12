using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public interface IDescriptor
    {
        string Name { get; }

        string Description { get; set; }

        void SetName(string name);
    }
}
