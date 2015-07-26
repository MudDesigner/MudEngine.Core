//-----------------------------------------------------------------------
// <copyright file="DefaultRealm.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;

    /// <summary>
    /// The Default Realm class for the engine.
    /// </summary>
    public class DefaultRealm : GameComponent
    {
        /// <summary>
        /// The time of day state manager
        /// </summary>
        private TimeOfDayStateManager timeOfDayStateManager;

        /// <summary>
        /// The collection of zones for this realm
        /// </summary>
        private List<DefaultZone> zones = new List<DefaultZone>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRealm"/> class.
        /// </summary>
        public DefaultRealm(IWorld world, TimeOfDay worldTimeOfDay)
        {
            ExceptionFactory
                .ThrowIf<ArgumentNullException>(world == null, "A valid world instance must be supplied.")
                .Or(worldTimeOfDay == null, "A valid TImeOfDay instance is required to initialize a realm.");

            this.World = world;
            this.timeOfDayStateManager = new TimeOfDayStateManager(world.TimeOfDayStates);
            this.ApplyTimeZoneOffset(worldTimeOfDay);

            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the offset from the World's current time for the Realm.
        /// </summary>
        public TimeOfDay TimeZoneOffset { get; set; }

        /// <summary>
        /// Gets or sets the time of day for the Realm.
        /// </summary>
        public TimeOfDay CurrentTimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the zones within this Realm.
        /// </summary>
        public IEnumerable<DefaultZone> Zones
        {
            get
            {
                return this.zones;
            }

            set
            {
                this.zones.Clear();

                if (value != null)
                {
                    this.zones.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Gets the number of zones.
        /// </summary>
        public int NumberOfZones
        {
            get
            {
                // This lets users not call the enumerator on the Zones property.
                return this.zones.Count;
            }
        }

        /// <summary>
        /// Gets or sets the World that owns this realm..
        /// </summary>
        public IWorld World { get; protected set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets how many seconds have passed since the creation date.
        /// </summary>
        public double TimeFromCreation
        {
            get
            {
                return this.CreationDate.Subtract(DateTime.Now).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Adds the given zone to this instance.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <exception cref="System.NullReferenceException">Attempted to add a null Zone to the Realm.
        /// or
        /// Adding a Zone to a Realm with a null Rooms collection is not allowed.</exception>
        public void AddZoneToRealm(DefaultZone zone)
        {
            if (zone == null)
            {
                throw new NullReferenceException("Attempted to add a null Zone to the Realm.");
            }

            if (zone.Rooms == null)
            {
                throw new NullReferenceException("Adding a Zone to a Realm with a null Rooms collection is not allowed.");
            }

            zone.Initialize(this);
            this.zones.Add(zone);
        }

        /// <summary>
        /// Updates the time for this realm, applying the realm's time zone offset to the given time.
        /// </summary>
        /// <param name="timeOfDay">The time of day.</param>
        /// <exception cref="System.NullReferenceException">
        /// ApplyTimeZoneOffset can not be given a null argument.
        /// or
        /// A Time Zone offset can not be applied when both the TimeZoneOffset and World properties are null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">You can not have a negative time-zone for realms. They must all be forward offsets from the world's current time.</exception>
        public void ApplyTimeZoneOffset(TimeOfDay timeOfDay)
        {
            if (timeOfDay == null)
            {
                throw new NullReferenceException("ApplyTimeZoneOffset can not be given a null argument.");
            }

            if (this.TimeZoneOffset == null)
            {
                if (this.World == null)
                {
                    throw new NullReferenceException("A Time Zone offset can not be applied when both the TimeZoneOffset and World properties are null.");
                }

                this.TimeZoneOffset = new TimeOfDay { Hour = 0, Minute = 0, HoursPerDay = this.World.HoursPerDay };
            }
            else if (this.TimeZoneOffset.Hour < 0 || this.TimeZoneOffset.Minute < 0)
            {
                throw new ArgumentOutOfRangeException("You can not have a negative time-zone for realms. They must all be forward offsets from the world's current time.");
            }

            if (this.CurrentTimeOfDay == null)
            {
                this.CurrentTimeOfDay = new TimeOfDay();
            }

            this.CurrentTimeOfDay.Hour = timeOfDay.Hour;
            this.CurrentTimeOfDay.Minute = timeOfDay.Minute;
            this.CurrentTimeOfDay.HoursPerDay = timeOfDay.HoursPerDay;

            this.CurrentTimeOfDay.DecrementByHour(this.TimeZoneOffset.Hour);
            this.CurrentTimeOfDay.DecrementByMinute(this.TimeZoneOffset.Minute);
        }

        /// <summary>
        /// Gets the state of the current time of day.
        /// </summary>
        /// <returns>Returns an instance representing the current time of day state.</returns>
        public TimeOfDayState GetCurrentTimeOfDayState()
        {
            TimeOfDayState state = this.timeOfDayStateManager.GetTimeOfDayState(this.CurrentTimeOfDay);

            if (state == null)
            {
                return this.World.CurrentTimeOfDay;
            }
            else
            {
                return state;
            }
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load()
        {
            // If we have a time zone offset to apply, then we apply it when ever the World's time is changed.
            if (this.TimeZoneOffset != null)
            {
                this.World.TimeOfDayChanged += this.WorldTimeChanged;
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload()
        {
            if (this.TimeZoneOffset != null)
            {
                this.World.TimeOfDayChanged -= this.WorldTimeChanged;
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets called when the owning world has its Time changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TimeOfDayChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void WorldTimeChanged(object sender, TimeOfDayChangedEventArgs e)
        {
            this.ApplyTimeZoneOffset(e.TransitioningTo.CurrentTime);
        }
    }
}
