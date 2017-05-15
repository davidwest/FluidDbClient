using System;
using System.Collections.Generic;

namespace FluidDbClient.Sql
{
    public static class EnumerableExtensions
    {
        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, string tableTypeName, Func<T, object> map = null)
        {
            map = map ?? (item => item);

            var builder = new StructuredDataBuilder(tableTypeName);

            foreach (var item in items)
            {
                builder.Append(map(item));
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

        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, TableTypeMap<T> map) where T : class
        {
            var builder = new StructuredDataBuilder(map);

            foreach (var item in items)
            {
                builder.Append(item);
            }

            return builder.Build();
        }
    }
}
