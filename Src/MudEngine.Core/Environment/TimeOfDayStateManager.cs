//-----------------------------------------------------------------------
// <copyright file="TimeOfDayStateManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Provides methods for fetching an ITimeOfDayState implementation based on a TimeOfDay instance.
    /// </summary>
    public sealed class TimeOfDayStateManager
    {
        private static Func<double, double, int, ITimeOfDay> _factory;

        private static int _hoursPerDay;

        /// <summary>
        /// The time of day states provided by an external source
        /// </summary>
        private IEnumerable<ITimeOfDayState> timeOfDayStates;

        public static void SetFactory(Func<double, double, int, ITimeOfDay> factory)
        {
            _factory = factory;
        }

        public static void SetDefaultHoursPerDay(int hours)
        {
            _hoursPerDay = hours;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDayStateManager"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        public TimeOfDayStateManager(IEnumerable<ITimeOfDayState> states)
        {
            if (states == null)
            {
                throw new ArgumentNullException(nameof(states), "You must provide the TimeOfDayStateManager a collection of states that is not null.");
            }

            this.timeOfDayStates = states.OrderBy(item => item.StateStartTime.Hour).ThenBy(item => item.StateStartTime.Minute);
        }

        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimeOfDayState that represents the current time of day in the game.
        /// </returns>
        public ITimeOfDayState GetTimeOfDayState(DateTime? currentTime = null)
        {
            ITimeOfDay time = TimeOfDayStateManager._factory(currentTime.Value.Hour, currentTime.Value.Minute, _hoursPerDay);
            
            return this.GetTimeOfDayState(time);
        }

        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentGameTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimeOfDayState that represents the current time of day in the game.
        /// </returns>
        public ITimeOfDayState GetTimeOfDayState(ITimeOfDay currentGameTime = null)
        {
            ITimeOfDayState inProgressState = null;
            ITimeOfDayState nextState = null;

            inProgressState = this.GetInProgressState(currentGameTime);
            nextState = this.GetNextState(currentGameTime);

            if (inProgressState != null)
            {
                return inProgressState;
            }
            else if (nextState != null && nextState.StateStartTime.Hour <= currentGameTime.Hour && nextState.StateStartTime.Minute <= currentGameTime.Minute)
            {
                return nextState;
            }

            return null;
        }

        /// <summary>
        /// Gets a state if there is one already in progress.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimeOfDayState that represents the current time of day if an instance with a StartTime 
        /// before the current world-time can be found 
        /// </returns>
        private ITimeOfDayState GetInProgressState(ITimeOfDay currentTime)
        {
            ITimeOfDayState inProgressState = null;
            foreach (ITimeOfDayState state in this.timeOfDayStates)
            {
                // If the state is already in progress, w
                if (state.StateStartTime.Hour <= currentTime.Hour ||
                    (state.StateStartTime.Hour <= currentTime.Hour && 
                    state.StateStartTime.Minute <= currentTime.Minute))
                {
                    if (inProgressState == null)
                    {
                        inProgressState = state;
                        continue;
                    }
                    else
                    {
                        if ((inProgressState.StateStartTime.Hour <= currentTime.Hour) ||
                            (inProgressState.StateStartTime.Hour == currentTime.Hour &&
                            inProgressState.StateStartTime.Minute <= currentTime.Minute))
                        {
                            inProgressState = state;
                        }
                    }
                }
            }

            return inProgressState;
        }

        /// <summary>
        /// Gets the state that is up next.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimeOfDayState that represents the up coming time of day if an instance with a StartTime 
        /// after the current world-time can be found 
        /// </returns>
        private ITimeOfDayState GetNextState(ITimeOfDay currentTime)
        {
            ITimeOfDayState nextState = null;
            foreach (ITimeOfDayState state in this.timeOfDayStates)
            {
                // If this state is a future state, then preserve it as a possible next state.
                if (state.StateStartTime.Hour > currentTime.Hour ||
                    (state.StateStartTime.Hour >= currentTime.Hour && 
                    state.StateStartTime.Minute > currentTime.Minute))
                {
                    // If we do not have a next state, set it.
                    if (nextState == null)
                    {
                        nextState = state;
                        continue;
                    }
                    else
                    {
                        // We have a next state, so we must check which is sooner.
                        if (nextState.StateStartTime.Hour > state.StateStartTime.Hour &&
                            nextState.StateStartTime.Minute >= state.StateStartTime.Minute)
                        {
                            nextState = state;
                        }
                    }
                }
            }

            return nextState;
        }
    }
}
