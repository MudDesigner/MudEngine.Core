//-----------------------------------------------------------------------
// <copyright file="ICloneableComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    /// <summary>
    /// Requires an object to support cloning of itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneableComponent<T>
    {
        /// <summary>
        /// Clones the properties of this instance to a new instance.
        /// Calling Clone does not guarantee that the clone will be a deep clone or a shallow.
        /// </summary>
        /// <returns>Returns a new instance with the properties of this instance copied to it.</returns>
        T Clone();
    }
}
