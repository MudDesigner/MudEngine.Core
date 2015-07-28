//-----------------------------------------------------------------------
// <copyright file="NorthDirection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    /// <summary>
    /// A direction that represents East.
    /// </summary>
    public sealed class NorthDirection : ITravelDirection
    {
        /// <summary>
        /// Gets the direction that can be traveled.
        /// </summary>
        public string Direction 
        { 
            get
            {
                return "North";
            }
        }

        /// <summary>
        /// Gets the opposite direction associated with this instance.
        /// </summary>
        /// <returns>
        /// Returns the direction required to travel in order to go in the opposite direction of this instance.
        /// </returns>
        public ITravelDirection GetOppositeDirection()
        {
            return new SouthDirection();
        }
    }
}
