//-----------------------------------------------------------------------
// <copyright file="ISubscription.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.MessageBrokering
{
    using System;

    public sealed class NotificationArgs
    {
        public NotificationArgs(ISubscription subscription, Type messageType)
        {
            this.Subscription = subscription;
            this.MessageType = messageType;
        }

        public ISubscription Subscription { get; private set; }

        public Type MessageType { get; private set; }
    }
}
