using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public partial class ManagedDbCommand
    {
        public async Task ExecuteAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            try
            {
                await CreateResourcesAsync(isolationLevel);

                await Command.ExecuteNonQueryAsync();

                Commit();
            }
            finally
            {
                DisposeResources();
            }
        }

        private async Task CreateResourcesAsync(IsolationLevel isolationLevel)
        {
            OnOperationStarted();

            await CreateConnectionAsync();

            await CreateTransactionAsync(isolationLevel);

            CreateCommand();
        }

        private async Task CreateTransactionAsync(IsolationLevel isolationLevel)
        {
            if (Transaction != null) return;

            if (!Connection.State.HasFlag(ConnectionState.Open))
            {
                await Connection.OpenAsync();
                Log("Opened DbConnection Async");
            }

            Transaction = Connection.BeginTransaction(isolationLevel);

            Log("Created DbTransaction");
        }
    }
}
