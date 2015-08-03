//-----------------------------------------------------------------------
// <copyright file="TimeOfDayState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System;

    /// <summary>
    /// ITimeOfDayState implementation that handles starting the state 
    /// clock and provides methods for resetting and disposing.
    /// </summary>
    public sealed class TimeOfDayState : ITimeOfDayState, IDisposable
    {
        /// <summary>
        /// The clock used to track the time of day.
        /// </summary>
        private EngineTimer<ITimeOfDay> timeOfDayClock;

        public TimeOfDayState()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Occurs when the state's time is changed.
        /// </summary>
        public event EventHandler<ITimeOfDay> TimeUpdated;

        /// <summary>
        /// Gets the name of this state.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the time of day in the game that this state begins.
        /// </summary>
        public ITimeOfDay StateStartTime { get; set; }

        /// <summary>
        /// Gets the current time.
        /// </summary>
        public ITimeOfDay CurrentTime { get; private set; }

        public Guid Id { get; private set; }

        public bool IsEnabled { get; private set; }

        public DateTime CreationDate { get; private set; }

        public double TimeAlive { get { return DateTime.Now.Subtract(this.CreationDate).TotalSeconds; } }

        /// <summary>
        /// Initializes the time of day state with the supplied in-game to real-world hours factor.
        /// </summary>
        /// <param name="worldTimeFactor">The world time factor.</param>
        /// <param name="hoursPerDay">The hours per day.</param>
        public void Initialize(ITimeOfDay startTime, double worldTimeFactor)
        {
            if (startTime == null)
            {
                throw new ArgumentNullException(nameof(startTime), "startTime can not be null.");
            }

            if (startTime.HoursPerDay == 0)
            {
                throw new InvalidTimeOfDayException("HoursPerDay can not be zero.", startTime);
            }

            // Calculate how many minutes in real-world it takes to pass 1 in-game hour.
            double hourInterval = 60 * worldTimeFactor;

            // Calculate how many seconds in real-world it takes to pass 1 minute in-game.
            double minuteInterval = 60 * worldTimeFactor;

            this.StateStartTime = startTime.Clone();
            this.Reset();

            // Update the state every in-game hour or minute based on the ratio we have
            if (minuteInterval < 0.4)
            {
                this.StartStateClock(TimeSpan.FromSeconds(minuteInterval).TotalMilliseconds, (timeOfDay) => timeOfDay.IncrementByHour(1));
            }
            else
            {
                this.StartStateClock(TimeSpan.FromSeconds(minuteInterval).TotalMilliseconds, (timeOfDay) => timeOfDay.IncrementByMinute(1));
            }

            this.Enable();
        }

        /// <summary>
        /// Resets this instance current time to that if its start time.
        /// </summary>
        public void Reset()
        {
            if (this.timeOfDayClock != null)
            {
                this.timeOfDayClock.Stop();
            }

            this.CurrentTime = this.StateStartTime.Clone();
            this.Disable();
        }

        public void Disable()
        {
            this.IsEnabled = false;
        }

        public void Enable()
        {
            this.IsEnabled = true;
        }

        public void Dispose()
        {
            this.Reset();
            this.timeOfDayClock.Dispose();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.CurrentTime != null)
            {
                return string.Format(
                    "{0} starting at {1}:{2} with a curent time of {3}:{4}",
                    this.Name,
                    this.StateStartTime.Hour,
                    this.StateStartTime.Minute,
                    this.CurrentTime.Hour,
                    this.CurrentTime.Minute);
            }
            else
            {
                return string.Format(
                    "{0} starting at {1}:{2}",
                    this.Name,
                    this.StateStartTime.Hour,
                    this.StateStartTime.Minute);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            TimeOfDayState secondState = (TimeOfDayState)obj;

            return secondState.StateStartTime.Hour == this.StateStartTime.Hour && secondState.StateStartTime.Minute == this.StateStartTime.Minute;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.StateStartTime.Hour.GetHashCode() * this.StateStartTime.Minute.GetHashCode() * this.Name.GetHashCode();
        }

        /// <summary>
        /// Starts the state clock at the specified interval, firing the callback provided.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="callback">The callback.</param>
        private void StartStateClock(double interval, Action<ITimeOfDay> callback)
        {
            // If the minute interval is less than 1 second,
            // then we increment by the hour to reduce excess update calls.
            this.timeOfDayClock = new EngineTimer<ITimeOfDay>(this.CurrentTime);
            this.timeOfDayClock.Start(interval, interval, 0, (state, clock) =>
                {
                    callback(state);
                    this.OnTimeUpdated();
                });
        }

        /// <summary>
        /// Called when the states time is updated.
        /// </summary>
        private void OnTimeUpdated()
        {
            EventHandler<ITimeOfDay> handler = this.TimeUpdated;
            if (handler == null)
            {
                return;
            }

            handler(this, this.CurrentTime);
        }
    }
}
