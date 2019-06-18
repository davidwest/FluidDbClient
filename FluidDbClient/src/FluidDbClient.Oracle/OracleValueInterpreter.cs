using System.Collections.Generic;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace FluidDbClient.Oracle
{
    public class OracleValueInterpreter : DbProviderValueInterpreter
    {
        public override string GetDiagnosticString(DbParameter parameter)
        {
            var sqlParam = (OracleParameter)parameter;

            // TODO
            return sqlParam.ToString();
        }

        public override bool CanEvaluateAsMultiParameters(object value)
        {
            return base.CanEvaluateAsMultiParameters(value) &&
                   !(value is IEnumerable<OracleParameter>);
        }
    }
}
