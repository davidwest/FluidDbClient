using System.Data.Common;

namespace FluidDbClient
{
    public interface IDbProvider
    {
        string ProviderId { get; }
        
        DbConnection CreateConnection<TDatabase>() where TDatabase : Database;
        DbConnection CreateConnection();
        DbConnection CreateConnection(string connectionString);

        DbCommand CreateCommandFrom(DbConnection connection);
        DbCommand CreateCommandFrom(DbTransaction transaction);
        
        DbParameter CreateParameter(string name, object value);

        IDbProviderTextInterpreter TextInterpreter { get; }
    }
}
