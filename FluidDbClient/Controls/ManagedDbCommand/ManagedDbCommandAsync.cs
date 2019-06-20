using System;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public partial class ManagedDbCommand
    {
        public async Task ExecuteAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            await ExecuteAsync(() => EstablishResourcesAsync(isolationLevel));
        }
        
        public async Task ExecuteWithoutTransactionAsync()
        {
            await ExecuteAsync(EstablishResourcesNoTransactionAsync);
        }
        
        private async Task ExecuteAsync(Func<Task> setup)
        {
            try
            {
                OnOperationStarted();

                await setup();

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

        private async Task EstablishResourcesNoTransactionAsync()
        {
            await EstablishConnectionAsync();

            CreateCommand();
        }
    }
}
