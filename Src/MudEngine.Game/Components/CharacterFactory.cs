//-----------------------------------------------------------------------
// <copyright file="CharacterFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    using System;

    /// <summary>
    /// A Factory used to create Character types.
    /// </summary>
    public static class CharacterFactory
    {
        /// <summary>
        /// The player type to instance
        /// </summary>
        private static Func<IGame, IPlayer> factoryDelegate;

        /// <summary>
        /// Creates a new player instance.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public static IPlayer CreatePlayer(IGame game)
        {
            IPlayer player = null;
            if (factoryDelegate == null)
            {
                ICommandManager commandManager = CommandManagerFactory.CreateManager();
                player = new DefaultPlayer(game, commandManager);
            }
            else
            {
                player = factoryDelegate(game);
            }

            // This is a fail-safe. The DefaultPlayer assigns the owner for us, but we 
            // force ownership of the command manager on the player regardless. In the event
            // a different IPlayer implementation is used that forgets to do this, the engine is still usable.
            player.CommandManager.SetOwner(player);
            return player;
        }

        /// <summary>
        /// Sets the factory method used to create a new player.
        /// </summary>
        /// <param name="factory"></param>
        public static void SetFactory(Func<IGame, IPlayer> factory)
        {
            factoryDelegate = factory;
        }
    }
}
