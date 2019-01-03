using System.Collections.Generic;

namespace FluidDbClient.Sql
{
    public static class EnumerableExtensions
    {
        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, string tableTypeName) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap(tableTypeName);

            if (tableTypeMap != null)
            {
                // use registered map if it exists:
                return items.ToStructuredData(tableTypeMap);
            }

            // otherwise, fall back to inferred building:
            var builder = new StructuredDataBuilder(tableTypeName);

            foreach (var item in items)
            {
                builder.Append(item);
            }

            return builder.Build();
        }

        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, TableTypeDefinition tableTypeDef) where T : class
        {
            var builder = new StructuredDataBuilder(tableTypeDef);

            foreach (var item in items)
            {
                builder.Append(item);
            }

            return builder.Build();
        }
        
        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, TableTypeMap map) where T : class
        {
            return items.ToStructuredData(map.GetDefinition());
        }

        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items) where T : class
        {
            var map = TableTypeRegistry.GetMap<T>();

            return items.ToStructuredData(map);
        }
    }
}
