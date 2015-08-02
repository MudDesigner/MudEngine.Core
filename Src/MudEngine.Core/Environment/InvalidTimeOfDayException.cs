using System;

namespace MudDesigner.MudEngine.Environment
{
    public sealed class InvalidTimeOfDayException : Exception
    {
        public InvalidTimeOfDayException(string message, ITimeOfDay timeOfDay) : base(message)
        {
            this.Data.Add(timeOfDay.GetType(), timeOfDay);
        }
    }
}
