using System;

namespace MudDesigner.MudEngine.Environment
{
    public interface ITimeOfDayState : IComponent
    {
        event EventHandler<TimeOfDay> TimeUpdated;

        TimeOfDay CurrentTime { get; }

        string Name { get; set; }

        TimeOfDay StateStartTime { get; set; }

        void Initialize(double worldTimeFactor, int hoursPerDay);

        void Reset();
    }
}