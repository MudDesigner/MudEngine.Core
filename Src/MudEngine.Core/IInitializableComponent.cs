//-----------------------------------------------------------------------
// <copyright file="IInitializableComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for initializing objects used by the engine and cleaning up when they are no longer needed.
    /// </summary>
    public interface IInitializableComponent
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns></returns>
        Task Initialize();

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <returns></returns>
        Task Delete();
    }
}
