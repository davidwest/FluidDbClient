using System;
using Oracle.ManagedDataAccess.Client;

namespace FluidDbClient.Oracle
{
    public static class OracleParameterFactory
    {
        public static OracleParameter CreateParameter(string name, object value)
        {
            return value == null 
                ? new OracleParameter(name, DBNull.Value) 
                : new OracleParameter(name, value);
        }
    }
}
