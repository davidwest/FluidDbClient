﻿using System.Data.Common;

namespace FluidDbClient
{
    public abstract class DbProvider : IDbProvider
    {
        protected DbProvider(string providerId)
        {
            ProviderId = providerId;
        }
        
        public string ProviderId { get; }

        public DbConnection CreateConnection<TDatabase>() where TDatabase : Database
        {
            var database = DbRegistry.GetDatabase<TDatabase>();
            
            return GetConnectionUsing(database.ConnectionString);
        }

        public DbConnection CreateConnection()
        {
            var database = DbRegistry.GetDatabase();

            return GetConnectionUsing(database.ConnectionString);
        }

        public abstract DbConnection CreateConnection(string connectionString);
        
        public abstract DbParameter CreateParameter(string name, object value);

        public abstract IDbProviderValueInterpreter Interpreter { get; }
        
        protected abstract DbConnection GetConnectionUsing(string connectionString);
    }
}
