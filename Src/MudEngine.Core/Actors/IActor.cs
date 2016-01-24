using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Actors
{
    public interface IActor : IGameComponent
    {
        IRace Race { get; }

        IRoom CurrentRoom { get; }

        IGender Gender { get; }

        void SetGender(IGender gender);

        void SetRace(IRace race);
    }
}
