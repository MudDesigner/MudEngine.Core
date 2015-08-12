using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Actors
{
    public interface IAbility : IDescriptor, IInitializableComponent, IComponent
    {
        string Abbreviation { get; set; }

        int BaseScore { get; }

        int BaseModifier { get; set; }

        int ActualScore { get; }

        Dictionary<string, int> AdditionalModifiers { get; }

        void SetScore(int score);
    }
}
