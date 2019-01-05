namespace FluidDbClient.Sql
{
    public class SqlBulkInserter<TDatabase> : SqlBulkInserterBase where TDatabase : Database
    {
        public SqlBulkInserter(SqlBulkInsertConfig config = null) : base(config)
        { }
        
        protected override string GetConnectionString() => DbRegistry.GetDatabase<TDatabase>().ConnectionString;
    }

    public class SqlBulkInserter: SqlBulkInserterBase
    {
        public SqlBulkInserter(SqlBulkInsertConfig config = null) : base(config)
        { }

        protected override string GetConnectionString() => DbRegistry.GetDatabase().ConnectionString;
    }
}
