using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.MessageBrokering;

namespace MudDesigner.MudEngine
{
    public abstract class AdapterBase : IAdapter, IDisposable
    {
        private Dictionary<Type, ISubscription> subscriptions;

        public AdapterBase()
        {
            this.subscriptions = new Dictionary<Type, ISubscription>();
        }

        public abstract string Name { get; }

        public IMessageBroker MessageBroker { get; set; }

        public void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            if (this.MessageBroker == null)
            {
                throw new NullReferenceException($"{this.GetType().Name} has a null INotificationCenter reference and can not use it to publish messages.");
            }

            this.MessageBroker.Publish(message);
        }

        public void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage
        {
            if (this.MessageBroker == null)
            {
                throw new NullReferenceException($"{this.GetType().Name} has a null INotificationCenter reference and can not use it to subscribe to publications.");
            }

            ISubscription subscription = null;
            Type messageType = typeof(TMessage);
            if (this.subscriptions.TryGetValue(messageType, out subscription))
            {
                subscription.Unsubscribe();
                this.subscriptions.Remove(messageType);
            }

            subscription = this.MessageBroker.Subscribe<TMessage>(callback, predicate);
            this.subscriptions.Add(messageType, subscription);
        }

        public void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage
        {
            Type messageType = typeof(TMessage);
            ISubscription subscription = null;
            if (!this.subscriptions.TryGetValue(messageType, out subscription))
            {
                return;
            }

            subscription.Unsubscribe();
            this.subscriptions.Remove(messageType);
        }

        public void UnsubscribeFromAllMessages()
        {
            foreach (KeyValuePair<Type, ISubscription> pair in this.subscriptions)
            {
                pair.Value.Unsubscribe();
            }

            this.subscriptions.Clear();
        }

        public void SetNotificationManager(IMessageBroker broker)
        {
            this.MessageBroker = broker;
        }

        public abstract Task Initialize();

        public abstract Task Delete();

        public void Dispose()
        {
            this.UnsubscribeFromAllMessages();
        }
    }
}
