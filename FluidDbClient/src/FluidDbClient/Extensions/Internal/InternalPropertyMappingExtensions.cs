using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluidDbClient
{
    internal static class InternalPropertyMappingExtensions
    {
        public static Dictionary<string, object> GetPropertyMap(this object obj, IDbProvider provider)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            return GetPropertyMap(obj, type, provider);
        }
        
        public static Dictionary<string, object> GetSimplePropertyMapIfAnonymous(this object obj)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            // --- NOTE: this test is sufficient for the usage context ---
            var isAnonymous = type.Namespace == null;
            
            return isAnonymous ? GetSimplePropertyMap(obj, type) : null;
        }
        
        public static ILookup<string, string> GetMultiParamReplacementMap(this IEnumerable<string> paramNames)
        {
            var result =
                paramNames
                .Select(pn => new
                {
                    SourceName = pn,
                    BaseName = TryDecodeMultiParamBaseName(pn)
                })
                .Where(pair => pair.BaseName != null)
                .ToLookup(pair => pair.BaseName, pair => pair.SourceName);

            return result;
        }

        private static Dictionary<string, object> GetSimplePropertyMap(this object obj, Type type)
        {
            return type.GetRuntimeProperties()
                   .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
        }

        private static Dictionary<string, object> GetPropertyMap(this object obj, Type type, IDbProvider provider)
        {
            var map = new Dictionary<string, object>();

            foreach (var prop in type.GetRuntimeProperties())
            {
                var name = prop.Name;
                var val = prop.GetValue(obj);

                if (!(val is IEnumerable items) || !provider.Interpreter.CanEvaluateAsMultiParameters(items))
                {
                    map[name] = val;
                    continue;
                }

                var i = 0;

                foreach (var item in items)
                {
                    var itemName = EncodeEnumerableItemParamName(name, i);
                    i++;

                    map[itemName] = item;
                }                
            }

            return map;
        }

        private static string EncodeEnumerableItemParamName(string name, int index)
        {
            return $"__{name}__{index}";
        }

        private static string TryDecodeMultiParamBaseName(string sourceName)
        {
            var parts = sourceName.Split(MultiParamDelimeters, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2) return null;

            var name = parts[0];

            return !int.TryParse(parts[1], out _) 
                ? null 
                : name;
        }

        private static readonly string[] MultiParamDelimeters = {"__"};
    }
}
