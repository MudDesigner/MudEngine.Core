using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Environment
{
    public interface IZone : IGameComponent
    {
        event EventHandler<WeatherStateChangedEventArgs> WeatherChanged;

        int NumberOfRoomsInZone { get; }

        IWeatherState CurrentWeather { get; }

        int WeatherUpdateFrequency { get; }

        IRealm Owner { get; }

        IWeatherState[] GetWeatherStatesForZone();

        IRoom[] GetRoomsForZone();

        Task AddRoomToZone(IRoom room);

        Task AddRoomsToZone(IEnumerable<IRoom> rooms);

        Task RemoveRoomFromZone(IRoom room);

        Task RemoveRoomsFromZone(IEnumerable<IRoom> rooms);

        bool HasRoomInZone(IRoom room);
    }
}
