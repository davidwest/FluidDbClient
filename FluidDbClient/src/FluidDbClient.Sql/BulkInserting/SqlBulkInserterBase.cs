using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FluidDbClient.Sql
{
    public abstract class SqlBulkInserterBase
    {
        private readonly SqlBulkInsertConfig _config;
        
        protected SqlBulkInserterBase(SqlBulkInsertConfig config = null)
        {
            _config = config ?? new SqlBulkInsertConfig();
        }

        public void Insert<T>(IEnumerable<T> items, string tableName = null) where T : class
        {
            var table = items.ToDataTable(tableName);

            Insert(table, tableName);
        }

        public async Task InsertAsync<T>(IEnumerable<T> items, string tableName = null) where T : class
        {
            var table = items.ToDataTable(tableName);

            await InsertAsync(table, tableName);
        }

        public void Insert(DataTable table, string tableName = null)
        {
            tableName = tableName ?? table.TableName;

            using (var copier = GetCopier(tableName))
            {
                copier.WriteToServer(table);
            }
        }

        public async Task InsertAsync(DataTable table, string tableName = null)
        {
            tableName = tableName ?? table.TableName;

            using (var copier = GetCopier(tableName))
            {
                await copier.WriteToServerAsync(table);
            }
        }

        private SqlBulkCopy GetCopier(string tableName)
        {
            var copier = new SqlBulkCopy(GetConnectionString(), _config.Options);

            foreach (var handlers in _config.CopiedEventHandlers)
            {
                copier.SqlRowsCopied += handlers;
            }

            copier.DestinationTableName = tableName;
            copier.BatchSize = _config.BatchSize;
            copier.BulkCopyTimeout = _config.Timeout;
            copier.NotifyAfter = _config.NotifyAfter;

            return copier;
        }
        
        protected abstract string GetConnectionString();
    }
}
