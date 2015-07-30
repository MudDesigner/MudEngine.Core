//-----------------------------------------------------------------------
// <copyright file="IDoorway.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for linking two rooms together via a doorway
    /// </summary>
    public interface IDoorway : IGameComponent
    {
        /// <summary>
        /// Gets the direction needed to travel in order to leave the DepartureRoom.
        /// </summary>
        ITravelDirection DepartureDirection { get; }

        /// <summary>
        /// Gets the room that an IActor would be departing from.
        /// </summary>
        IRoom DepartureRoom { get; }

        /// <summary>
        /// Gets the room that an IActor would be arriving into during travel.
        /// </summary>
        IRoom ArrivalRoom { get; }

        /// <summary>
        /// Connects the departing room to the arrival room.
        /// </summary>
        /// <para>
        /// This connection is one-way. Actors can not travel back to the departing room.
        /// </para>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task ConnectRooms(IRoom departureRoom, IRoom arrivalRoom);

        /// <summary>
        /// Connects the departing room to the arrival room.
        /// </summary>
        /// <para>
        /// This connection can be set to either one-way or two way, allowing or disallowing
        /// Actors the ability to travel back to the departing room.
        /// </para>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="autoCreateReverseDoorway">if set to <c>true</c> a doorway will be added to the arrival room so actors can travel back to the departing room.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task ConnectRooms(IRoom departureRoom, IRoom arrivalRoom, bool autoCreateReverseDoorway);

        /// <summary>
        /// Connects the departing room currently assigned to this doorway to the arrival room.
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task ConnectRooms(IRoom arrivalRoom);

        /// <summary>
        /// Connects the departing room currently assigned to this doorway to the arrival room.
        /// </summary>
        /// <para>
        /// This connection can be set to either one-way or two way, allowing or disallowing
        /// Actors the ability to travel back to the departing room.
        /// </para>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="autoCreateReverseDoorway">if set to <c>true</c> a doorway will be added to the arrival room so actors can travel back to the departing room.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task ConnectRooms(IRoom arrivalRoom, bool autoCreateReverseDoorway);

        /// <summary>
        /// Disconnects the two linked rooms from each other.
        /// </summary>
        void DisconnectRooms();
    }
}
