﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace FluidDbClient
{
    public abstract partial class ManagedDbQuery : ManagedDbControl, IManagedDbQuery
    {
        private readonly bool _usingExternalSession;
        private DbDataReader _reader;

        protected ManagedDbQuery(Database database, DbSessionBase session, object parameters)
            : base(database, session, parameters)
        {
            _usingExternalSession = session != null;
        }
        
        public T GetScalar<T>(T dbNullSubstitute = default(T))
        {
            try
            {
                CreateCommonResources();

                if (!Connection.State.HasFlag(ConnectionState.Open))
                {
                    Connection.Open();
                }

                var val = Command.ExecuteScalar();

                return val.DbCast(dbNullSubstitute);
            }
            finally
            {
                DisposeResources();
            }
        }

        public IDataRecord GetRecord()
        {
            return GetResultSet(CommandBehavior.SingleRow, dr => dr.Copy()).FirstOrDefault();
        }

        public IEnumerable<IDataRecord> GetResultSet()
        {
            return GetResultSet(CommandBehavior.SingleResult, dr => dr);
        }
                
        public void ProcessResultSets(params Action<IEnumerable<IDataRecord>>[] processes)
        {
            if (processes.Length == 0) return;

            var isSingleProcess = processes.Length == 1;

            try
            {
                CreateReaderResources(processes.Length == 1 ? CommandBehavior.SingleResult : CommandBehavior.Default);

                for (var i = 0; i != processes.Length; i++)
                {
                    processes[i](YieldRecords());

                    if (!isSingleProcess)
                    {
                        _reader.NextResult();
                    }
                }
            }
            finally
            {
                DisposeResources();
            }
        }

        
        private IEnumerable<IDataRecord> GetResultSet(CommandBehavior readBehavior, Func<IDataRecord, IDataRecord> yieldRecord)
        {
            try
            {
                CreateReaderResources(readBehavior);

                while (_reader.Read())
                {
                    yield return yieldRecord(_reader);
                }
            }
            finally
            {
                DisposeResources();
            }
        }

        
        private void CreateCommonResources()
        {
            OnOperationStarted();

            CreateConnection();
            CreateCommand();
        }

        private void CreateReaderResources(CommandBehavior readBehavior)
        {
            CreateCommonResources();
            CreateReader(readBehavior);
        }

        private void DisposeResources()
        {
            DisposeReader();
            DisposeCommand();
            ReleaseConnection(Transaction != null);
        }

        private void CreateReader(CommandBehavior readBehavior)
        {
            if (!Connection.State.HasFlag(ConnectionState.Open))
            {
                Connection.Open();
                Log("Opened DbConnection");
            }

            _reader = Command.ExecuteReader(readBehavior);

            Log("Created DbReader");
        }

        private void DisposeReader()
        {
            if (_reader == null) return;

            _reader.Dispose();
            _reader = null;

            Log("Disposed DbReader");
        }

        private IEnumerable<IDataRecord> YieldRecords()
        {
            while (_reader.Read())
            {
                yield return _reader;
            }
        }
    }
}
