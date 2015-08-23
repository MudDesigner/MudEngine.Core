//-----------------------------------------------------------------------
// <copyright file="MessageBrokerFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.MessageBrokering
{
    using System;

    /// <summary>
    /// Provides methods for creating a message broker or defining an abstract factory for delegating the creation of a message broker
    /// </summary>
    public static class MessageBrokerFactory
    {
        static Func<IMessageBroker> _factory;

        static IMessageBroker instance;

        public static IMessageBroker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateBroker();
                }

                return instance;
            }
        }

        /// <summary>
        /// Sets a delegate factory that will be responsible for creating a message broker.
        /// </summary>
        /// <param name="factory">The factory delegate.</param>
        public static void SetFactory(Func<IMessageBroker> factory)
        {
            MessageBrokerFactory._factory = factory;
        }

        /// <summary>
        /// Creates a new instance of a message broker.
        /// </summary>
        /// <returns>Returns an IMessageBroker implementation</returns>
        public static IMessageBroker CreateBroker()
        {
            if (MessageBrokerFactory._factory == null)
            {
                return new MessageBroker();
            }

            return _factory();
        }
    }
}
