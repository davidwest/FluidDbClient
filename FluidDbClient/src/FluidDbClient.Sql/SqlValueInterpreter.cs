using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class SqlValueInterpreter : IDbProviderValueInterpreter
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

        public bool CanEvaluateAsMultiParameters(object value)
        {
            return !(value is string) && !(value is byte[]) && !(value is IEnumerable<SqlDataRecord>);
        }
    }
}
