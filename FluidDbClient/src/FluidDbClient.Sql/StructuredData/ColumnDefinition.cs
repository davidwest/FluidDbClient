using System;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class ColumnDefinition
    {
        public ColumnDefinition(Type type, SqlMetaData metaData, ColumnBehavior behavior, bool isIgnored = false)
        {
            ScalarType = type.GetUnderlyingScalarFieldType();
            MetaData = metaData;
            Behavior = behavior;
            IsIgnored = isIgnored;
        }
        
        public Type ScalarType { get; }
        public SqlMetaData MetaData { get; }
        public ColumnBehavior Behavior { get; }
        public bool IsIgnored { get; }
    }
}
