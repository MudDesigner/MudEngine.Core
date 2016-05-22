using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Commanding;

namespace MudDesigner.MudEngine.Actors
{
    public interface IPlayer : ICharacter
    {
        IActorCommand InitialCommand { get; }


    }
}