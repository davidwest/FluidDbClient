namespace FluidDbClient.Sql
{
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
