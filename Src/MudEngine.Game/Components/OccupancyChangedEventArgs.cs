//-----------------------------------------------------------------------
// <copyright file="OccupancyChangedEventArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    using MudDesigner.MudEngine.Game.Components;
    using System;

    /// <summary>
    /// Event arguments for when a rooms occupancy status changes.
    /// </summary>
    public class OccupancyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OccupancyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="occupant">The occupant.</param>
        /// <param name="departureRoom">The departure room.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        public OccupancyChangedEventArgs(ICharacter occupant, ITravelDirection travelDirection, DefaultRoom departureRoom, DefaultRoom arrivalRoom)
        {
            this.Occupant = occupant;
            this.DepartureRoom = departureRoom;
            this.ArrivalRoom = arrivalRoom;
            this.TravelDirection = travelDirection;
        }

        /// <summary>
        /// Gets the occupant that triggered this event.
        /// </summary>
        public ICharacter Occupant { get; private set; }

        /// <summary>
        /// Gets the direction that the occupant traveled when leaving the departure room.
        /// </summary>
        public ITravelDirection TravelDirection { get; private set; }

        /// <summary>
        /// Gets the departure room.
        /// </summary>
        public DefaultRoom DepartureRoom { get; private set; }

        /// <summary>
        /// Gets the arrival room.
        /// </summary>
        public DefaultRoom ArrivalRoom { get; private set; }
    }
}
