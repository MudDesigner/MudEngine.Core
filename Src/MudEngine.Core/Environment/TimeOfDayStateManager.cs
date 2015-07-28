﻿//-----------------------------------------------------------------------
// <copyright file="TimeOfDayStateManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for fetching an ITimeOfDayState implementation based on a TimeOfDay instance.
    /// </summary>
    internal class TimeOfDayStateManager
    {
        /// <summary>
        /// The time of day states provided by an external source
        /// </summary>
        private IEnumerable<TimeOfDayState> timeOfDayStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDayStateManager"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        internal TimeOfDayStateManager(IEnumerable<TimeOfDayState> states)
        {
            this.timeOfDayStates = states;
        }

        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimeOfDayState that represents the current time of day in the game.
        /// </returns>
        internal TimeOfDayState GetTimeOfDayState(DateTime? currentTime = null)
        {
            TimeOfDay time = new TimeOfDay();
            time.Hour = currentTime.Value.Hour;
            time.Minute = currentTime.Value.Minute;

            return this.GetTimeOfDayState(time);
        }

        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentGameTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimeOfDayState that represents the current time of day in the game.
        /// </returns>
        internal TimeOfDayState GetTimeOfDayState(TimeOfDay currentGameTime = null)
        {
            TimeOfDayState inProgressState = null;
            TimeOfDayState nextState = null;

            Parallel.Invoke(
                () => inProgressState = this.GetInProgressState(currentGameTime),
                () => nextState = this.GetNextState(currentGameTime));

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
        private TimeOfDayState GetInProgressState(TimeOfDay currentTime)
        {
            TimeOfDayState inProgressState = null;
            foreach (TimeOfDayState state in this.timeOfDayStates)
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
                        if (inProgressState.StateStartTime.Hour <= currentTime.Hour &&
                            inProgressState.StateStartTime.Minute <= currentTime.Minute)
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
        private TimeOfDayState GetNextState(TimeOfDay currentTime)
        {
            TimeOfDayState nextState = null;
            foreach (TimeOfDayState state in this.timeOfDayStates)
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
