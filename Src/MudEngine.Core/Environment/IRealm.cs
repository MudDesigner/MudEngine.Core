using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Environment
{
    public interface IRealm : IGameComponent
    {
        ITimeOfDay TimeZoneOffset { get; }

        ITimeOfDay CurrentTime { get; }

        int NumberOfZonesInRealm { get; }

        IZone[] GetZonesInRealm();

        IWorld Owner { get; }

        Task AddZoneToRealm(IZone zone);

        Task AddZonesToRealm(IZone[] zones);

        void ApplyTimeZoneOffset(int hour, int minute);

        ITimeOfDayState GetCurrentTimeOfDayState();

        bool HasZoneInRealm(IZone zone);
    }
}
