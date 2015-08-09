//-----------------------------------------------------------------------
// <copyright file="IRealm.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides properties and methods for defining and maintaining Realms
    /// </summary>
    public interface IRealm : IGameComponent
    {
        /// <summary>
        /// Gets or sets the offset from the World's current time for the Realm.
        /// </summary>
        ITimeOfDay TimeZoneOffset { get; }

        /// <summary>
        /// Gets or sets the time of day for the Realm.
        /// </summary>
        ITimeOfDay CurrentTime { get; }

        /// <summary>
        /// Gets the number of zones in realm.
        /// </summary>]
        int NumberOfZonesInRealm { get; }

        /// <summary>
        /// Gets the all of the zones assigned to the realm.
        /// </summary>
        /// <returns>Returns an array of each Zone contained in the Realm.</returns>
        IZone[] GetZonesInRealm();

        /// <summary>
        /// Gets the World that owns this Realm.
        /// </summary>
        IWorld Owner { get; }

        /// <summary>
        /// Adds a given zone to this Realm.
        /// </summary>
        /// <param name="zone">The zone to add to the realm.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddZoneToRealm(IZone zone);

        /// <summary>
        /// Adds a collection of zones to the realm.
        /// </summary>
        /// <param name="zones">The collection of zones to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddZonesToRealm(IEnumerable<IZone> zones);

        /// <summary>
        /// Applies a time zone offset to the Realm.
        /// </summary>
        /// <para>
        /// The Hour and Minute provided will cause the Realm's timezone to be offset from
        /// the Worlds standard time-zone.
        /// </para>
        /// <param name="hour">The number of hours to offset by.</param>
        /// <param name="minute">The number of minutes to offset by.</param>
        void ApplyTimeZoneOffset(int hour, int minute);

        /// <summary>
        /// Gets the state of the current time of day.
        /// </summary>
        /// <returns>Returns the current Time of Day State the Realm is in.</returns>
        ITimeOfDayState GetCurrentTimeOfDayState();

        /// <summary>
        /// Determines whether this Realm contains the given zone.
        /// </summary>
        /// <param name="zone">The zone to lookup.</param>
        /// <returns>Returns true if the Zone exists in the Realm.</returns>
        bool HasZoneInRealm(IZone zone);
    }
}
