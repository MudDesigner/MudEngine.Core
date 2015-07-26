using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Game.Components
{
    public class InputCommandResult
    {
        public InputCommandResult(string result, bool isCommandCompleted, IInputCommand command, ICharacter executor)
        {
            this.Result = result;
            this.IsCommandCompleted = isCommandCompleted;
            this.CommandExecuted = command;
            this.Executor = executor;
        }

        public InputCommandResult(bool isCommandCompleted, IInputCommand command, ICharacter executor)
        {
            this.Result = string.Empty;
            this.IsCommandCompleted = isCommandCompleted;
            this.CommandExecuted = command;
            this.Executor = executor;
        }

        public bool HasContent
        {
            get
            {
                return !string.IsNullOrEmpty(this.Result);
            }
        }

        public string Result { get; private set; }

        public bool IsCommandCompleted { get; private set; }

        public IInputCommand CommandExecuted { get; private set; }

        public ICharacter Executor { get; private set; }
    }
}
