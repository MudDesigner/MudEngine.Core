using System;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine
{
    public interface IGame : IGameComponent
    {
        event Func<IGame, WorldLoadedArgs, Task> WorldLoaded;

        Autosave<IGame> Autosave { get; }

        bool IsRunning { get; }

        // IWorld[] Worlds { get; }

        //Task AddWorld(IWorld world);
    }
}
