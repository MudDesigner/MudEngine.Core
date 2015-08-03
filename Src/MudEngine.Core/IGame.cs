using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Environment;
using MudDesigner.MudEngine.Game;

namespace MudDesigner.MudEngine
{
    public interface IGame : IGameComponent
    {
        bool IsRunning { get; }

        Task Configure(IGameConfiguration config);

        IWorld[] Worlds { get; }

        Task AddWorld(IWorld world);

        Task AddWorlds(IEnumerable<IWorld> worlds);
    }
}
