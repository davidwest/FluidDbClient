using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluidDbClient.Sql
{
    internal static class InternalPropertyMappingExtensions
    {
        public static Dictionary<string, object> GetPropertyMap(this object obj)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            return GetPropertyMap(obj, type);
        }

        private static Dictionary<string, object> GetPropertyMap(this object obj, Type type)
        {
            return type.GetRuntimeProperties()
                   .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
        }
    }
}
