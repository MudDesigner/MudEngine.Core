using System.Collections.Generic;

namespace MudDesigner.MudEngine.Game.Components
{
    public interface ISecurityRole
    {
        string Name { get; }

        IEnumerable<ISecurityPermission> Permissions { get; }
    }
}
