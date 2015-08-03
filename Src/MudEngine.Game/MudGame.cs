//-----------------------------------------------------------------------
// <copyright file="DefaultGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Environment;

    /// <summary>
    /// The Default engine implementation of the IGame interface. This implementation provides validation support via ValidationBase.
    /// </summary>
    public class MudGame : GameComponent, IGame
    {
        //private ILoggingService loggingService;

        //private IWorldService worldService;

        private List<IWorld> worlds = new List<IWorld>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MudGame" /> class.
        /// </summary>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="worldService">The world service.</param>
        public MudGame(/*ILoggingService loggingService, IWorldService worldService*/)
        {
            //ExceptionFactory
            //    .ThrowIf<ArgumentNullException>(loggingService == null, "Logging service can not be null.", this)
            //    .Or(worldService == null, "World service can not be null.");

            //this.loggingService = loggingService;
            //this.worldService = worldService;

            this.Configuration = new GameConfiguration();
            this.Autosave = new Autosave<IGame>(this, this.SaveWorlds) { AutoSaveFrequency = 1 };
        }

        public event Func<IGame, WorldLoadedArgs, Task> WorldLoaded;

        /// <summary>
        /// Gets information pertaining to the game.
        /// </summary>
        public IGameConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Gets the Autosaver responsible for automatically saving the game at a set interval.
        /// </summary>
        public Autosave<IGame> Autosave { get; protected set; }

        /// <summary>
        /// Gets a value indicating that the initialized or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets or sets the last saved.
        /// </summary>
        public DateTime LastSaved { get; private set; }

        /// <summary>
        /// Gets the current World for the game. Contains all of the Realms, Zones and Rooms.
        /// </summary>
        public IWorld[] Worlds
        {
            get
            {
                return this.worlds.ToArray();
            }
        }

        public Task Configure(IGameConfiguration configuration)
        {
            this.Configuration = configuration;
            return Task.FromResult(0);
        }

        /// <summary>
        /// The initialize method is responsible for restoring the world and state.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        protected async override Task Load()
        {
            //this.worlds = new List<IWorld>(await this.worldService.GetAllWorlds());

            if (!this.Worlds.Any())
            {
                // Handle
            }
            else
            {
                List<Task> worldTasks = new List<Task>();
                foreach (IWorld world in this.Worlds)
                {
                    Task worldTask = this.OnWorldLoaded(world).ContinueWith(task => world.Initialize());
                    worldTasks.Add(worldTask);
                }

                await Task.WhenAll(worldTasks)
                    .ContinueWith(task =>
                    {
                        this.IsRunning = true;
                        this.Autosave.Initialize();
                    });
            }
        }

        /// <summary>
        /// Called when the game is deleted.
        /// Handles clean up of the autosave timer, saving the game state and cleaning up objects.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        protected override async Task Unload()
        {
            await this.Autosave.Delete();

            var worldsToSave = this.Worlds.ToArray();
            await this.SaveWorlds();

            foreach (var world in worldsToSave)
            {
                // Let the world perform clean up and notify it's subscribers it is going away.
                // world.Delete();
                this.worlds.Remove(world);
            }
        }

        /// <summary>
        /// Occurs when a world is loaded, prior to initialization of the world.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <returns>Returns an awaitable Task</returns>
        protected virtual Task OnWorldLoaded(IWorld world)
        {
            var handler = this.WorldLoaded;
            if (handler == null)
            {
                return Task.FromResult(0);
            }

            return handler(this, new WorldLoadedArgs(world));
        }

        /// <summary>
        /// Saves each World within the worlds collecton.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        private async Task SaveWorlds()
        {
            List<Task> runningTasks = new List<Task>();
            foreach (IWorld world in this.Worlds)
            {
                //runningTasks.Add(this.worldService.SaveWorld(world));
            }

            await Task.WhenAll(runningTasks);
        }

        public Task AddWorld(IWorld world)
        {
            if (this.worlds.Contains(world))
            {
                return Task.FromResult(0);
            }

            this.worlds.Add(world);
            return world.Initialize();
        }
    }
}
