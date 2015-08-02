using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Environment
{
    public sealed class OccupancyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OccupancyChangedEventArgs" /> class.
        /// </summary>
        /// <param name="occupant">The occupant.</param>
        /// <param name="travelDirection">The travel direction.</param>
        /// <param name="departureRoom">The departure room.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        /// <exception cref="System.ArgumentNullException">
        /// A valid Occupant must be provided.
        /// or
        /// A valid travelDirection must be provided.
        /// or
        /// A valid departureRoom must be provided.
        /// or
        /// A valid arrivalRoom must be provided.
        /// </exception>
        public OccupancyChangedEventArgs(ICharacter occupant, ITravelDirection travelDirection, IRoom departureRoom, IRoom arrivalRoom)
        {
            if (occupant == null)
            {
                throw new ArgumentNullException(nameof(occupant), "A valid Occupant must be provided.");
            }
            else if (travelDirection == null)
            {
                throw new ArgumentNullException(nameof(travelDirection), "A valid travelDirection must be provided.");
            }
            else if (departureRoom == null)
            {
                throw new ArgumentNullException(nameof(departureRoom), "A valid departureRoom must be provided.");
            }
            else if (arrivalRoom == null)
            {
                throw new ArgumentNullException(nameof(arrivalRoom), "A valid arrivalRoom must be provided.");
            }

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
        public IRoom DepartureRoom { get; private set; }

        /// <summary>
        /// Gets the arrival room.
        /// </summary>
        public IRoom ArrivalRoom { get; private set; }
    }
}
