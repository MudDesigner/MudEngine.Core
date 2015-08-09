//-----------------------------------------------------------------------
// <copyright file="StandardMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.MessageBrokering
{
    /// <summary>
    /// Standard string based message
    /// </summary>
    public sealed class StandardMessage : MessageBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public StandardMessage(string message)
        {
            this.Content = message;
        }
    }
}
