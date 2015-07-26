using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Game.Components;

namespace MudDesigner.MudEngine.Game.Components
{
    public interface ICommandManager
    {
        void SetOwner(ICharacter owningCharacter);

        Task ProcessCommandForCharacter(string command, string[] args);

        Task ProcessCommandForCharacter(IInputCommand command, string[] args);
    }
}
