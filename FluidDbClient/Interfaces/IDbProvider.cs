using System.Data.Common;

namespace FluidDbClient
{
    public interface IDbProvider
    {        
        string ProviderId { get; }
        
        DbConnection CreateConnection();

        DbCommand CreateCommand(DbConnection connection);

        DbParameter CreateParameter(string name, object value);

        IDbProviderValueInterpreter Interpreter { get; }
    }
}
