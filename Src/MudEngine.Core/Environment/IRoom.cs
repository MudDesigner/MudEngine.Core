//-----------------------------------------------------------------------
// <copyright file="IRealm.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MudDesigner.MudEngine.Actors;

    /// <summary>
    /// Provides Properties and Methods for creating and maintaining rooms.
    /// </summary>
    public interface IRoom : IGameComponent
    {
        /// <summary>
        /// Occurs when an occupant enters the room. The occupant must enter the room through via the AddActorToRoom or AddActorsToRoom methods
        /// </summary>
        event EventHandler<OccupancyChangedEventArgs> OccupantEntered;

        /// <summary>
        /// Occurs when an occupant leaves the room. The occupant must leave the room through via the RemoveActorFromRoom or RemoveActorsFromRoom methods
        /// </summary>
        event EventHandler<OccupancyChangedEventArgs> OccupantLeft;

        /// <summary>
        /// Gets Zone that owns this Room.
        /// </summary>
        IZone Owner { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is sealed. A sealed room will throw an exception if anything tries to add or remove an actor.
        /// A sealed room prevent actors from entering or leaving it.
        /// </summary>
        bool IsSealed { get; }

        /// <summary>
        /// Gets all of the doorways that this room has connected to it.
        /// </summary>
        /// <returns>Returns an array of doorways</returns>
        IDoorway[] GetDoorwaysForRoom();

        /// <summary>
        /// Gets all of the actors that this room has occupying it.
        /// </summary>
        /// <returns>Returns an array of Actors</returns>
        IActor[] GetActorsInRoom();

        /// <summary>
        /// Gets a subset of all actors in the room that are ICharacter instances only.
        /// </summary>
        /// <returns>Returns an array of Characters</returns>
        ICharacter[] GetCharactersInRoom();

        Task AddActorToRoom(IActor actor);

        Task AddActorsToRoom(IEnumerable<IActor> actors);

        Task RemoveActorFromRoom(IActor actor);

        Task RemoveActorsFromRoom(IEnumerable<IActor> actors);

        IDoorway CreateDoorway(ITravelDirection travelDirectionToReachDoor);

        Task AddDoorwayToRoom(IDoorway doorway);

        Task RemoveDoorwayFromRoom(IDoorway doorway);

        Task RemoveDoorwayFromRoom(ITravelDirection travelDirection);

        /// <summary>
        /// Seals the room so that no other actors may enter and any existing IActors may not leave.
        /// </summary>
        void SealRoom();

        void UnsealRoom();
    }
}
