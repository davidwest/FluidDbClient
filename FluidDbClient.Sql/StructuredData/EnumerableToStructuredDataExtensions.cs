using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient.Sql
{
    public static class EnumerableToStructuredDataExtensions
    {
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            DataBindingOptions bindingOptions = DataBindingOptions.Default) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap<T>();

            return tableTypeMap != null 
                ? items.ToStructuredData(tableTypeMap) 
                : items.ToStructuredData(typeof(T).Name, bindingOptions);
        }
        
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            TableTypeMap map, 
            DataBindingOptions bindingOptions = DataBindingOptions.Default) where T : class
        {
            return items.ToStructuredData(map.GetDefinition(), bindingOptions);
        }
        
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            TableTypeDefinition def, 
            DataBindingOptions bindingOptions = DataBindingOptions.Default) 
            where T : class
        {
            return new StructuredData(def.TypeName, items.ToSqlRecords(def, bindingOptions));
        }
        
        public static StructuredData ToStructuredData<T>(
            this IEnumerable<T> items, 
            string tableTypeName, 
            DataBindingOptions bindingOptions = DataBindingOptions.Default) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap(tableTypeName);

            if (tableTypeMap != null)
            {
                return items.ToStructuredData(tableTypeMap, bindingOptions);
            }

            // fall back to using a builder, which allows inference:
            var builder = new StructuredDataBuilder(tableTypeName, bindingOptions);

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
                var meta = Microsoft.Data.SqlClient.Server.SqlMetaData.InferFromValue(kvp.Value, kvp.Key);

                var columnDef = new ColumnDefinition(kvp.Value.GetType(), meta, ColumnBehavior.Nullable);

                columnMap.Add(kvp.Key, columnDef);
            }

            return columnMap;
        }
        
        internal static SqlDataRecord GetSqlDataRecord(
            this IReadOnlyDictionary<string, object> propertyMap, 
            IReadOnlyDictionary<string, ColumnDefinition> columnMap, 
            DataBindingOptions bindingOptions)
        {
            var metaValuePairs = propertyMap.GetMetaValuePairs(columnMap, bindingOptions).ToArray();

            var record = new SqlDataRecord(metaValuePairs.Select(p => p.Item1).ToArray());
            
            record.SetValues(metaValuePairs.Select(p => p.Item2).ToArray());

            return record;
        }

        private static IEnumerable<Tuple<SqlMetaData, object>> GetMetaValuePairs(
            this IReadOnlyDictionary<string, object> propertyMap,
            IReadOnlyDictionary<string, ColumnDefinition> columnMap, 
            DataBindingOptions bindingOptions)
        {
            return 
                from kvp in columnMap
                select kvp.Value into columnDef
                where !columnDef.IsIgnored
                select GetMetaValuePair(propertyMap, columnDef, bindingOptions);
        }

        private static Tuple<SqlMetaData, object> GetMetaValuePair(
            this IReadOnlyDictionary<string, object> propertyMap, 
            ColumnDefinition columnDef, 
            DataBindingOptions bindingOptions)
        {
            var columnName = columnDef.MetaData.Name;

            if (!propertyMap.TryGetValue(columnName, out var value))
            {
                if (columnDef.Behavior != ColumnBehavior.Nullable)
                {
                    if (bindingOptions.HasFlag(DataBindingOptions.AllowMissingProperties))
                    {
                        if (columnDef.ScalarType.IsValueType)
                        {
                            value = Activator.CreateInstance(columnDef.ScalarType);
                        }
                        else
                        {
                            throw new NotImplementedException("Cannot create object with default value");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"Column \"{columnName}\" is required");
                    }
                }
            }

            var effectiveValue =
                !bindingOptions.HasFlag(DataBindingOptions.CoerceTypes)
                    ? value
                    : value != null
                        ? Convert.ChangeType(value, columnDef.ScalarType)
                        : null;

            return new Tuple<SqlMetaData, object>(columnDef.MetaData, effectiveValue);
        }
        
        private static IEnumerable<SqlDataRecord> ToSqlRecords<T>(
            this IEnumerable<T> items, 
            TableTypeDefinition def, 
            DataBindingOptions bindingOptions) where T : class
        {
            var columnMap =
                def.Columns
                    .ToDictionary(c => c.MetaData.Name, c => c, StringComparer.OrdinalIgnoreCase);

            return items.ToSqlRecords(columnMap, bindingOptions);
        }

        private static IEnumerable<SqlDataRecord> ToSqlRecords<T>(
            this IEnumerable<T> items, 
            IReadOnlyDictionary<string, ColumnDefinition> columnMap, 
            DataBindingOptions bindingOptions) where T : class
        {
            return items.Select(item => item.GetPropertyMap().GetSqlDataRecord(columnMap, bindingOptions));
        }
    }
}
