﻿//-----------------------------------------------------------------------
// <copyright file="ICollectionExtensions.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for instances implementing the ICollection interface.
    /// </summary>
    public static class ICollectionExtensions
    {
        /// <summary>
        /// return a random element of the list or default if list is empty
        /// </summary>
        /// <typeparam name="T">The Type that this method will use to compare</typeparam>
        /// <param name="e">The sender.</param>
        /// <param name="weightSelector">return chances to be picked for the element. A weigh of 0 or less means 0 chance to be picked.
        /// If all elements have weight of 0 or less they all have equal chances to be picked.</param>
        /// <returns>Returns a reference to the item that was selected using the given delegate</returns>
        /// <exception cref="System.Exception">Unable to produce a result from the given collection using the supplied selector.</exception>
        public static T AnyOrDefaultFromWeight<T>(this ICollection<T> e, Func<T, double> weightSelector)
        {
            if (e.Count < 1)
            {
                return default(T);
            }
            else if (e.Count == 1)
            {
                return e.ElementAtOrDefault(0);
            }

            var weights = weightSelector == null ? new double[0] : e.Select(item => Math.Max(weightSelector(item), 0)).ToArray();
            var sum = weights.Sum(d => d);

            var rnd = new Random().NextDouble();
            return FindWeight<T>(e, weights, sum, rnd);
        }

        private static T FindWeight<T>(ICollection<T> e, double[] weights, double sum, double randomValue)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                // Normalize weight
                var w = sum == 0
                    ? 1 / (double)e.Count
                    : weights.ElementAtOrDefault(i) / sum;

                if (randomValue < w)
                {
                    return e.ElementAtOrDefault(i);
                }

                randomValue -= w;
            }

            throw new InvalidOperationException("Unable to produce a result from the given collection using the supplied selector.");
        }
    }
}
