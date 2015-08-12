using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Actors
{
    public interface IActor : IGameComponent
    {
        IRace Race { get; }

        string Description { get; set; }

        IGender Gender { get; }

        void SetGender(IGender gender);

        void SetRace(IRace race);
    }
}
