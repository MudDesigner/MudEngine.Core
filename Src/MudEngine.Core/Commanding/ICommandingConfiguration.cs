﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Commanding
{
    public interface ICommandingConfiguration : IConfiguration
    {
        ICommandFactory CommandFactory { get; }

        IActorCommand[] GetCommands();
    }
}
