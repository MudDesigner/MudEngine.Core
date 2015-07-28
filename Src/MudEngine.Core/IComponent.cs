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
    }
}
