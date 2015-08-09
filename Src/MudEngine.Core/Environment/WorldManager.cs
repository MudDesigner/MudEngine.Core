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
    public sealed class WorldManager
    {
        /// <summary>
        /// Gets a collection of worlds associated with this game.
        /// </summary>
        IWorld[] Worlds { get; }

        /// <summary>
        /// Adds a given world to the games available worlds.
        /// </summary>
        /// <param name="world">The world to give the game.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddWorld(IWorld world)
        {
            throw new NotImplementedException("WorldManager.AddWorld(IWorld) is not implemented.");
        }

        /// <summary>
        /// Adds a collection of worlds to the game.
        /// </summary>
        /// <para>
        /// If a world already exists in the game, it is ignored.
        /// </para>
        /// <param name="worlds">The worlds collection to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddWorlds(IEnumerable<IWorld> worlds)
        {
            throw new NotImplementedException("WorldManager.AddWorlds(IEnumerable<IWorld>) is not implemented.");
        }
    }
}
