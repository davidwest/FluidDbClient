using System;
using System.Data.Common;

namespace FluidDbClient
{
    public class DbProvider : IDbProvider
    {
        private readonly string _connectionString;
        private readonly Func<string, DbConnection> _createConnection;
        private readonly Func<string, object, DbParameter> _createParameter;
        private readonly Func<DbConnection, DbCommand> _createCommand;
        
        public DbProvider(
            string providerId, 
            string connectionString, 
            Func<string, DbConnection> createConnection, 
            Func<string, object, DbParameter> createParameter, 
            Func<DbConnection, DbCommand> createCommand = null)
        {
            ProviderId = providerId;

            _connectionString = connectionString;
            _createConnection = createConnection;
            _createParameter = createParameter;
            _createCommand = createCommand ?? (conn => conn.CreateCommand());
        }
        
        public string ProviderId { get; }

        public DbConnection CreateConnection() => _createConnection(_connectionString);

        public DbCommand CreateCommand(DbConnection connection) => _createCommand(connection);

        public DbParameter CreateParameter(string name, object value) => _createParameter(name, value);

        public virtual IDbProviderValueInterpreter Interpreter { get; } = new DbProviderValueInterpreter();
    }
}
