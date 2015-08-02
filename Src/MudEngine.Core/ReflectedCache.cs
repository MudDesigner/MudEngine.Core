using System;
using System.Collections.Concurrent;

namespace MudDesigner.MudEngine
{
    /// <summary>
    /// A cached Type Pool used to fetch Types, PropertyInfo's and Attributes from a cache
    /// </summary>
    public static class ReflectedCache
    {
        /// <summary>
        /// The type property cache
        /// </summary>
        internal static readonly ConcurrentDictionary<Type, CachedTypeData> TypePropertyCache =
            new ConcurrentDictionary<Type, CachedTypeData>();

        /// <summary>
        /// Clears all of the cached information, for all Types.
        /// </summary>
        public static void ClearAllReflectedCache()
        {
            TypePropertyCache.Clear();
        }
    }
}
