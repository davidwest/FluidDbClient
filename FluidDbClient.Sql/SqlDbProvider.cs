using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace FluidDbClient.Sql
{
    internal class SqlDbProvider : IDbProvider
    {
        private readonly string _connectionString;

        public SqlDbProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ProviderId => "Microsoft.Data.SqlClient";

        public DbConnection CreateConnection() => new SqlConnection(_connectionString);

        public DbCommand CreateCommand(DbConnection connection) => connection.CreateCommand();

        public DbParameter CreateParameter(string name, object value) => SqlParameterFactory.CreateParameter(name, value);

        public IDbProviderValueInterpreter Interpreter { get; } = new SqlValueInterpreter();
    }
}
