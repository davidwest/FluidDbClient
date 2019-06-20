using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;

namespace FluidDbClient.OleDb
{
    public class OleDbValueInterpreter : DbProviderValueInterpreter
    {
        public override string GetDiagnosticString(DbParameter parameter)
        {
            var sqlParam = (OleDbParameter)parameter;

            // TODO
            return sqlParam.ToString();
        }

        public override bool CanEvaluateAsMultiParameters(object value)
        {
            return base.CanEvaluateAsMultiParameters(value) &&
                   !(value is IEnumerable<OleDbParameter>);
        }
    }
}
