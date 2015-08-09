//-----------------------------------------------------------------------
// <copyright file="MudWorld.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudEngine.Game.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MudDesigner.MudEngine.Environment;
    using MudDesigner.MudEngine.Game;

    public class MudWorld : GameComponent, IWorld
    {
        private List<ITimePeriod> timePeriods;

        private TimePeriodManager timePeriodManager;

        private List<IRealm> realms;

        public MudWorld()
        {
            this.HoursPerDay = 24;
            this.GameDayToRealHourRatio = 0.75;

            this.realms = new List<IRealm>();
            this.timePeriods = new List<ITimePeriod>();
            this.timePeriodManager = new TimePeriodManager(this.timePeriods);
            this.Name = "Mud World";
        }

        public event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

        public ITimePeriod CurrentTimeOfDay { get; protected set; }

        public double GameDayToRealHourRatio { get; set; }

        public double GameTimeAdjustmentFactor
        {
            get
            {
                return this.GameDayToRealHourRatio / this.HoursPerDay;
            }
        }

        public int HoursPerDay { get; protected set; }

        /// <summary>
        /// Adds a collection of realms to world, initializing them as they are added.
        /// </summary>
        /// <param name="realms">The realms.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public async Task AddRealmsToWorld(IEnumerable<IRealm> realms)
        {
            if (realms == null)
            {
                return;
            }

            foreach(IRealm realm in realms)
            {
                await this.AddRealmToWorld(realm);
            }
        }

        /// <summary>
        /// Initializes and then adds the given realm to this world instance.
        /// </summary>
        /// <param name="realm">The realm to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        /// <exception cref="MudDesigner.MudEngine.Environment.InvalidRealmException">The realm name can not be null or blank.</exception>
        public async Task AddRealmToWorld(IRealm realm)
        {
            if (this.realms.Contains(realm))
            {
                return;
            }

            if (string.IsNullOrEmpty(realm.Name))
            {
                throw new InvalidRealmException(realm, "The realm name can not be null or blank.");
            }

            realm.Owner = this;
            await realm.Initialize();
            this.realms.Add(realm);
        }

        /// <summary>
        /// Adds the time period to world.
        /// </summary>
        /// <param name="timePeriod">The time period.</param>
        /// <exception cref="MudDesigner.MudEngine.Environment.InvalidTimePeriodException">
        /// The time period must be given a name prior to adding it to the world.
        /// or
        /// The time period must have a starting state time.
        /// </exception>
        /// <exception cref="InvalidTimeOfDayException">The time of day does not define the number of hours there are in a day.</exception>
        public void AddTimePeriodToWorld(ITimePeriod timePeriod)
        {
            if (this.timePeriods.Contains(timePeriod))
            {
                return;
            }

            if (string.IsNullOrEmpty(timePeriod.Name))
            {
                throw new InvalidTimePeriodException(timePeriod, "The time period must be given a name prior to adding it to the world.");
            }
            else if (timePeriod.StateStartTime == null)
            {
                throw new InvalidTimePeriodException(timePeriod, "The time period must have a starting state time.");
            }
            else if (timePeriod.StateStartTime.HoursPerDay == 0)
            {
                throw new InvalidTimeOfDayException("The time of day does not define the number of hours there are in a day.", timePeriod.StateStartTime);
            }
            
            this.timePeriods.Add(timePeriod);
        }

        /// <summary>
        /// Gets the available time periods for this world.
        /// </summary>
        /// <returns>Returns an array of time periods</returns>
        public ITimePeriod[] GetTimePeriodsForWorld()
        {
            return this.timePeriods.ToArray();
        }

        /// <summary>
        /// Gets all of the realms that have been added to this world.
        /// </summary>
        /// <returns>Returns an array of realms</returns>
        public IRealm[] GetRealmsInWorld()
        {
            return this.realms.ToArray();
        }

        /// <summary>
        /// Removes the given realm from this world instance, deleting the realm in the process.
        /// If it must be reused, you may clone the realm and add the clone to another world.
        /// </summary>
        /// <param name="realm">The realm to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveRealmFromWorld(IRealm realm)
        {
            if (realm == null)
            {
                return Task.FromResult(0);
            }

            if (!this.realms.Contains(realm))
            {
                return Task.FromResult(0);
            }

            this.realms.Remove(realm);
            return realm.Delete();
        }

        /// <summary>
        /// Removes a collection of realms from this world instance.
        /// If any of the realms don't exist in the world, they will be ignored.
        /// The realms will be deleted during the process.
        /// If they must be reused, you may clone the realm and add the clone to another world.
        /// </summary>
        /// <param name="realms">The realms collection.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task RemoveRealmsFromWorld(IEnumerable<IRealm> realms)
        {
            if (realms == null)
            {
                return;
            }

            foreach(IRealm realm in realms)
            {
                await this.RemoveRealmFromWorld(realm);
            }
        }

        /// <summary>
        /// Removes a time period from the world.
        /// </summary>
        /// <param name="timePeriod">The time period being removed.</param>
        public void RemoveTimePeriodFromWorld(ITimePeriod timePeriod)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets how many hours it takes the world to complete a full day.
        /// </summary>
        /// <param name="hours">The number of hours that defines how long a full day is.</param>
        public void SetHoursPerDay(int hours)
        {
            this.HoursPerDay = hours;
            foreach(ITimePeriod period in this.timePeriods)
            {
                period.StateStartTime.SetHoursPerDay(hours);
            }
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
        public IWorld Clone()
        {
            var clone = new MudWorld()
            {
                Id = this.Id,
                CurrentTimeOfDay = this.CurrentTimeOfDay,
                GameDayToRealHourRatio = this.GameDayToRealHourRatio,
                HoursPerDay = this.HoursPerDay,
                IsEnabled = this.IsEnabled,
                Name = this.Name,
                realms = this.realms,
                timePeriods = this.timePeriods,
            };

            clone.timePeriodManager = new TimePeriodManager(this.timePeriods);
            return clone;
        }

        protected override Task Load()
        {
            throw new NotImplementedException();
        }

        protected override Task Unload()
        {
            throw new NotImplementedException();
        }
    }
}
