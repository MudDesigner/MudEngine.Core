//-----------------------------------------------------------------------
// <copyright file="GameInformation.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides meta-information for the currently running game.
    /// </summary>
    public class GameConfiguration : IGameConfiguration
    {
        private List<IAdapter> components;

        public GameConfiguration()
        {
            this.Version = new Version("1.0.0.0");
            this.components = new List<IAdapter>();
        }

        /// <summary>
        /// Gets or sets the name of the game being played.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the game.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the current version of the game.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the website that users can visit to get information on the game.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets the configuration components.
        /// </summary>
        /// <returns></returns>
        public IAdapter[] GetConfigurationComponents()
        {
            return this.components.ToArray();
        }

        /// <summary>
        /// Tells the game configuration that a specific component must be used by the game.
        /// A new instance of TConfigComponent will be created when the game starts.
        /// </summary>
        /// <typeparam name="TConfigComponent">The type of the configuration component to use.</typeparam>
        public void UseGameComponent<TConfigComponent>() where TConfigComponent : class, IAdapter, new()
        {
            this.components.Add(new TConfigComponent());
        }

        /// <summary>
        /// Tells the game configuration that a specific component must be used by the game.
        /// </summary>
        /// <typeparam name="TConfigComponent">The type of the configuration component.</typeparam>
        /// <param name="component">The component instance you want to use.</param>
        /// <exception cref="System.ArgumentNullException">$The configuration component provided of Type {component.GetType().Name} was null.</exception>
        public void UseGameComponent<TConfigComponent>(TConfigComponent component) where TConfigComponent : class, IAdapter
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component), $"The configuration component provided of Type {component.GetType().Name} was null.");
            }

            this.components.Add(component);
        }
    }
}
