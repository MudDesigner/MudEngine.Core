﻿//-----------------------------------------------------------------------
// <copyright file="RainyWeather.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    /// <summary>
    /// Represents rainy weather.
    /// </summary>
    public sealed class RainyWeather : IWeatherState
    {
        /// <summary>
        /// Gets the occurrence probability of this weather state occurring in an environment.
        /// The higher the probability relative to other weather states, the more likely it is going to occur.
        /// </summary>
        public double OccurrenceProbability
        {
            get
            {
                return 30;
            }
        }

        /// <summary>
        /// Gets the name of the weather state.
        /// </summary>
        public string Name
        {
            get
            {
                return "Rainy";
            }
        }
    }
}