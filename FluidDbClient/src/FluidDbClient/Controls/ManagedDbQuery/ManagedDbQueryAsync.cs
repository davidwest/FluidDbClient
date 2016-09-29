
using System;
using System.Collections.Generic;
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

        public async Task<List<IDataRecord>> CollectResultSetAsync()
        {
            var resultSets = await CollectResultSetsAsync(1);

            return resultSets[0];
        }

        public async Task<List<T>> CollectResultSetAsync<T>(Func<IDataRecord, T> map)
        {
            var resultSets = await CollectResultSetsAsync(1, map);

            return resultSets[0];
        }

        public async Task<List<dynamic>> CollectResultSetDynamicAsync()
        {
            var resultSets = await CollectResultSetsDynamicAsync(1);

            return resultSets[0];
        }


        public async Task ProcessResultSetsAsync(params Action<IDataRecord>[] processes)
        {
            await ProcessResultSetsAsync(rec => rec, processes);
        }
        
        public async Task<List<IDataRecord>[]> CollectResultSetsAsync(int resultCount)
        {
            return await CollectResultSetsAsync(resultCount, dr => new DataRecord(dr) as IDataRecord);
        }

        public async Task<List<dynamic>[]> CollectResultSetsDynamicAsync(int resultCount)
        {
            return await CollectResultSetsAsync(resultCount, dr => dr.ToDynamic());
        }



        private async Task<List<T>[]> CollectResultSetsAsync<T>(int resultCount, Func<IDataRecord, T> copy)
        {
            var sets = new List<T>[resultCount];

            var processes = new Action<T>[resultCount];

            for (var i = 0; i != resultCount; i++)
            {
                var index = i;
                sets[index] = new List<T>();
                processes[i] = data => sets[index].Add(data);
            }

            await ProcessResultSetsAsync(copy, processes);

            return sets;
        }

        
        private async Task ProcessResultSetsAsync<T>(Func<IDataRecord, T> yield, params Action<T>[] processes)
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
                        processes[i](yield(_reader));
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

