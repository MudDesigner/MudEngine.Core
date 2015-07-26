//-----------------------------------------------------------------------
// <copyright file="WorldLoadedArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game
{
    using System;
    using Components;

    /// <summary>
    /// Provides a reference to a World that was loaded.
    /// </summary>
    public class WorldLoadedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLoadedArgs"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        public WorldLoadedArgs(IWorld world)
        {
            this.World = world;
        }

        /// <summary>
        /// Gets the world that was just loaded.
        /// </summary>
        public IWorld World { get; private set; }
    }
}
