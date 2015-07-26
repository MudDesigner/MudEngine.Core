using System;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Game.Components;

namespace MudDesigner.MudEngine.Game
{
    public interface IGame : IGameComponent
    {
        /// <summary>
        /// Occurs when a world is loaded, prior to initialization of the world.
        /// </summary>
        event Func<DefaultGame, WorldLoadedArgs, Task> WorldLoaded;

        /// <summary>
        /// Gets information pertaining to the game.
        /// </summary>
        GameInformation Information { get; }

        /// <summary>
        /// Gets the Autosaver responsible for automatically saving the game at a set interval.
        /// </summary>
        Autosave<DefaultGame> Autosave { get; }

        /// <summary>
        /// Gets a value indicating that the initialized or not.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets or sets the last saved.
        /// </summary>
        DateTime LastSaved { get; }

        /// <summary>
        /// Gets the current World for the game. Contains all of the Realms, Zones and Rooms.
        /// </summary>
        IWorld[] Worlds { get; }

        Task AddWorld(IWorld world);
    }
}