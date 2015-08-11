//-----------------------------------------------------------------------
// <copyright file="IDoorwayFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating an instance of an IDoorway implementation.
    /// </summary>
    public interface IDoorwayFactory
    {
        /// <summary>
        /// Creates an uninitialized instance of a doorway, connected to a departing room.
        /// </summary>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="travelDirection">The direction need to travel in order to leave the departure room.</param>
        /// <returns>Returns an uninitialized doorway</returns>
        Task<IDoorway> CreateDoor(IRoom departureRoom, ITravelDirection travelDirection);

        /// <summary>
        /// Creates an uninitialized instance of a doorway, connected to a departing and arrival room.
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="travelDirection">The direction need to travel in order to leave the departure room.</param>
        /// <returns>Returns an uninitialized doorway</returns>
        Task<IDoorway> CreateDoor(IRoom arrivalRoom, IRoom departureRoom, ITravelDirection travelDirection);
    }
}
