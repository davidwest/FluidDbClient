using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class StructuredDataBuilder
    {
        private readonly string _tableTypeName;
        private readonly List<object[]> _rows = new List<object[]>();

        private List<SqlMetaData> _metaData;

        public StructuredDataBuilder(string tableTypeName, params SqlMetaData[] metaData)
        {
            _tableTypeName = tableTypeName;

            _metaData = metaData.ToList();
        }
                
        public StructuredDataBuilder AppendValues(params object[] values)
        {
            _rows.Add(values);
            return this;
        }

        public StructuredDataBuilder Append(IDictionary<string, object> parameters)
        {
            var propertyMap = parameters.GetPropertyMap();

            if (_metaData.Count == 0)
            {
                DefineMetaDataFrom(propertyMap);
            }

            Append(propertyMap);

            return this;
        }

        public StructuredDataBuilder Append(object parameters)
        {
            var propertyMap = parameters.GetPropertyMap();
            
            if (_metaData.Count == 0)
            {
                DefineMetaDataFrom(propertyMap);
            }

            Append(propertyMap);

            return this;
        }

        public StructuredData Build()
        {
            if (_rows.Count == 0)
            {
                return new StructuredData(_tableTypeName, new SqlDataRecord[0]);
            }

            var rowsGroupedByLength = _rows.ToLookup(r => r.Length);

            if (rowsGroupedByLength.Count > 1)
            {
                throw new InvalidOperationException("All data rows must have the same field count");
            }

            var rowWidth = rowsGroupedByLength.First().Key;

            if (rowWidth != _metaData.Count)
            {
                throw new InvalidOperationException("Meta data size does not match data rows field count");
            }

            var records = new List<SqlDataRecord>();
            var metaDataArray = _metaData.ToArray();

            foreach (var row in _rows)
            {
                var record = new SqlDataRecord(metaDataArray);

                record.SetValues(row);

                records.Add(record);
            }

            var result = new StructuredData(_tableTypeName, records);

            return result;
        }

        private void DefineMetaDataFrom(Dictionary<string, object> propertyMap)
        {
            _metaData = propertyMap.Select(kvp => GetMetaDataFrom(kvp.Key, kvp.Value)).ToList();
        }


        private void Append(Dictionary<string, object> propertyMap)
        {
            var currentMap = new Dictionary<int, object>();

            foreach (var kvp in propertyMap)
            {
                var name = kvp.Key;
                var value = kvp.Value;

                var metaData = _metaData.FirstOrDefault(md => md.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (metaData == null)
                {
                    throw new InvalidOperationException($"Could not find SqlMetaData with name = {name}");
                }

                var index = _metaData.IndexOf(metaData);

                currentMap.Add(index, value);
            }

            var ordered = 
                currentMap
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value)
                .ToArray();

            AppendValues(ordered);
        }

        private static SqlMetaData GetMetaDataFrom(string name, object value)
        {
            var sqlType = PrimitiveClrToSqlTypeMap.GetSqlTypeFor(value);

            if (!sqlType.HasValue)
            {
                throw new ArgumentException("Cannot infer SqlDbType from value", nameof(value));
            }

            if (sqlType == SqlDbType.NVarChar || sqlType == SqlDbType.Binary)
            {
                return new SqlMetaData(name, sqlType.Value, -1);
            }

            return new SqlMetaData(name, sqlType.Value);
        }
    }
}
