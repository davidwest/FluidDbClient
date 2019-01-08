using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public static class EnumerableToStructuredDataExtensions
    {
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            TypeMapOption option = TypeMapOption.Strict) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap<T>();

            return tableTypeMap != null 
                ? items.ToStructuredData(tableTypeMap) 
                : items.ToStructuredData(typeof(T).Name, option);
        }

        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            TableTypeMap map, 
            TypeMapOption option = TypeMapOption.Strict) where T : class
        {
            return items.ToStructuredData(map.GetDefinition(), option);
        }
        
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            TableTypeDefinition def, 
            TypeMapOption option = TypeMapOption.Strict) 
            where T : class
        {
            return new StructuredData(def.TypeName, items.ToSqlRecords(def, option));
        }
        
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            string tableTypeName, 
            TypeMapOption option = TypeMapOption.Strict) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap(tableTypeName);

            if (tableTypeMap != null)
            {
                return items.ToStructuredData(tableTypeMap, option);
            }

            // fall back to using a builder, which allows inference:
            var builder = new StructuredDataBuilder(tableTypeName, option);

            foreach (var item in items)
            {
                builder.Append(item);
            }

            return builder.Build();
        }
        
        internal static IReadOnlyDictionary<string, ColumnDefinition> InferColumnMap(this IReadOnlyDictionary<string, object> propertyMap)
        {
            var columnMap = new Dictionary<string, ColumnDefinition>();

            foreach (var kvp in propertyMap)
            {
                var meta = SqlMetaData.InferFromValue(kvp.Value, kvp.Key);

                var columnDef = new ColumnDefinition(kvp.Value.GetType(), meta, ColumnBehavior.Nullable);

                columnMap.Add(kvp.Key, columnDef);
            }

            return columnMap;
        }

        internal static SqlDataRecord GetSqlDataRecord(
            this IReadOnlyDictionary<string, object> propertyMap, 
            IReadOnlyDictionary<string, ColumnDefinition> columnMap, 
            TypeMapOption option)
        {
            var metaValuePairs = propertyMap.GetMetaValuePairs(columnMap, option).ToArray();

            var record = new SqlDataRecord(metaValuePairs.Select(p => p.Item1).ToArray());
            
            record.SetValues(metaValuePairs.Select(p => p.Item2).ToArray());

            return record;
        }

        private static IEnumerable<Tuple<SqlMetaData, object>> GetMetaValuePairs(
            this IReadOnlyDictionary<string, object> propertyMap,
            IReadOnlyDictionary<string, ColumnDefinition> columnMap, 
            TypeMapOption option)
        {
            foreach (var kvp in columnMap)
            {
                var columnDef = kvp.Value;

                if (columnDef.IsIgnored) continue;

                var name = kvp.Key;

                if (!propertyMap.TryGetValue(name, out var value))
                {
                    if (columnDef.Behavior != ColumnBehavior.Nullable)
                    {
                        throw new InvalidOperationException($"Column \"{name}\" is required");
                    }
                }

                var effectiveValue = 
                    option == TypeMapOption.Strict 
                        ? value 
                        : value != null 
                            ? Convert.ChangeType(value, columnDef.ScalarType) 
                            : null;
                
                yield return new Tuple<SqlMetaData, object>(columnDef.MetaData, effectiveValue);
            }
        }

        private static IEnumerable<SqlDataRecord> ToSqlRecords<T>(
            this IEnumerable<T> items, 
            TableTypeDefinition def, 
            TypeMapOption option) where T : class
        {
            var columnMap =
                def.Columns
                    .ToDictionary(c => c.MetaData.Name, c => c, StringComparer.OrdinalIgnoreCase);

            return items.ToSqlRecords(columnMap, option);
        }

        private static IEnumerable<SqlDataRecord> ToSqlRecords<T>(
            this IEnumerable<T> items, 
            IReadOnlyDictionary<string, ColumnDefinition> columnMap, 
            TypeMapOption option) where T : class
        {
            return items.Select(item => item.GetPropertyMap().GetSqlDataRecord(columnMap, option));
        }
    }
}
