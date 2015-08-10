using System;
using System.Collections.Generic;
//-----------------------------------------------------------------------
// <copyright file="MudRealm.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudEngine.Game.Environment
{
    using System.Threading.Tasks;
    using MudDesigner.MudEngine;
    using MudDesigner.MudEngine.Environment;

    /// <summary>
    /// Provides properties and methods for defining and maintaining Realms
    /// </summary>
    public class MudRealm : GameComponent, IRealm, ICloneableComponent<IRealm>
    {
        private List<IZone> zones;

        public MudRealm()
        {
            this.zones = new List<IZone>();
        }

        public MudRealm(IWorld world) : this()
        {
            this.Owner = world;
        }

        /// <summary>
        /// Gets the time of day for the Realms time zone.
        /// </summary>
        public ITimeOfDay CurrentTime
        {
            get
            {
                if (this.Owner == null)
                {
                    return null;
                }

                // Fetch the normalized world time and apply our new timezne offset to it.
                ITimeOfDay adjustedTimeOfDay = this.Owner.CurrentTimeOfDay.CurrentTime;
                adjustedTimeOfDay.IncrementByMinute(this.TimeZoneOffset.Minute);
                adjustedTimeOfDay.IncrementByHour(this.TimeZoneOffset.Hour);

                return adjustedTimeOfDay;
            }
        }

        /// <summary>
        /// Gets the number of zones in realm.
        /// </summary>]
        public int NumberOfZonesInRealm { get { return this.zones.Count; } }

        /// <summary>
        /// Gets the World that owns this Realm.
        /// </summary>
        public IWorld Owner { get; set; }

        /// <summary>
        /// Gets or sets the offset from the World's current time for the Realm.
        /// </summary>
        public ITimeOfDay TimeZoneOffset { get; protected set; }

        /// <summary>
        /// Adds a collection of zones to the realm.
        /// </summary>
        /// <param name="zones">The collection of zones to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        /// <exception cref="System.ArgumentNullException">You must not provide a null collection when adding zones to a realm.</exception>
        public async Task AddZonesToRealm(IEnumerable<IZone> zones)
        {
            if (zones == null)
            {
                throw new ArgumentNullException(nameof(zones), "You must not provide a null collection when adding zones to a realm.");
            }

            foreach(IZone zone in zones)
            {
                await this.AddZoneToRealm(zone);
            }
        }

        /// <summary>
        /// Adds a given zone to this Realm.
        /// </summary>
        /// <param name="zone">The zone to add to the realm.</param>
        /// <returns>Returns an awaitable Task</returns>
        /// <exception cref="InvalidZoneException">A zone can not be added with an empty name.</exception>
        public async Task AddZoneToRealm(IZone zone)
        {
            if (this.zones.Contains(zone))
            {
                return;
            }

            if (string.IsNullOrEmpty(zone.Name))
            {
                throw new InvalidZoneException(zone, "A zone can not be added with an empty name.");
            }

            zone.Owner = this;
            await zone.Initialize();
            this.zones.Add(zone);
        }

        /// <summary>
        /// Applies a time zone offset to the Realm.
        /// </summary>
        /// <para>
        /// The Hour and Minute provided will cause the Realm's timezone to be offset from
        /// the Worlds standard time-zone.
        /// </para>
        /// <param name="hour">The number of hours to offset by.</param>
        /// <param name="minute">The number of minutes to offset by.</param>
        public void ApplyTimeZoneOffset(int hour, int minute)
        {
            if (this.Owner == null)
            {
                throw new InvalidTimeZoneException("The time zone offset can not be applied due to the realm not being owned by a world. Worlds manage the in-game time and time periods. A world must be assigned as owner on the realm in order for any time zone interactions to take place.");
            }

            TimePeriodManager timeManager = this.Owner.TimePeriodManager;
            this.TimeZoneOffset = timeManager.CreateTimeOfDay(hour, minute, this.Owner.HoursPerDay);
        }

        /// <summary>
        /// Clones the properties of this instance to a new instance.
        /// </summary>
        /// <returns>
        /// Returns a new instance with the properties of this instance copied to it.
        /// </returns>
        /// <para>
        /// Cloning does not guarantee that the internal state of an object will be cloned nor
        /// does it guarantee that the clone will be a deep clone or a shallow.
        /// </para>
        public IRealm Clone()
        {
            var clone = new MudRealm(this.Owner)
            {
                Id = this.Id,
                IsEnabled = this.IsEnabled,
                Name = this.Name,
                Owner = this.Owner,
                TimeZoneOffset = this.TimeZoneOffset,
                zones = this.zones,
            };
            return clone;
        }

        /// <summary>
        /// Gets the state of the current time of day.
        /// </summary>
        /// <returns>Returns the current Time of Day State the Realm is in.</returns>
        public ITimePeriod GetCurrentTimePeriodForZone() => this.Owner.TimePeriodManager.GetTimePeriodForDay(this.CurrentTime);

        /// <summary>
        /// Gets the all of the zones assigned to the realm.
        /// </summary>
        /// <returns>Returns an array of each Zone contained in the Realm.</returns>
        public IZone[] GetZonesInRealm() => this.zones.ToArray();

        /// <summary>
        /// Determines whether this Realm contains the given zone.
        /// </summary>
        /// <param name="zone">The zone to lookup.</param>
        /// <returns>Returns true if the Zone exists in the Realm.</returns>
        public bool HasZoneInRealm(IZone zone) => this.zones.Contains(zone);

        /// <summary>
        /// Loads the component and any resources or dependencies it might have. 
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load()
        {
            // The ApplyTimeZoneOffset throws an exception if Owner is null, so we check for that first.
            if (this.Owner != null)
            {
                this.ApplyTimeZoneOffset(0, 0);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload()
        {
            return Task.FromResult(0);
        }
    }
}
