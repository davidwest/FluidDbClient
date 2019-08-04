using System;
using Oracle.ManagedDataAccess.Client;

namespace FluidDbClient.Oracle
{
    public static class OracleParameterFactory
    {
        public static OracleParameter CreateParameter(string name, object value)
        {
            var effectiveName = GetEffectiveParameterName(name);

            if (value == null)
            {
                return new OracleParameter(effectiveName, DBNull.Value);
            }
            
            return new OracleParameter(effectiveName, value);
        }

        private static string GetEffectiveParameterName(string sourceName)
        {
            return sourceName.StartsWith(":") ? sourceName : $":{sourceName}";
        }
    }
}
