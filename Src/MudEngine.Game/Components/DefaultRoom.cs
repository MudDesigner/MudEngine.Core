//-----------------------------------------------------------------------
// <copyright file="DefaultRoom.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The Default engine Room Type.
    /// </summary>
    public class DefaultRoom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRoom"/> class.
        /// </summary>
        public DefaultRoom()
        {
            this.Doorways = new List<DefaultDoorway>();
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Occurs when a character enters the room.
        /// </summary>
        public event EventHandler<OccupancyChangedEventArgs> EnteredRoom;

        /// <summary>
        /// Occurs when a character leaves the room.
        /// </summary>
        public event EventHandler<OccupancyChangedEventArgs> LeftRoom;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets how many seconds have passed since the creation date.
        /// </summary>
        public double TimeFromCreation { get; protected set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the zone that owns this Room.
        /// </summary>
        public DefaultZone Zone { get; protected set; }

        /// <summary>
        /// Gets or sets the doorways that this Room has, linked to other IRooms.
        /// </summary>
        public ICollection<DefaultDoorway> Doorways { get; protected set; }

        /// <summary>
        /// Gets or sets the occupants within this Room..
        /// </summary>
        public ICollection<ICharacter> Occupants { get; protected set; }

        /// <summary>
        /// Initializes the room with the given zone.
        /// </summary>
        /// <param name="zone">The zone that represents the owner of this room.</param>
        public virtual void Initialize(DefaultZone zone)
        {
            this.Zone = zone;
        }

        /// <summary>
        /// Adds the occupant to this instance.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <exception cref="System.NullReferenceException">Attempted to add a null character to the Room.</exception>
        public void MoveOccupantToRoom(ICharacter character, DefaultRoom departingRoom)
        {
            // We don't allow the user to enter a disabled room.
            if (!this.IsEnabled)
            {
                // TODO: Need to do some kind of communication back to the caller that this can't be traveled to.
                throw new InvalidOperationException("The room is disabled and can not be traveled to.");
            }

            if (character == null)
            {
                throw new NullReferenceException("Attempted to add a null character to the Room.");
            }

            // Remove the character from their previous room.
            //Find the doorway that the character traveled through
            DefaultDoorway doorwayTraveledThrough = 
                departingRoom?.Doorways.FirstOrDefault(door => door.ArrivalRoom == this);

            // Get the opposite direction of the doorways travel direction. This will be the direction that they entered from
            // within this instance's available entry points.
            ITravelDirection enteredDirection = doorwayTraveledThrough?.DepartureDirection.GetOppositeDirection();

            // Handle removing the occupant from the previous room. The character might handle this for us
            // so our RemoveOccupantFromRoom implementation will try to remove safely.
            departingRoom?.TryRemoveOccupantFromRoom(character, doorwayTraveledThrough.DepartureDirection, this);

            this.Occupants.Add(character);
            character.Deleted += this.OnCharacterDeletingStarting;
            // Notify our event handles that the character has entered the room.
            this.OnEnteringRoom(character, enteredDirection, departingRoom);
            character.Move(enteredDirection, this);
        }

        private void OnCharacterDeletingStarting(object sender, EventArgs args)
        {
            var character = (ICharacter)sender;
            character.Deleted -= this.OnCharacterDeletingStarting;
            this.RemoveOccupantFromRoom(character);
        }

        public void RemoveOccupantFromRoom(ICharacter character)
        {
            this.Occupants.Remove(character);
            this.OnLeavingRoom(character, null, null);
        }

        /// <summary>
        /// Removes the occupant from room.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        protected void TryRemoveOccupantFromRoom(ICharacter character, ITravelDirection leavingDirection, DefaultRoom arrivalRoom)
        {
            if (character == null)
            {
                return;
            }

            this.Occupants.Remove(character);
            this.OnLeavingRoom(character, leavingDirection, arrivalRoom);
        }

        /// <summary>
        /// Called when a character enters this room.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="departingRoom">The departing room.</param>
        protected virtual void OnEnteringRoom(ICharacter character, ITravelDirection enteringDirection, DefaultRoom departingRoom)
        {
            EventHandler<OccupancyChangedEventArgs> handler = this.EnteredRoom;
            if (handler == null)
            {
                return;
            }

            handler(this, new OccupancyChangedEventArgs(character, enteringDirection, departingRoom, this));
        }

        /// <summary>
        /// Called when a character leaves this room.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="arrivalRoom">The arrival room.</param>
        protected virtual void OnLeavingRoom(ICharacter character, ITravelDirection enteringDirection, DefaultRoom arrivalRoom)
        {
            EventHandler<OccupancyChangedEventArgs> handler = this.LeftRoom;
            if (handler == null)
            {
                return;
            }

            handler(this, new OccupancyChangedEventArgs(character, enteringDirection, this, arrivalRoom));
        }
    }
}
