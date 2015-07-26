//-----------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MudDesigner.MudEngine.Game.Character;

namespace MudDesigner.MudEngine.Game
{
    /// <summary>
    /// A chat message.
    /// </summary>
    public class ShoutMessage : ChatMessageBase
    {
        public ShoutMessage(string message, DefaultPlayer sender) : base(message, sender)
        {
        }
    }
}
