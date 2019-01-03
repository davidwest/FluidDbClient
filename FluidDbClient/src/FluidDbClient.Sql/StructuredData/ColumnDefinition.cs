using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
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
}
