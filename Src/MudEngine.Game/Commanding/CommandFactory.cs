using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Commanding;

namespace MudEngine.Game.Commanding
{
    public class CommandFactory : ICommandFactory
    {
        public IActorCommand CreateCommand(string command)
        {
            throw new NotImplementedException();
        }

        public bool IsCommandAvailable(string command)
        {
            throw new NotImplementedException();
        }
    }
}
