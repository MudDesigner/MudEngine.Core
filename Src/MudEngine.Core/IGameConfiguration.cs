//-----------------------------------------------------------------------
// <copyright file="IGameConfiguration.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System;

    /// <summary>
    /// Provides properties that are used to define a game
    /// </summary>
    public interface IGameConfiguration
    {
        /// <summary>
        /// Gets or sets the description of the game.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the game.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        Version Version { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        string Website { get; set; }

        /// <summary>
        /// Gets the game adapter components that have been registered.
        /// </summary>
        /// <returns>Returns an array of adapter components</returns>
        IAdapter[] GetAdapters();

        /// <summary>
        /// Tells the game configuration that a specific adapter component must be used by the game.
        /// A new instance of TConfigComponent will be created when the game starts.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter component to use.</typeparam>
        void UseAdapter<TAdapter>() where TAdapter : class, IAdapter, new();

        /// <summary>
        /// Tells the game configuration that a specific adapter component must be used by the game.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter component.</typeparam>
        /// <param name="component">The component instance you want to use.</param>
        void UseAdapter<TAdapter>(TAdapter component) where TAdapter : class, IAdapter;
    }
}