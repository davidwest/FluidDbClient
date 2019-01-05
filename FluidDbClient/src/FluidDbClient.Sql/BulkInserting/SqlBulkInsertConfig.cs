using System.Data.SqlClient;

namespace FluidDbClient.Sql
{
    public class SqlBulkInsertConfig
    {
        private const SqlBulkCopyOptions OptionsDefault =
            SqlBulkCopyOptions.Default | SqlBulkCopyOptions.UseInternalTransaction;

        public SqlBulkInsertConfig(
            SqlBulkCopyOptions options,
            int batchSize, 
            int timeout, 
            int notifyAfter, 
            params SqlRowsCopiedEventHandler[] copiedEventHandlers)
        {
            Options = options;
            BatchSize = batchSize >= 0 ? batchSize : 0;
            Timeout = timeout >= 0 ? timeout : 0;
            NotifyAfter = notifyAfter >= 0 ? notifyAfter : 0;

            CopiedEventHandlers = copiedEventHandlers;
        }
        
        public SqlBulkInsertConfig(SqlBulkCopyOptions options, int batchSize, int timeout = 0) 
            : this(options, batchSize, timeout, 0)
        { }

        public SqlBulkInsertConfig(SqlBulkCopyOptions options = OptionsDefault) 
            : this(options, 0)
        { }

        public SqlBulkInsertConfig(
            int batchSize,
            int timeout,
            int notifyAfter,
            params SqlRowsCopiedEventHandler[] copiedEventHandlers) 
            : this(OptionsDefault, batchSize, timeout, notifyAfter, copiedEventHandlers)
        { }

        public SqlBulkInsertConfig(int batchSize, int timeout = 0)
            : this(OptionsDefault, batchSize, timeout, 0)
        { }

        public SqlBulkCopyOptions Options { get;  }

        public int BatchSize { get; }

        public int Timeout { get; }

        public int NotifyAfter { get; }
        
        public SqlRowsCopiedEventHandler[] CopiedEventHandlers { get; }

        public bool IsBatching => BatchSize > 0;

        public bool HasTimeout => Timeout > 0;

        public bool GeneratesNotifications => NotifyAfter > 0;
    }
}
