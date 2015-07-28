//-----------------------------------------------------------------------
// <copyright file="IWeatherState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Environment
{
    public interface IWeatherState
    {
        double OccurrenceProbability { get; }

        string Name { get; }
    }
}