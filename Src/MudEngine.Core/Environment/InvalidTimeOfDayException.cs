using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
