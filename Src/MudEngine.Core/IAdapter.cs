//-----------------------------------------------------------------------
// <copyright file="IAdapter.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    public interface IAdapter : IMessagingComponent, IInitializableComponent
    {
        string Name { get; }
    }
}
