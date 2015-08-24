using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Actors
{
    public interface ICharacterClass : IDescriptor, IComponent
    {
        void AddModifer(IModifier modifier);

        void RemoveModifier(IModifier modifier);

        IModifier[] GetModifiers();
    }
}
