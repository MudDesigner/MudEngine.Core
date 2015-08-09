//-----------------------------------------------------------------------
// <copyright file="WorldManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating and maintaining the worlds in the game.
    /// </summary>
    public sealed class WorldManager : AdapterBase
    {
        private List<IWorld> worlds;

        /// <summary>
        /// Gets a collection of worlds associated with this game.
        /// </summary>
        public IWorld[] Worlds { get; }

        public override string Name { get { return "World Manager"; } }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        /// <returns>Returns an awaitable Task</returns>
        public override Task Delete()
        {
            this.Dispose();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public override Task Initialize()
        {
            if (this.worlds == null)
            {
                this.worlds = new List<IWorld>();
            }

            return Task.FromResult(0);
        }
        
        /// <summary>
        /// Adds a given world to the games available worlds.
        /// </summary>
        /// <param name="world">The world to give the game.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task AddWorld(IWorld world)
        {
            if (this.worlds.Contains(world))
            {
                return;
            }


            if (System.Math.Abs(world.GameDayToRealHourRatio - default(double)) <= 0.000)
            {
                var exception = new InvalidOperationException("You assign the ratio between an in-game day to a real-world hour.");
                exception.Data.Add(this, world);
                throw exception;
            }

            if (world.HoursPerDay == 0)
            {
                var exception = new InvalidOperationException("You must define how many hours it takes to make up a single day in the world.");
                exception.Data.Add(this, world);
                throw exception;
            }

            await world.Initialize();
            this.worlds.Add(world);
        }

        /// <summary>
        /// Adds a collection of worlds to the game.
        /// </summary>
        /// <para>
        /// If a world already exists in the game, it is ignored.
        /// </para>
        /// <param name="worlds">The worlds collection to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task AddWorlds(IEnumerable<IWorld> worlds)
        {
            foreach(IWorld world in worlds)
            {
                await this.AddWorld(world);
            }
        }
    }
}
