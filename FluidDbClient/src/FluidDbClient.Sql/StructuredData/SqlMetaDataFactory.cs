using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    internal static class SqlMetaDataFactory
    {
        public static SqlMetaData CreateSqlMetaData(string name, SqlDbType sqlType, int order)
        {
            return CreateSqlMetaData(name, sqlType, -1, false, order);
        }
        
        public static SqlMetaData CreateSqlMetaData(string name, SqlDbType sqlType, long maxLength, bool isInUniqueKey, int order)
        {
            return sqlType.CanSpecifyLength()
                ? new SqlMetaData(name, sqlType, maxLength, false, isInUniqueKey, SortOrder.Ascending, order)
                : new SqlMetaData(name, sqlType, false, isInUniqueKey, SortOrder.Ascending, order);
        }

        public static SqlMetaData CreateSqlMetaData(string name, SqlDbType sqlType, byte precision, byte scale, bool isInUniqueKey, int order)
        {
            return sqlType.CanSpecifyPrecision()
                ? new SqlMetaData(name, sqlType, precision, scale, false, isInUniqueKey, SortOrder.Ascending, order)
                : new SqlMetaData(name, sqlType, false, isInUniqueKey, SortOrder.Ascending, order);
        }
    }
}

