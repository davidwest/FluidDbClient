
using System.Data.Common;
using System.Data.SqlClient;

namespace FluidDbClient.Sql
{
    public class SqlTextInterpreter : IDbProviderTextInterpreter
    {
        public string GetDiagnosticString(DbParameter parameter)
        {
            var sqlParam = (SqlParameter)parameter;

            return sqlParam.ToDiagnosticString();
        }

        public string FormatScriptLiteral(object value)
        {
            return SqlScriptLiteralFormatter.Format(value);
        }

        public string GetPrefixedParameterName(string parameterName)
        {
            return parameterName.StartsWith("@") ? parameterName : $"@{parameterName}";
        }

        public string GetUnprefixedParameterName(string parameterName)
        {
            return parameterName.TrimStart('@');
        }
    }
}
