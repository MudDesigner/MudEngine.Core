using System;
using System.Threading.Tasks;
using MudDesigner.MudEngine.MessageBrokering;

namespace MudDesigner.MudEngine
{
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

        string Name { get; }

        void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage;

        void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage;

        void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage;

        void UnsubscribeFromAllMessages();

        void SetNotificationManager(INotificationCenter notificationManager);
    }
}
