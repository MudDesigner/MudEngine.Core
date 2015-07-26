//-----------------------------------------------------------------------
// <copyright file="ExceptionFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides helper methods for quickly throwing exceptions based on conditions.
    /// </summary>
    public static class ExceptionFactory
    {
        /// <summary>
        /// Throws the exception if predicate is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="message">The message.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult<TException> ThrowIf<TException>(Func<bool> predicate, string message = null, IComponent component = null, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Exception Predicate must not be null.");
            }

            return ThrowIf<TException>(predicate(), message, component, data);
        }

        /// <summary>
        /// Throws the exception given by the delegate if predicate is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="exceptionFactory">The exception.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult<TException> ThrowIf<TException>(Func<bool> predicate, Func<TException> exceptionFactory, IComponent component = null, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            if (predicate == null) 
            {
                throw new ArgumentNullException(nameof(predicate), "Exception Predicate must not be null.");
            }

            return ThrowIf<TException>(predicate(), exceptionFactory, component, data);
        }

        /// <summary>
        /// Throws the exception if condition is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="message">The message.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult<TException> ThrowIf<TException>(bool condition, string message = null, IComponent component = null, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            return ThrowIf<TException>(
                condition,
                () => (TException)Activator.CreateInstance(typeof(TException), message),
                component,
                data);
        }

        /// <summary>
        /// Throws the given exception from the delegate if condition is true.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <param name="exception">The exception.</param>
        /// <param name="character">The character.</param>
        /// <param name="data">The data.</param>
        public static ExceptionFactoryResult<TException> ThrowIf<TException>(bool condition, Func<TException> exception, IComponent component = null, params KeyValuePair<string, string>[] data) where TException : Exception, new()
        {
            if (exception == null)
            {
                throw new ArgumentNullException("Exception factory must not be null.");
            }

            if (!condition)
            {
                return new ExceptionFactoryResult<TException>(component);
            }

            TException exceptionToThrow = exception();
            if (exceptionToThrow == null)
            {
                throw new InvalidOperationException("Exception Factory delegate returned a null exception.");
            }

            AddExceptionData(exceptionToThrow, data);

            if (component != null)
            {
                AddExceptionData(
                    exceptionToThrow,
                    new KeyValuePair<string, string>("ComponentType", component.GetType().Name),
                    new KeyValuePair<string, string>("ComponentId", component.Id.ToString()));
            }

            AddExceptionData(
                exceptionToThrow,
                new KeyValuePair<string, string>("Date", DateTime.Now.ToString()));

            throw exceptionToThrow;
        }

        /// <summary>
        /// Adds data to a given exception.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <param name="data">The data.</param>
        public static void AddExceptionData<TException>(TException exception, params KeyValuePair<string, string>[] data) where TException : Exception
        {
            foreach (var exceptionData in data)
            {
                exception.Data.Add(exceptionData.Key, exceptionData.Value);
            }
        }
    }
}
