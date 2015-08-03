using System;

namespace MudDesigner.MudEngine.Game
{
    public interface IGameConfiguration
    {
        string Description { get; set; }
        string Name { get; set; }
        Version Version { get; set; }
        string Website { get; set; }
    }
}