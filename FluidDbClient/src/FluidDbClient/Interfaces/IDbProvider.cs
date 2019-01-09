using System.Data.Common;

namespace FluidDbClient
{
    public interface IDbProvider
    {        
        string ProviderId { get; }
        
        DbConnection CreateConnection();
        
        DbParameter CreateParameter(string name, object value);

        IDbProviderValueInterpreter Interpreter { get; }
    }
}
