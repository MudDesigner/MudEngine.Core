using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Environment
{
    public interface IWorld : IGameComponent
    {
        event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

        string Name { get; }

        double TimeFromCreation { get; }

        DateTime CreationDate { get; }

        TimeOfDayState CurrentTimeOfDay { get; }

        int HoursPerDay { get; }

        double GameTimeAdjustmentFactory { get; }

        double GameDayToRealHourRatio { get; set; }

        TimeOfDayState[] GetAvailableTimeOfDays();

        IRealm[] GetRealmsInWorld();

        Task AddRealmToWorld(IRealm realm);

        Task AddRealmsToWorld(IEnumerable<IRealm> realms);

        Task RemoveRealmFromWorld(IRealm realm);

        Task RemoveRealmsFromWorld(IEnumerable<IRealm> realms);

        void AddTimeOfDayStateToWorld(TimeOfDayState state);

        void RemoveTimeOfDayStateFromWorld(TimeOfDayState state);

        void SetHoursPerDay(int hours);
    }
}
