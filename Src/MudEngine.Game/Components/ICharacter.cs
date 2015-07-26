//-----------------------------------------------------------------------
// <copyright file="ICharacter.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    using System;

    /// <summary>
    /// Defines what makes up a Character within the game.
    /// </summary>
    public interface ICharacter : IGameComponent
    {
        /// <summary>
        /// Occurs when the character changes rooms.
        /// </summary>
        event EventHandler<OccupancyChangedEventArgs> RoomChanged;

        /// <summary>
        /// Gets the game.
        /// </summary>
        IGame Game { get; }

        /// <summary>
        /// Gets or sets the current room that this character occupies.
        /// </summary>
        DefaultRoom CurrentRoom {get; }

        /// <summary>
        /// Gets or sets information that defines what the character is.
        /// </summary>
        ICharacterInformation Information { get; }

        ICommandManager CommandManager { get; }

        /// <summary>
        /// Moves this character to the given room going in the specified direction.
        /// </summary>
        /// <param name="directionEnteringFrom">The direction.</param>
        /// <param name="newRoom">The new room.</param>
        void Move(ITravelDirection directionEnteringFrom, DefaultRoom newRoom);

        void SetCharacterInformation(ICharacterInformation characterInformation);
    }
}