﻿//-----------------------------------------------------------------------
// <copyright file="ExceptionFactoryResult.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Core
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ExceptionFactoryResult<TException> where TException : Exception, new()
    {
        private IComponent component;

        public ExceptionFactoryResult(IComponent component)
        {
            this.component = component;
        }

        /// <summary>
        /// Callback on the results of an ExceptionFactory invocation
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TException> ElseDo(Action callback)
        {
            ExceptionFactory.ThrowIf<ArgumentNullException>(
                callback == null,
                "callback can not be null",
                component);
            callback();

            return this;
        }

        /// <summary>
        /// Permits piggybacking on the result of an ExceptionFactory.ThrowIf invocation with an
        /// additional predicate check that results in an exception being thrown if true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TException> Or(Func<bool> predicate, string message = null)
        {
            // TODO: Need to just mirror the ThrowIf signatures so Or invocations can provide data.
            return ExceptionFactory.ThrowIf<TException>(predicate, message, component);
        }

        /// <summary>
        /// Permits piggybacking on the result of an ExceptionFactory.ThrowIf invocation with an
        /// additional predicate check that results in an exception being thrown if true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TAlternateException> Or<TAlternateException>(Func<bool> predicate, string message = null) where TAlternateException : Exception, new()
        {
            return ExceptionFactory.ThrowIf<TAlternateException>(predicate, message, component);
        }

        /// <summary>
        /// Permits piggybacking on the result of an ExceptionFactory.ThrowIf invocation with an
        /// additional predicate check that results in an exception being thrown if true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TAlternateException> Or<TAlternateException>(Func<bool> predicate, Func<TAlternateException> exceptionFactory) where TAlternateException : Exception, new()
        {
            return ExceptionFactory.ThrowIf(predicate, exceptionFactory, component);
        }

        /// <summary>
        /// Permits piggybacking on the result of an ExceptionFactory.ThrowIf invocation with an
        /// additional condition check that results in an exception being thrown if true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TException> Or(bool condition, string message = null)
        {
            return ExceptionFactory.ThrowIf<TException>(condition, message, component);
        }

        /// <summary>
        /// Permits piggybacking on the result of an ExceptionFactory.ThrowIf invocation with an
        /// additional condition check that results in an exception being thrown if true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TAlternateException> Or<TAlternateException>(bool condition, string message = null) where TAlternateException : Exception, new()
        {
            return ExceptionFactory.ThrowIf<TAlternateException>(condition, message, component);
        }

        /// <summary>
        /// Permits piggybacking on the result of an ExceptionFactory.ThrowIf invocation with an
        /// additional condition check that results in an exception being thrown if true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <returns>Returns an instance of ExceptionFactoryResult</returns>
        public ExceptionFactoryResult<TAlternateException> Or<TAlternateException>(bool condition, Func<TAlternateException> exceptionFactory) where TAlternateException : Exception, new()
        {
            return ExceptionFactory.ThrowIf(condition, exceptionFactory, component);
        }
    }
}
