using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.MessageBrokering
{
    public static class NotificationManagerFactory
    {
        private static Func<IMessageBroker> _factory;

        public static void SetFactory(Func<IMessageBroker> factory)
        {
            NotificationManagerFactory._factory = factory;
        }

        public static IMessageBroker CreateNotificationCenter()
        {
            return _factory();
        }
    }
}
