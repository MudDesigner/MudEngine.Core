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
        private List<IWorld> worlds = new List<IWorld>();

        /// <summary>
        /// Gets information pertaining to the game.
        /// </summary>
        public IGameConfiguration Configuration { get; protected set; }

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

        public Task AddWorld(IWorld world)
        {
            if (this.worlds.Contains(world))
            {
                return Task.FromResult(0);
            }

            this.worlds.Add(world);
            return world.Initialize();
        }

        public async Task AddWorlds(IEnumerable<IWorld> worlds)
        {
            foreach(IWorld world in worlds)
            {
                await this.AddWorld(world);
            }
        }

        protected override Task Load()
        {
            return Task.FromResult(0);
        }

        protected override Task Unload()
        {
            return Task.FromResult(0);
        }
    }
}
