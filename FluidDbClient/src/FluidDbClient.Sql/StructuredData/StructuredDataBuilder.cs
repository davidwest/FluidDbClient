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
        private readonly SqlMetaData[] _orderedMetaData;

        private Dictionary<string, SqlMetaData> _metaMap;
        
        public StructuredDataBuilder(string tableTypeName, params SqlMetaData[] orderedMetaData)
        {
            _tableTypeName = tableTypeName;

            _orderedMetaData = orderedMetaData;
            _metaMap = orderedMetaData.ToDictionary(m => m.Name, m => m);
        }

        public StructuredDataBuilder(TableTypeDefinition def)
        {
            _tableTypeName = def.TypeName;

            _orderedMetaData = 
                def.Columns
                .Select(c => c.MetaData)
                .OrderBy(md => md.SortOrdinal)
                .ToArray();

            _metaMap = 
                def.Columns
                .ToDictionary(c => c.MetaData.Name, c => c.MetaData, StringComparer.OrdinalIgnoreCase);
        }

        public StructuredDataBuilder(TableTypeMap map)
        {
            _tableTypeName = map.TypeName;

            _orderedMetaData = 
                map.GetDefinition().Columns
                .Select(c => c.MetaData)
                .OrderBy(md => md.SortOrdinal)
                .ToArray();

            _metaMap = 
                map.PropertyMap
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.MetaData, StringComparer.OrdinalIgnoreCase);
        }
                
        public StructuredDataBuilder Append(IDictionary<string, object> parameters)
        {
            var propertyMap = parameters as Dictionary<string, object> ?? 
                              parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (_metaMap.Count == 0)
            {
                DefineMetaMapFrom(propertyMap);
            }
            
            Append(propertyMap);

            return this;
        }

        public StructuredDataBuilder Append(object parameters)
        {
            var propertyMap = parameters.GetPropertyMap();
            
            if (_metaMap.Count == 0)
            {
                DefineMetaMapFrom(propertyMap);
            }

            Append(propertyMap);

            return this;
        }

        public StructuredDataBuilder AppendValues(params object[] values)
        {
            if (_orderedMetaData == null)
            {
                throw new InvalidOperationException("Cannot append values directly to data builder when ordered SQL meta data is undefined");
            }

            if (values.Length != _orderedMetaData.Length)
            {
                throw new ArgumentException("Specified values do not match the established SQL meta data", nameof(values));
            }

            var record = new SqlDataRecord(_orderedMetaData);

            record.SetValues(values);

            _records.Add(record);

            return this;
        }

        public StructuredData Build()
        {
            return new StructuredData(_tableTypeName, _records);
        }
        

        private void DefineMetaMapFrom(Dictionary<string, object> propertyMap)
        {
            _metaMap =
                propertyMap
                .ToDictionary(kvp => kvp.Key, kvp => ExtractMetaDataFrom(kvp.Key, kvp.Value));
        }

        private void Append(Dictionary<string, object> propertyMap)
        {
            var metaList = new List<SqlMetaData>();
            var valueList = new List<object>();

            foreach (var kvp in propertyMap)
            {
                var name = kvp.Key;
                var value = kvp.Value;

                SqlMetaData metaData;
                if (!_metaMap.TryGetValue(name, out metaData))
                {
                    continue;
                }
                
                metaList.Add(metaData);
                valueList.Add(value);
            }

            if (metaList.Count != valueList.Count)
            {
                throw new InvalidOperationException("Specified properties do not match the established SQL meta data");
            }

            var record = new SqlDataRecord(metaList.ToArray());
            record.SetValues(valueList.ToArray());

            _records.Add(record);
        }
        
        private static SqlMetaData ExtractMetaDataFrom(string name, object value)
        {
            var sqlType = PrimitiveClrToSqlTypeMap.GetSqlTypeFor(value);

            if (!sqlType.HasValue)
            {
                throw new ArgumentException($"Cannot infer SqlDbType from value of type {value.GetType().Name}", nameof(value));
            }

            return sqlType.Value.CanSpecifyLength() 
                ? new SqlMetaData(name, sqlType.Value, -1) 
                : new SqlMetaData(name, sqlType.Value);
        }
    }
}
