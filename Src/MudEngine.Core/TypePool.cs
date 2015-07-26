using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MudDesigner.MudEngine.Core
{
    /// <summary>
    /// A cached Type Pool used to fetch Types, PropertyInfo's and Attributes from a cache
    /// </summary>
    public static class TypePool
    {
        /// <summary>
        /// The type property cache
        /// </summary>
        private static readonly ConcurrentDictionary<Type, CachedTypeData> TypePropertyCache =
            new ConcurrentDictionary<Type, CachedTypeData>();

        /// <summary>
        /// Gets the matching the provided predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Types</returns>
        public static IEnumerable<Type> GetTypes(Func<Type, bool> predicate)
        {
            var types = new List<Type>();
            return TypePropertyCache.Select(pair => pair.Key).Where(predicate);
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <typeparam name="T">The Type to fetch from cache</typeparam>
        /// <returns>Returns a cached Type</returns>
        public static Type GetType<T>() where T : class
        {
            CachedTypeData cacheType;
            Type itemType = typeof(T);
            TypePropertyCache.TryGetValue(itemType, out cacheType);

            if (cacheType != null)
            {
                return cacheType.Type;
            }

            cacheType = new CachedTypeData(itemType);
            TypePropertyCache.TryAdd(itemType, cacheType);

            return cacheType.Type;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="T">The Type to fetch from cache</typeparam>
        /// <returns>Returns a cached Type</returns>
        public static Type GetType<T>(T item) where T : class
        {
            CachedTypeData cacheType;
            Type itemType = item.GetType();
            TypePropertyCache.TryGetValue(itemType, out cacheType);

            if (cacheType != null)
            {
                return cacheType.Type;
            }

            cacheType = new CachedTypeData(itemType);
            TypePropertyCache.TryAdd(itemType, cacheType);

            return cacheType.Type;
        }

        /// <summary>
        /// Adds the type.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void AddType(Type type)
        {
            CachedTypeData cacheType;
            TypePropertyCache.TryGetValue(type, out cacheType);
            if (cacheType != null)
            {
                return;
            }

            cacheType = new CachedTypeData(type);
            TypePropertyCache.TryAdd(type, cacheType);
        }

        /// <summary>
        /// Gets the name of the type by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns a cached Type</returns>
        public static Type GetTypeByName(string name)
        {
            var t = TypePropertyCache.Keys.FirstOrDefault(k => k.Name == name);

            return t;
        }

        /// <summary>
        /// Gets all attributes for a given Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of attributes</returns>
        public static IEnumerable<Attribute> GetAttributes(Type type, PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttributes(property, predicate);
        }

        /// <summary>
        /// Gets all attributes for a given Type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Returns a collection of attributes
        /// </returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute, TType>(PropertyInfo property = null, Func<Attribute, bool> predicate = null)
            where TAttribute : Attribute
            where TType : class
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(typeof(TType), new CachedTypeData(typeof(TType)));
            return cacheType.GetAttributes<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets all attributes for a given Type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(Type type, PropertyInfo property = null, Func<Attribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttributes<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the type of the attributes for.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute, TType>(TType type, PropertyInfo property = null, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
            where TType : class
        {
            Type desiredType = type == null ? typeof(TType) : type.GetType();
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(desiredType, new CachedTypeData(desiredType));
            return cacheType.GetAttributes<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the single attribute for Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        public static Attribute GetAttribute(Type type, PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttribute(property, predicate);
        }

        /// <summary>
        /// Gets the single attribute matching TAttribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        public static TAttribute GetAttribute<TAttribute>(Type type, PropertyInfo property = null, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttribute<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the type of the attribute for.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        public static TAttribute GetAttribute<TAttribute, TType>(PropertyInfo property = null, TType type = null, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
            where TType : class
        {
            Type desiredType = type == null ? typeof(TType) : type.GetType();
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(desiredType, new CachedTypeData(desiredType));
            return cacheType.GetAttribute<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the properties for the provided Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of PropertyInfo types</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesForType(Type type, Func<PropertyInfo, bool> predicate = null)
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));

            return predicate == null
                ? cacheType.GetProperties()
                : cacheType.GetProperties(predicate);
        }

        /// <summary>
        /// Gets the properties associated with the given items Type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of PropertyInfo types</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesForType<T>(Func<PropertyInfo, bool> predicate = null)
            where T : class
        {
            Type desiredType = typeof(T);
            return GetPropertiesForType(desiredType, predicate);
        }

        /// <summary>
        /// Gets the properties associated with the given items Type.
        /// </summary>
        /// <typeparam name="T">The type to get the property off of</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Returns a collection of PropertyInfo types
        /// </returns>
        public static IEnumerable<PropertyInfo> GetPropertiesForType<T>(T item, Func<PropertyInfo, bool> predicate = null)
            where T : class
        {
            Type desiredType = item.GetType();
            return GetPropertiesForType(desiredType, predicate);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T">The type to get the property off of</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="item">The item.</param>
        /// <returns>Returns a PropertyInfo</returns>
        public static PropertyInfo GetProperty<T>(Func<PropertyInfo, bool> predicate, T item = null)
            where T : class
        {
            Type desiredType = item == null ? typeof(T) : item.GetType();
            return GetProperty(desiredType, predicate);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a PropertyInfo</returns>
        public static PropertyInfo GetProperty(Type type, Func<PropertyInfo, bool> predicate)
        {
            CachedTypeData cacheType = TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            try
            {
                return cacheType.GetProperty(predicate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Clears the type from pool.
        /// </summary>
        /// <typeparam name="T">The type to clear</typeparam>
        public static void ClearTypeFromPool<T>()
        {
            ClearTypeFromPool(typeof(T));
        }

        /// <summary>
        /// Clears the type from pool.
        /// </summary>
        /// <typeparam name="T">The Type to clear out of the cache</typeparam>
        /// <param name="item">The item.</param>
        public static void ClearTypeFromPool<T>(T item)
        {
            ClearTypeFromPool(item.GetType());
        }

        /// <summary>
        /// Clears the type from pool.
        /// </summary>
        /// <param name="typeToClear">The type to clear.</param>
        public static void ClearTypeFromPool(Type typeToClear)
        {
            CachedTypeData CachedTypeData;
            TypePropertyCache.TryRemove(typeToClear, out CachedTypeData);
        }

        /// <summary>
        /// Clears all of the cached information, for all Types.
        /// </summary>
        public static void ClearPool()
        {
            TypePropertyCache.Clear();
        }

        /// <summary>
        /// Determines whether the given Type is already cached.
        /// </summary>
        /// <typeparam name="T">The Type you want to check if it has been cached.</typeparam>
        /// <returns>Returns true if the Type exists within the Pool</returns>
        public static bool HasTypeInCache<T>()
        {
            return HasTypeInCache(typeof(T));
        }

        /// <summary>
        /// Determines whether the given item has its Type in the pool.
        /// </summary>
        /// <typeparam name="T">The Type to perform the check against</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>Returns True if the type exists in the cache</returns>
        public static bool HasTypeInCache<T>(T item)
        {
            return HasTypeInCache(item.GetType());
        }

        /// <summary>
        /// Determines whether the given Type is already cached.
        /// </summary>
        /// <param name="typeInCache">The Type you want to check if it has been cached.</param>
        /// <returns>Returns true if the Type exists within the Pool</returns>
        public static bool HasTypeInCache(Type typeInCache)
        {
            return TypePropertyCache.ContainsKey(typeInCache);
        }
    }
}
