using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public interface IActorCommand
    {
        string Name { get; }

        bool IsCompleted { get; }

        string[] GetArguments();

        Task<bool> CanProcessCommand(IActor source, params string[] arguments);

        Task ProcessCommand(IActor source, params string[] arguments);
    }
}
