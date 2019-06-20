using System;
using System.Data.OleDb;

namespace FluidDbClient.OleDb
{
    public static class OleDbParameterFactory
    {
        public static OleDbParameter CreateParameter(string name, object value)
        {
            var effectiveName = GetEffectiveParameterName(name);

            if (value == null)
            {
                return new OleDbParameter(effectiveName, DBNull.Value);
            }

            return new OleDbParameter(effectiveName, value);
        }

        private static string GetEffectiveParameterName(string sourceName)
        {
            return sourceName.StartsWith("@", StringComparison.OrdinalIgnoreCase) ? sourceName : $"@{sourceName}";
        }
    }
}
