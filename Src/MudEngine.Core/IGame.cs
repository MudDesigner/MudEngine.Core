using System;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Environment;
using MudDesigner.MudEngine.Game;

namespace MudDesigner.MudEngine
{
    public interface IGame : IGameComponent
    {
        event Func<IGame, WorldLoadedArgs, Task> WorldLoaded;

        Autosave<IGame> Autosave { get; }

        bool IsRunning { get; }

        Task Configure(IGameConfiguration config);

        IWorld[] Worlds { get; }

        Task AddWorld(IWorld world);
    }
}
