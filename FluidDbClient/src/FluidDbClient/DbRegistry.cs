using System;
using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient
{
    public static class DbRegistry
    {
        private static Database _defaultDatabase;
        private static IReadOnlyDictionary<Type, Database> _databases;

        private static bool _isInitialized;

        public static void Initialize(params Database[] databases)
        {
            if (databases.Length == 0) return;

            if (_isInitialized)
            {
                throw new InvalidOperationException("Db Registry is already initialized");
            }

            _defaultDatabase = databases[0];
            _databases = databases.ToDictionary(db => db.GetType(), db => db);

            _isInitialized = true;
        }

        public static Database GetDatabase()
        {
            return _defaultDatabase;
        }

        public static Database GetDatabase<TDatabase>() where TDatabase : Database
        {
            return _databases[typeof(TDatabase)];
        }   
    }
}
