using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Game.Components
{
    public interface IWorld : IGameComponent
    {
        event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets how many seconds have passed since the creation date.
        /// </summary>
        double TimeFromCreation { get; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the current time of day.
        /// </summary>
        TimeOfDayState CurrentTimeOfDay { get; set; }

        /// <summary>
        /// Gets a collection of states that can be used for the time of day.
        /// </summary>
        TimeOfDayState[] TimeOfDayStates { get; }

        /// <summary>
        /// Gets or sets hour many hours it takes in-game to complete 1 day.
        /// </summary>
        int HoursPerDay { get; set; }

        /// <summary>
        /// Gets or sets the hours ratio. If set to 4, it takes 4 in-game hours to equal 1 real-world hour.
        /// </summary>
        double GameDayToRealHourRatio { get; set; }

        /// <summary>
        /// Gets the game time ratio used to convert real-world time to game-time.
        /// </summary>
        double GameTimeAdjustmentFactor { get; }

        /// <summary>
        /// Gets the realms in this world.
        /// </summary>
        DefaultRealm[] Realms { get; }

        /// <summary>
        /// Adds the given realm to world and initializes it.
        /// </summary>
        /// <param name="realm">The realm.</param>
        Task AddRealmToWorld(DefaultRealm realm);

        /// <summary>
        /// Adds a collection of realms to world.
        /// </summary>
        /// <param name="realms">The realm collection.</param>
        Task AddRealmsToWorld(IEnumerable<DefaultRealm> realms);

        /// <summary>
        /// Removes the realm from world.
        /// </summary>
        /// <param name="realm">The realm.</param>
        void RemoveRealmFromWorld(DefaultRealm realm);

        /// <summary>
        /// Adds the time of day state to the world.
        /// </summary>
        /// <param name="state">The state.</param>
        void AddTimeOfDayState(TimeOfDayState state);
    }
}
