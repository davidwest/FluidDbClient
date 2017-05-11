﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace FluidDbClient
{
    public abstract partial class ManagedDbQuery : ManagedDbControl, IManagedDbQuery
    {
        private readonly bool _usingExternalResources;
        private DbDataReader _reader;

        protected ManagedDbQuery(Database database, object parameters) 
            : base(database, parameters)
        { }

        protected ManagedDbQuery(Database database, DbSessionBase session, object parameters)
            : base(database, session, parameters)
        {
            _usingExternalResources = true;
        }

        protected ManagedDbQuery(Database database, DbConnection connection, object parameters)
            : base(database, connection, parameters)
        {
            _usingExternalResources = true;
        }

        protected ManagedDbQuery(Database database, DbTransaction transaction, object parameters)
            : base(database, transaction, parameters)
        {
            _usingExternalResources = true;
        }

        public T GetScalar<T>(T dbNullSubstitute = default(T))
        {
            try
            {
                EstablishCommonResources();
                
                var val = Command.ExecuteScalar();

                return val.DbCast(dbNullSubstitute);
            }
            finally
            {
                ReleaseResources();
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
                EstablishReaderResources(processes.Length == 1 ? CommandBehavior.SingleResult : CommandBehavior.Default);

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
                ReleaseResources();
            }
        }

        
        private IEnumerable<IDataRecord> GetResultSet(CommandBehavior readBehavior, Func<IDataRecord, IDataRecord> yieldRecord)
        {
            try
            {
                EstablishReaderResources(readBehavior);

                while (_reader.Read())
                {
                    yield return yieldRecord(_reader);
                }
            }
            finally
            {
                ReleaseResources();
            }
        }

        
        private void EstablishCommonResources()
        {
            OnOperationStarted();

            EstablishConnection();
            CreateCommand();
        }

        private void EstablishReaderResources(CommandBehavior readBehavior)
        {
            EstablishCommonResources();
            CreateReader(readBehavior);
        }

        private void ReleaseResources()
        {
            DisposeReader();
            DisposeCommand();
            ReleaseConnection(_usingExternalResources);
        }

        private void CreateReader(CommandBehavior readBehavior)
        {
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
