using System;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public abstract partial class ManagedDbQuery
    {
        public async Task<T> GetScalarAsync<T>(T dbNullsubstitute = default(T))
        {
            try
            {           
                await EstablishCommonResourcesAsync();
                
                var val = await Command.ExecuteScalarAsync();

                return val.DbCast(dbNullsubstitute);
            }
            finally
            {
                ReleaseResources();
            }
        }


        public async Task<IDataRecord> GetRecordAsync()
        {
            try
            {
                await CreateReaderResourcesAsync(CommandBehavior.SingleRow);

                if (await _reader.ReadAsync())
                {
                    return _reader.Copy();
                }

                return null;
            }
            finally
            {
                ReleaseResources();
            }
        }


        public async Task ProcessResultSetAsync(Action<IDataRecord> process)
        {
            await ProcessResultSetsAsync(process);
        }
        

        public async Task ProcessResultSetsAsync(params Action<IDataRecord>[] processes)
        {
            if (processes.Length == 0) return;

            var isSingleProcess = processes.Length == 1;

            try
            {
                await CreateReaderResourcesAsync(isSingleProcess ? CommandBehavior.SingleResult : CommandBehavior.Default);

                for (var i = 0; i != processes.Length; i++)
                {
                    while (await _reader.ReadAsync())
                    {
                        processes[i](_reader);
                    }

                    if (!isSingleProcess)
                    {
                        await _reader.NextResultAsync();
                    }
                }
            }
            finally
            {
                ReleaseResources();
            }
        }
        
        
        private async Task EstablishCommonResourcesAsync()
        {
            OnOperationStarted();

            await EstablishConnectionAsync();

            CreateCommand();
        }


        private async Task CreateReaderResourcesAsync(CommandBehavior readBehavior)
        {
            await EstablishCommonResourcesAsync();
            
            await CreateReaderAsync(readBehavior);
        }
    
        
        private async Task CreateReaderAsync(CommandBehavior readBehavior)
        {
            _reader = await Command.ExecuteReaderAsync(readBehavior);

            Log("Created DbReader Async");
        }
    }
}

