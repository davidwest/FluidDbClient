﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public static class EnumerableToStructuredDataExtensions
    {
        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap<T>();

            return tableTypeMap != null 
                ? items.ToStructuredData(tableTypeMap) 
                : items.ToStructuredData(typeof(T).Name);
        }

        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, TableTypeMap map) where T : class
        {
            return items.ToStructuredData(map.GetDefinition());
        }
        
        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, TableTypeDefinition def) where T : class
        {
            return new StructuredData(def.TypeName, items.ToSqlRecords(def));
        }

        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, string tableTypeName) where T : class
        {
            var tableTypeMap = TableTypeRegistry.GetMap(tableTypeName);

            if (tableTypeMap != null)
            {
                return items.ToStructuredData(tableTypeMap);
            }

            // fall back to using a builder, which allows inference:
            var builder = new StructuredDataBuilder(tableTypeName);

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

                var columnDef = new ColumnDefinition(meta, ColumnBehavior.Nullable);

                columnMap.Add(kvp.Key, columnDef);
            }

            return columnMap;
        }

        internal static SqlDataRecord GetSqlDataRecord(
            this IReadOnlyDictionary<string, object> propertyMap, 
            IReadOnlyDictionary<string, ColumnDefinition> columnMap)
        {
            var columnList = new List<ColumnDefinition>();
            var valueList = new List<object>();

            foreach (var kvp in propertyMap)
            {
                var name = kvp.Key;
                var value = kvp.Value;

                if (!columnMap.TryGetValue(name, out var columnDef))
                {
                    continue;
                }

                if (columnDef.IsIgnored)
                {
                    continue;
                }

                columnList.Add(columnDef);
                valueList.Add(value);
            }

            var metaArray = columnList.Select(c => c.MetaData).ToArray();

            var record = new SqlDataRecord(metaArray);

            record.SetValues(valueList.ToArray());

            return record;
        }

        private static IEnumerable<SqlDataRecord> ToSqlRecords<T>(this IEnumerable<T> items, TableTypeDefinition def) where T : class
        {
            var columnMap =
                def.Columns
                    .ToDictionary(c => c.MetaData.Name, c => c, StringComparer.OrdinalIgnoreCase);

            return items.ToSqlRecords(columnMap);
        }

        private static IEnumerable<SqlDataRecord> ToSqlRecords<T>(this IEnumerable<T> items, IReadOnlyDictionary<string, ColumnDefinition> columnMap) where T : class
        {
            return items.Select(item => item.GetPropertyMap().GetSqlDataRecord(columnMap));
        }
    }
}
