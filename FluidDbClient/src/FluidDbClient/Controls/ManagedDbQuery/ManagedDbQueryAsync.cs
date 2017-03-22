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
                if (_usingExternalSession)
                {
                    await CreateCommonResourcesAsync();
                }
                else
                {
                    CreateCommonResources();
                }
                
                if (!Connection.State.HasFlag(ConnectionState.Open))
                {
                    await Connection.OpenAsync();
                }

                var val = await Command.ExecuteScalarAsync();

                return val.DbCast(dbNullsubstitute);
            }
            finally
            {
                DisposeResources();
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
                DisposeResources();
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
                DisposeResources();
            }
        }
        
        
        private async Task CreateCommonResourcesAsync()
        {
            OnOperationStarted();

            await CreateConnectionAsync();

            CreateCommand();
        }


        private async Task CreateReaderResourcesAsync(CommandBehavior readBehavior)
        {
            if (_usingExternalSession)
            {
                await CreateCommonResourcesAsync();
            }
            else
            {
                CreateCommonResources();
            }
            
            await CreateReaderAsync(readBehavior);
        }
    
        
        private async Task CreateReaderAsync(CommandBehavior readBehavior)
        {
            if (!Connection.State.HasFlag(ConnectionState.Open))
            {
                await Connection.OpenAsync();
                Log("Opened DbConnection Async");
            }

            _reader = await Command.ExecuteReaderAsync(readBehavior);

            Log("Created DbReader Async");
        }
    }
}

