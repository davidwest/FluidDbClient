
using System;

namespace FluidDbClient.Sql
{
    internal static class SqlScriptLiteralFormatter
    {
        public static string Format(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            var type = value.GetType();

            if (type == typeof(Guid))
            {
                return $"'{value}'";
            }

            if (type == typeof(DateTime))
            {
                return $"'{(DateTime)value:o}'";
            }

            if (type == typeof(string))
            {
                return $"'{((string)value).Replace("'", "''")}'";
            }

            if (type == typeof(bool))
            {
                return (bool) value ? "1" : "0";
            }

            return value.ToString();
        }
    }
}
