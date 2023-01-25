using Microsoft.Data.SqlClient.Server;
using System.Data;

namespace FluidDbClient.Sql
{
    internal static class SqlMetaDataFactory
    {
        public static SqlMetaData CreateSqlMetaData(string name, SqlDbType sqlType)
        {
            return CreateSqlMetaData(name, sqlType, -1);
        }
        
        public static SqlMetaData CreateSqlMetaData(string name, SqlDbType sqlType, long maxLength)
        {
            return sqlType.CanSpecifyLength()
                ? new SqlMetaData(name, sqlType, maxLength)
                : new SqlMetaData(name, sqlType);
        }

        public static SqlMetaData CreateSqlMetaData(string name, SqlDbType sqlType, byte precision, byte scale)
        {
            return sqlType.CanSpecifyPrecision()
                ? new SqlMetaData(name, sqlType, precision, scale)
                : new SqlMetaData(name, sqlType);
        }
    }
}

