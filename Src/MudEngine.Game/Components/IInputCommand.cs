using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Game.Components
{
    public interface IInputCommand
    {
        /// <summary>
        /// Gets whether or not this command must run exclusively from any other command.
        /// When true, other command requests will be ignored until this command returns a
        /// InputCommandResult with IsCommandCompleted being true.
        /// </summary>
        bool ExclusiveCommand { get; }

        bool CanExecuteCommand(ICharacter owner, params string[] args);

        Task<InputCommandResult> ExecuteAsync(ICharacter owner, params string[] args);
    }
}
