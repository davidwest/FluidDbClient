using System.Data.Common;
using System.Data.SqlClient;

namespace FluidDbClient.Sql
{
    public class SqlDbProvider : DbProvider
    {
        public SqlDbProvider() : base("System.Data.SqlClient")
        {
            TextInterpreter = new SqlTextInterpreter();
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public override DbCommand CreateCommandFrom(DbConnection connection)
        {
            var command = new SqlCommand
            {
                Connection = (SqlConnection) connection
            };

            return command;
        }

        public override DbCommand CreateCommandFrom(DbTransaction transaction)
        {
            var command = new SqlCommand
            {
                Connection = (SqlConnection) transaction.Connection,
                Transaction = (SqlTransaction) transaction
            };

            return command;
        }
        
        public override DbParameter CreateParameter(string name, object value)
        {
            return SqlParameterFactory.CreateParameter(name, value);
        }

        public override IDbProviderTextInterpreter TextInterpreter { get; }

        protected override DbConnection GetConnectionUsing(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
