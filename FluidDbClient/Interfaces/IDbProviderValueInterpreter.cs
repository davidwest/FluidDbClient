using System.Data.Common;

namespace FluidDbClient
{
    public interface IDbProviderValueInterpreter
    {
        string GetDiagnosticString(DbParameter parameter);

        string FormatScriptLiteral(object value);

        string GetPrefixedParameterName(string parameterName);

        string GetUnprefixedParameterName(string parameterName);

        bool CanEvaluateAsMultiParameters(object value);
    }
}
