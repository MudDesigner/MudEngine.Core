using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Actors
{
    public interface IActor : IDescriptor, IGameComponent
    {
        IRace Race { get; }

        IGender Gender { get; }

        void SetGender(IGender gender);

        void SetRace(IRace race);
    }
}
