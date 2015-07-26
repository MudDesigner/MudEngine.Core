//-----------------------------------------------------------------------
// <copyright file="DefaultWorld.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The Default World class used by the engine.
    /// </summary>
    public class DefaultWorld : GameComponent, IWorld
    {
        /// <summary>
        /// The time of day states
        /// </summary>
        private List<TimeOfDayState> timeOfDayStates = new List<TimeOfDayState>();

        /// <summary>
        /// The realms
        /// </summary>
        private List<DefaultRealm> realms = new List<DefaultRealm>();

        /// <summary>
        /// The time of day state manager
        /// </summary>
        private TimeOfDayStateManager timeOfDayStateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWorld"/> class.
        /// </summary>
        public DefaultWorld()
        {
            this.CreationDate = DateTime.Now;

            // Set the in-game day to be 24 hours, with each day taking 45 real-world minutes to cycle.
            this.HoursPerDay = 24;
            this.GameDayToRealHourRatio = 0.75;

            // Set up default states for the time of day.
            this.timeOfDayStates = new List<TimeOfDayState>();
            this.timeOfDayStateManager = new TimeOfDayStateManager(this.timeOfDayStates);

            // Must be in the constructor. If assigned within the Initialization method
            // the property could potentially never be restored properly from the data store.
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Occurs when [time of day changed].
        /// </summary>
        public event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

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
        /// Gets or sets the current time of day.
        /// </summary>
        public TimeOfDayState CurrentTimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets a collection of states that can be used for the time of day.
        /// </summary>
        public TimeOfDayState[] TimeOfDayStates
        {
            get
            {
                return this.timeOfDayStates.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets hour many hours it takes in-game to complete 1 day.
        /// </summary>
        public int HoursPerDay { get; set; }

        /// <summary>
        /// Gets or sets the hours ratio. If set to 4, it takes 4 in-game hours to equal 1 real-world hour.
        /// </summary>
        public double GameDayToRealHourRatio { get; set; }

        /// <summary>
        /// Gets the game time ratio used to convert real-world time to game-time.
        /// </summary>
        public double GameTimeAdjustmentFactor
        {
            get
            {
                return this.GameDayToRealHourRatio / this.HoursPerDay;
            }
        }

        /// <summary>
        /// Gets or sets the realms in this world.
        /// </summary>
        public DefaultRealm[] Realms
        {
            get
            {
                // Do not return a reference to our private list.
                // Instead, create a new List and return that. In the event someone
                // casts the IReadOnlyCollection to List<T>, that don't modify our private collection.
                return this.realms.ToArray();
            }
        }

        /// <summary>
        /// Gets the number of realms.
        /// </summary>
        public int NumberOfRealms
        {
            get
            {
                return this.realms.Count;
            }
        }


        /// <summary>
        /// Adds the given realm to world and initializes it.
        /// </summary>
        /// <param name="realm">The realm.</param>
        /// <exception cref="System.NullReferenceException">Attempted to add a null Realm to the world.
        /// or
        /// Adding a Realm to a World with a null Zones collection is not allowed.</exception>
        public Task AddRealmToWorld(DefaultRealm realm)
        {
            if (realm == null)
            {
                throw new NullReferenceException("Attempted to add a null Realm to the world.");
            }

            if (realm.Zones == null)
            {
                throw new NullReferenceException("Adding a Realm to a World with a null Zones collection is not allowed.");
            }

            // We don't attempt to add duplicates.
            if (this.Realms.Contains(realm))
            {
                return Task.FromResult(0);
            }

            this.realms.Add(realm);
            return realm.Initialize();
        }

        /// <summary>
        /// Adds a collection of realms to world.
        /// Each Realm is initialized upon being added to the World.
        /// </summary>
        /// <param name="realm">The realms collection.</param>
        public Task AddRealmsToWorld(IEnumerable<DefaultRealm> realms)
        {
            List<Task> realmTasks = new List<Task>();
            foreach(DefaultRealm realm in realms.AsParallel())
            {
                realmTasks.Add(this.AddRealmToWorld(realm));
            }

            return Task.WhenAll(realmTasks);
        }

        /// <summary>
        /// Removes the realm from world.
        /// </summary>
        /// <param name="realm">The realm.</param>
        public void RemoveRealmFromWorld(DefaultRealm realm)
        {
            if (!this.Realms.Contains(realm))
            {
                return;
            }

            this.realms.Remove(realm);
        }

        /// <summary>
        /// Adds the time of day state to the world.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="System.InvalidOperationException">You can not have two states with the same start time in the same world.</exception>
        /// <exception cref="System.NullReferenceException">State's start time must not be null.</exception>
        public void AddTimeOfDayState(TimeOfDayState state)
        {
            // We prevent states with the same start time from being added to the world
            if (this.TimeOfDayStates.Any(s => s.StateStartTime.Equals(state)))
            {
                throw new InvalidOperationException("You can not have two states with the same start time in the same world.");
            }

            // Null start times engine from transitioning to the new state.
            if (state.StateStartTime == null)
            {
                throw new NullReferenceException("State's start time must not be null.");
            }

            this.timeOfDayStates.Add(state);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} - {1} - with {2} realms.", this.Name, this.CurrentTimeOfDay.Name, this.NumberOfRealms);
        }

        /// <summary>
        /// Initializes the world by starting the world clock and the associated Realm clocks.
        /// The world must have all of its associated realms assigned before invoking Initialize.
        /// </summary>
        /// <param name="initialState">The optional initial state that the world must start with</param>
        protected override Task Load()
        {
            // If we do not have an initial state, then we create a state based on our current real-world time.
            var initialState = this.timeOfDayStateManager.GetTimeOfDayState(DateTime.Now);

            // If none exists within our current real-world time, then just grab the first state and assign it.
            if (initialState == null)
            {
                initialState = this.TimeOfDayStates
                    .OrderBy(s => s.StateStartTime.Hour)
                    .ThenBy(s => s.StateStartTime.Minute)
                    .FirstOrDefault();

                ExceptionFactory.ThrowIf<InvalidOperationException>(initialState == null, "No time of day states added to the world.");
            }

            // If an initial state is provided, then we hand it off to the setup method.
            this.SetupWorldClock(initialState);

            // Notify listeners that our time of day has changed.
            this.OnTimeOfDayChanged(null, this.CurrentTimeOfDay);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override Task Unload()
        {
            // These should all have their clocks disabled, but we ensure they are anyway.
            // This will also pick up our Current state during the process.
            foreach (TimeOfDayState state in this.TimeOfDayStates)
            {
                state.Reset();
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Called when the current time of day has changed and a new one is transitioning in.
        /// </summary>
        /// <param name="oldTimeOfDay">The old time of day.</param>
        /// <param name="newTimeOfDay">The new time of day.</param>
        protected virtual void OnTimeOfDayChanged(TimeOfDayState oldTimeOfDay, TimeOfDayState newTimeOfDay)
        {
            // Our event handler
            EventHandler<TimeOfDayChangedEventArgs> handler = this.TimeOfDayChanged;
            if (handler == null)
            {
                return;
            }

            // Invoke the handler
            handler(this.CurrentTimeOfDay, new TimeOfDayChangedEventArgs(oldTimeOfDay, newTimeOfDay));
        }

        /// <summary>
        /// Sets up the world clock.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        private void SetupWorldClock(TimeOfDayState initialState)
        {
            // We want to reset our current state before we set up the next state
            // The next state starts on a background thread and can cause listeners to access
            // the old state before the new state is assigned preventing a proper reset.
            if (this.CurrentTimeOfDay != null)
            {
                this.CurrentTimeOfDay.TimeUpdated -= this.CurrentTimeOfDayState_TimeUpdated;
            }

            // Register for event updates
            initialState.TimeUpdated += this.CurrentTimeOfDayState_TimeUpdated;

            // Initialize the state. 
            // TODO: This could potentially reset an in-use state if a zone has a large time-zone offset. An exception should be thrown if still in-use.
            initialState.Initialize(this.GameTimeAdjustmentFactor, this.HoursPerDay);

            this.CurrentTimeOfDay = initialState;
        }

        /// <summary>
        /// Event handler method fired when the Current TimeOfDay state has its time changed.
        /// </summary>
        /// <param name="sender">The TimeOfDayState that caused the time change.</param>
        /// <param name="updatedTimeOfDay">The new time of day.</param>
        private void CurrentTimeOfDayState_TimeUpdated(object sender, TimeOfDay updatedTimeOfDay)
        {
            // Takes our current time of day within the time of day state, and updates the realms with it.
            this.UpdateTimeOfDayStatesForRealms();

            // Fetch the world time of day.
            // This acts as +0 GMT in-game. Each realm can have an offset via its own timezone.
            TimeOfDayState newTimeOfDay = this.timeOfDayStateManager.GetTimeOfDayState(updatedTimeOfDay);
            if (this.CurrentTimeOfDay == newTimeOfDay)
            {
                return;
            }

            // We need to store a reference to the current state, since the Setup method overwrites it.
            var currentTimeOfDay = this.CurrentTimeOfDay;
            this.SetupWorldClock(newTimeOfDay);

            // Since the world time of day state was changed, we need to re-evaluate our realms.
            this.UpdateTimeOfDayStatesForRealms();

            // Invoke our event handler with the old state and the new state.
            this.OnTimeOfDayChanged(currentTimeOfDay, newTimeOfDay);
        }

        /// <summary>
        /// Updates the time of day states for realms.
        /// </summary>
        private void UpdateTimeOfDayStatesForRealms()
        {
            this.Realms.AsParallel().ForAll(realm =>
            {
                realm.ApplyTimeZoneOffset(this.CurrentTimeOfDay.CurrentTime);
            });
        }
    }
}
