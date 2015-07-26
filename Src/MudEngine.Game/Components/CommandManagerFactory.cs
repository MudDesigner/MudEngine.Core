using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Game.Components
{
    public static class CommandManagerFactory
    {
        private static Func<ICommandManager> factoryDelegate;

        public static void SetFactory(Func<ICommandManager> factory)
        {
            CommandManagerFactory.factoryDelegate = factory;
        }

        public static ICommandManager CreateManager()
        {
            if (CommandManagerFactory.factoryDelegate == null)
            {
                // TODO: Fetch a collection of security roles to pass in to our internal manager
                return new CommandManager(Enumerable.Empty<ISecurityRole>(), Enumerable.Empty<IInputCommand>(), new NotificationManager());
            }

            return CommandManagerFactory.factoryDelegate();
        }
    }
}
