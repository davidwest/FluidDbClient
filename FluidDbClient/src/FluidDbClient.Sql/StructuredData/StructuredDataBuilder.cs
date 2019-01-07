﻿using System;
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
        private IReadOnlyDictionary<string, ColumnDefinition> _columnMap;
        
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

        public StructuredDataBuilder(TableTypeMap map) : this(map.GetDefinition())
        { }
                
        public StructuredDataBuilder Append(IReadOnlyDictionary<string, object> propertyMap)
        {
            if (_columnMap.Count == 0)
            {
                _columnMap = propertyMap.InferColumnMap();
            }

            var record = propertyMap.GetSqlDataRecord(_columnMap);

            _records.Add(record);

            return this;
        }
        
        public StructuredDataBuilder Append(object item)
        {
            return Append(item.GetPropertyMap());
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
            return new StructuredData(_tableTypeName, _records);
        }
    }
}
