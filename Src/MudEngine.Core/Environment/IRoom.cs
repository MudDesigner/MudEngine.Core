using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Environment
{
    public interface IRoom : IGameComponent
    {
        event EventHandler<OccupancyChangedEventArgs> OccupantEntered;

        event EventHandler<OccupancyChangedEventArgs> OccupantLeft;
        
        IZone Owner { get; }

        bool IsSealed { get; }

        IDoorway[] GetDoorwaysForRoom();

        IActor[] GetActorsInRoom();

        ICharacter[] GetCharactersInRoom();

        Task AddActorToRoom(IActor actor);

        Task AddActorsToRoom(IEnumerable<IActor> actors);

        Task RemoveActorFromRoom(IActor actor);

        Task RemoveActorsFromRoom(IEnumerable<IActor> actors);

        IDoorway CreateDoorway(ITravelDirection travelDirectionToReachDoor);

        Task AddDoorwayToRoom(IDoorway doorway);

        Task RemoveDoorwayFromRoom(IDoorway doorway);

        Task RemoveDoorwayFromRoom(ITravelDirection travelDirection);

        /// <summary>
        /// Seals the room so that no other actors may enter and any existing IActors may not leave.
        /// </summary>
        void SealRoom();

        void UnsealRoom();
    }
}
