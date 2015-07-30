//-----------------------------------------------------------------------
// <copyright file="IComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System;

    public interface IComponent
    {
        Guid Id { get; }

        bool IsEnabled { get; }

        DateTime CreationDate { get; }

        double TimeAlive { get; }

        void Disable();

        void Enable();
    }
}
