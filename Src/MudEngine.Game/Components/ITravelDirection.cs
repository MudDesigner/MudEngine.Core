//-----------------------------------------------------------------------
// <copyright file="ITravelDirection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game.Components
{
    public interface ITravelDirection
    {
        string Direction { get; }

        ITravelDirection GetOppositeDirection();
    }
}