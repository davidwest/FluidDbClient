using System.Data.Common;
using System.Data.OleDb;

namespace FluidDbClient.OleDb
{
    public class OleDbProvider : IDbProvider
    {
        private readonly string _connectionString;

        public OleDbProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ProviderId => "System.Data.OleDb";

        public DbConnection CreateConnection() => new OleDbConnection(_connectionString);

        public DbCommand CreateCommand(DbConnection connection) => connection.CreateCommand();

        public DbParameter CreateParameter(string name, object value) => OleDbParameterFactory.CreateParameter(name, value);

        public IDbProviderValueInterpreter Interpreter => new OleDbValueInterpreter();
    }
}
