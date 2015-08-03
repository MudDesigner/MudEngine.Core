using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine
{
    public sealed class WorldLoadedArgs : EventArgs
    {
        public WorldLoadedArgs(IWorld world)
        {
            this.World = world;
        }

        public IWorld World { get; private set; }
    }
}
