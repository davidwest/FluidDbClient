using System;
using System.Data;

namespace FluidDbClient.Sql
{
    public class TableTypePropertyConfiguration
    {
        private readonly string _propertyName;
        private readonly ColumnDefinition _columnDef;
        private readonly Action<string, ColumnDefinition> _onChange;

        internal TableTypePropertyConfiguration(string propertyName, ColumnDefinition columnDef, Action<string, ColumnDefinition> onChange)
        {
            _propertyName = propertyName;
            _columnDef = columnDef;
            _onChange = onChange;

            onChange(propertyName, columnDef);
        }
        
        public TableTypePropertyConfiguration Ignore()
        {
            var meta = _columnDef.MetaData;

            var newMeta = SqlMetaDataFactory.CreateSqlMetaData(meta.Name, meta.SqlDbType, meta.MaxLength, meta.IsUniqueKey, meta.SortOrdinal);

            var newColumnDef = new ColumnDefinition(newMeta, _columnDef.Behavior, true);

            return new TableTypePropertyConfiguration(_propertyName, newColumnDef, _onChange);
        }

        public TableTypePropertyConfiguration HasName(string name)
        {
            // TODO: name validation

            var meta = _columnDef.MetaData;

            var newMeta = SqlMetaDataFactory.CreateSqlMetaData(name, meta.SqlDbType, meta.MaxLength, meta.IsUniqueKey, meta.SortOrdinal);

            var newColumnDef = new ColumnDefinition(newMeta, _columnDef.Behavior);

            return new TableTypePropertyConfiguration(_propertyName, newColumnDef, _onChange);
        }

        public TableTypePropertyConfiguration HasSqlType(SqlDbType type)
        {
            var meta = _columnDef.MetaData;

            var newMeta = SqlMetaDataFactory.CreateSqlMetaData(meta.Name, type, meta.MaxLength, meta.IsUniqueKey, meta.SortOrdinal);

            var newColumnDef = new ColumnDefinition(newMeta, _columnDef.Behavior);

            return new TableTypePropertyConfiguration(_propertyName, newColumnDef, _onChange);
        }

        public TableTypePropertyConfiguration HasLength(int? size = null)
        {
            var meta = _columnDef.MetaData;

            var encodedSize = size ?? -1;
            
            var newMeta = SqlMetaDataFactory.CreateSqlMetaData(meta.Name, meta.SqlDbType, encodedSize, meta.IsUniqueKey, meta.SortOrdinal);

            var newColumnDef = new ColumnDefinition(newMeta, _columnDef.Behavior);

            return new TableTypePropertyConfiguration(_propertyName, newColumnDef, _onChange);
        }
        
        public TableTypePropertyConfiguration IsInUniqueKey()
        {
            var meta = _columnDef.MetaData;

            var newMeta = SqlMetaDataFactory.CreateSqlMetaData(meta.Name, meta.SqlDbType, meta.MaxLength, true, meta.SortOrdinal);

            var newColumnDef = new ColumnDefinition(newMeta, _columnDef.Behavior);

            return new TableTypePropertyConfiguration(_propertyName, newColumnDef, _onChange);
        }
        
        public TableTypePropertyConfiguration HasBehavior(ColumnBehavior behavior)
        {
            var newColumnDef = new ColumnDefinition(_columnDef.MetaData, behavior);

            return new TableTypePropertyConfiguration(_propertyName, newColumnDef, _onChange);
        }
    }
}
