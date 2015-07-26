//-----------------------------------------------------------------------
// <copyright file="IGameComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Game
{
    using Core;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a series of event for components that the game needs to manage
    /// </summary>
    public interface IGameComponent : IComponent, IInitializableComponent
    {
        /// <summary>
        /// The Loading event is fired during initialization of the component prior to being loaded.
        /// </summary>
        event Func<IGameComponent, Task> Loading;

        /// <summary>
        /// The Loaded event is fired upon completion of the components initialization and loading.
        /// </summary>
        event EventHandler<EventArgs> Loaded;

        /// <summary>
        /// The Deleting event is fired immediately upon a delete request.
        /// </summary>
        event Func<IGameComponent, Task> Deleting;

        /// <summary>
        /// The Deleted event is fired once the object has finished processing it's unloading and clean up.
        /// </summary>
        event EventHandler<EventArgs> Deleted;

        INotificationCenter NotificationCenter { get; }

        void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage;

        void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage;

        void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage;

        void UnsubscribeFromAllMessages();

        void SetNotificationManager(INotificationCenter notificationManager);
    }
}
