using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class StructuredDataBuilder
    {
        private readonly string _tableTypeName;
        private readonly List<SqlDataRecord> _records = new List<SqlDataRecord>();

        private readonly SqlMetaData[] _preOrderedMetaData;
        private Dictionary<string, ColumnDefinition> _columnMap;
        
        public StructuredDataBuilder(string tableTypeName, params SqlMetaData[] preOrderedMetaData)
        {
            _tableTypeName = tableTypeName;

            _preOrderedMetaData = preOrderedMetaData;

            _columnMap = 
                preOrderedMetaData
                .Select(md => new ColumnDefinition(md, ColumnBehavior.Nullable))
                .ToDictionary(m => m.MetaData.Name, m => m);
        }

        public StructuredDataBuilder(TableTypeDefinition def)
        {
            _tableTypeName = def.TypeName;

            _columnMap = 
                def.Columns
                .ToDictionary(c => c.MetaData.Name, c => c, StringComparer.OrdinalIgnoreCase);
        }

        public StructuredDataBuilder(TableTypeMap map)
        {
            _tableTypeName = map.TypeName;

            _columnMap = 
                map.PropertyMap
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);
        }
                
        public StructuredDataBuilder Append(IDictionary<string, object> parameters)
        {
            var propertyMap = parameters as Dictionary<string, object> ?? 
                              parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (_columnMap.Count == 0)
            {
                CreateColumnMapFrom(propertyMap);
            }
            
            Append(propertyMap);

            return this;
        }

        public StructuredDataBuilder Append(object parameters)
        {
            var propertyMap = parameters.GetPropertyMap();
            
            if (_columnMap.Count == 0)
            {
                CreateColumnMapFrom(propertyMap);
            }

            Append(propertyMap);

            return this;
        }

        public StructuredDataBuilder AppendValues(params object[] values)
        {
            if (_preOrderedMetaData == null)
            {
                throw new InvalidOperationException("Cannot append values directly to data builder when pre-ordered SQL meta data is undefined");
            }

            if (values.Length != _preOrderedMetaData.Length)
            {
                throw new ArgumentException("Specified values do not match the established SQL meta data", nameof(values));
            }

            var record = new SqlDataRecord(_preOrderedMetaData);

            record.SetValues(values);

            _records.Add(record);

            return this;
        }

        public StructuredData Build()
        {
            // TODO: verify

            return new StructuredData(_tableTypeName, _records);
        }
        
        private void CreateColumnMapFrom(Dictionary<string, object> propertyMap)
        {
            _columnMap = new Dictionary<string, ColumnDefinition>();

            foreach (var kvp in propertyMap)
            {
                var meta = ExtractMetaDataFrom(kvp.Key, kvp.Value);
                if (meta != null)
                {
                    var columnDef = new ColumnDefinition(meta, ColumnBehavior.Nullable);
                    _columnMap.Add(kvp.Key, columnDef);
                }
            }
        }

        private void Append(Dictionary<string, object> propertyMap)
        {
            var columnList = new List<ColumnDefinition>();
            var valueList = new List<object>();

            foreach (var kvp in propertyMap)
            {
                var name = kvp.Key;
                var value = kvp.Value;
                
                if (!_columnMap.TryGetValue(name, out var columnDef))
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

            _records.Add(record);
        }
        
        private static SqlMetaData ExtractMetaDataFrom(string name, object value)
        {
            var sqlType = PrimitiveClrToSqlTypeMap.GetSqlTypeFor(value);

            if (!sqlType.HasValue) return null;

            return sqlType.Value.CanSpecifyLength() 
                ? new SqlMetaData(name, sqlType.Value, -1) 
                : new SqlMetaData(name, sqlType.Value);
        }
    }
}
