//-----------------------------------------------------------------------
// <copyright file="IGameComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    using System;
    using System.Threading.Tasks;
    using MudDesigner.MudEngine.MessageBrokering;

    /// <summary>
    /// Provides methods, events and properties for interacting with a component during it's life-cycle.
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

        /// <summary>
        /// Gets the name of this component.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Publishes a given message to any subscriber.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message payload.</param>
        void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage;

        /// <summary>
        /// Subscribes to a specific message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="callback">The callback delegate that can handle the payload provided.</param>
        /// <param name="predicate">The predicate that governs whether or not the callback is invoked..</param>
        void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage;

        /// <summary>
        /// Unsubscribes from listening to publications of the message specified.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage;

        /// <summary>
        /// Unsubscribes from all messages.
        /// </summary>
        void UnsubscribeFromAllMessages();

        /// <summary>
        /// Sets the notification manager to be used by this component.
        /// </summary>
        /// <param name="notificationManager">The notification manager.</param>
        void SetNotificationManager(INotificationCenter notificationManager);
    }
}
