using System.Data.Common;

namespace FluidDbClient
{
    public class DbProviderValueInterpreter : IDbProviderValueInterpreter
    {
        public virtual string GetDiagnosticString(DbParameter parameter)
        {
            return $"{parameter.ParameterName} : {parameter.Value}";
        }

        public virtual string FormatScriptLiteral(object value)
        {
            return ScriptLiteralFormatter.Format(value);
        }

        public virtual string GetPrefixedParameterName(string parameterName)
        {
            return parameterName.StartsWith("@") ? parameterName : $"@{parameterName}";
        }

        public virtual string GetUnprefixedParameterName(string parameterName)
        {
            return parameterName.TrimStart('@');
        }

        public virtual bool CanEvaluateAsMultiParameters(object value)
        {
            return !(value is string) && !(value is byte[]);
        }
    }
}
