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
                OnOperationStarted();

                await EstablishResourcesAsync(isolationLevel);

                await Command.ExecuteNonQueryAsync();

                TryCommit();
            }
            finally
            {
                ReleaseResources();
            }
        }
        
        private async Task EstablishResourcesAsync(IsolationLevel isolationLevel)
        {
            await EstablishConnectionAsync();

            EstablishTransaction(isolationLevel);

            CreateCommand();
        }
    }
}
