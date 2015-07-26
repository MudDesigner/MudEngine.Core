using System.Collections.Generic;

namespace MudDesigner.MudEngine.Game.Components
{
    public interface IPlayer : ICharacter
    {
        IEnumerable<ISecurityRole> Roles { get; }
    }
}