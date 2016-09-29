
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluidDbClient
{
    internal static class InternalPropertyMappingExtensions
    {
        public static Dictionary<string, object> GetPropertyMap(this object obj)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            return GetPropertyMap(obj, type);
        }

        public static Dictionary<string, object> GetPropertyMapIfAnonymous(this object obj)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            // --- NOTE: this test is sufficient for the usage context ---
            var isAnonymous = type.Namespace == null;

            return isAnonymous ? GetPropertyMap(obj, type) : null;
        }

        private static Dictionary<string, object> GetPropertyMap(this object obj, Type type)
        {
            return type.GetRuntimeProperties()
                   .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
        }

    }
}
