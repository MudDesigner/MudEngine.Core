using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Game.Components;

namespace MudDesigner.MudEngine.Game.Components
{
    public class InformationMessage : CharacterMessage
    {
        public InformationMessage(string message, ICharacter targetCharacter)
            : base(message, targetCharacter)
        {
        }
    }
}
