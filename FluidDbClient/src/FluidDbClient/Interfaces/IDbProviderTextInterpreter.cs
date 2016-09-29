
using System.Data.Common;

namespace FluidDbClient
{
    public interface IDbProviderTextInterpreter
    {
        string GetDiagnosticString(DbParameter parameter);

        string FormatScriptLiteral(object value);

        string GetPrefixedParameterName(string parameterName);

        string GetUnprefixedParameterName(string parameterName);
    }
}
