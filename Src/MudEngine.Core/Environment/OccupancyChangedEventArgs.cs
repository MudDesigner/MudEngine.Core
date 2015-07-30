using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Environment
{
    public class OccupancyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OccupancyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="occupant">The occupant.</param>
        /// <param name="departureRoom">The departure room.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        public OccupancyChangedEventArgs(/*ICharacter occupant, */ITravelDirection travelDirection, IRoom departureRoom, IRoom arrivalRoom)
        {
            // this.Occupant = occupant;
            this.DepartureRoom = departureRoom;
            this.ArrivalRoom = arrivalRoom;
            this.TravelDirection = travelDirection;
        }

        /// <summary>
        /// Gets the occupant that triggered this event.
        /// </summary>
        //public ICharacter Occupant { get; private set; }

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
