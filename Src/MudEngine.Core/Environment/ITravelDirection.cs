using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Environment
{
    public interface ITravelDirection
    {
        string Direction { get; }

        ITravelDirection GetOppositeDirection();
    }
}
