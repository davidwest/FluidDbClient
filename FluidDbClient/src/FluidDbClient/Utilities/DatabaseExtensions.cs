using System.Data.Common;

namespace FluidDbClient
{
    public static class DatabaseExtensions
    {
        public static DbConnection CreateConnection(this Database database)
        {
            return database.Provider.CreateConnection();
        }

        public static DbParameter CreateParameter(this Database database, string name, object value)
        {
            return database.Provider.CreateParameter(name, value);
        }
    }
}
