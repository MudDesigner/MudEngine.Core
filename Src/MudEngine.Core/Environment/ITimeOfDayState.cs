using System;

namespace MudDesigner.MudEngine.Environment
{
    public interface ITimeOfDayState : IComponent
    {
        event EventHandler<ITimeOfDay> TimeUpdated;

        ITimeOfDay CurrentTime { get; }

        string Name { get; set; }

        ITimeOfDay StateStartTime { get; }

        void Initialize(ITimeOfDay startTime, double worldTimeFactor);

        void Reset();
    }
}