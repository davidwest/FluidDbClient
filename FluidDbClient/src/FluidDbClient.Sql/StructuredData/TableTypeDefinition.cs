using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public enum ColumnBehavior
    {
        Nullable,
        NotNullable,
        PrimaryKeyComponent
    }
    
    public class ColumnDefinition
    {
        public ColumnDefinition(SqlMetaData metaData, ColumnBehavior behavior, bool isIgnored = false)
        {
            MetaData = metaData;
            Behavior = behavior;
            IsIgnored = isIgnored;
        }

        public SqlMetaData MetaData { get; }
        public ColumnBehavior Behavior { get; }
        public bool IsIgnored { get; }
    }
    
    public class TableTypeDefinition
    {
        public TableTypeDefinition(string typeName, ColumnDefinition[] columns)
        {
            TypeName = typeName;
            Columns = columns;
        }

        public string TypeName { get; }
        public ColumnDefinition[] Columns { get; }
    }
}
