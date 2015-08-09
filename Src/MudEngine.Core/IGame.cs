//-----------------------------------------------------------------------
// <copyright file="IGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System.Threading.Tasks;

    /// <summary>
    /// Exposes properties and methods for managing the current game
    /// </summary>
    public interface IGame : IGameComponent
    {
        /// <summary>
        /// Gets a value indicating whether the game is currently running.
        /// </summary>
        /// <para>
        /// If false, it is possible that all of the objects are disabled or destroyed.
        /// </para>
        bool IsRunning { get; }

        /// <summary>
        /// Configures the game using the provided game configuration.
        /// </summary>
        /// <param name="config">The configuration the game should use.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task Configure(IGameConfiguration config);
    }
}
