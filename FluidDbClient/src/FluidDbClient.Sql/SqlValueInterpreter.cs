﻿using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class SqlValueInterpreter : DbProviderValueInterpreter
    {
        public override string GetDiagnosticString(DbParameter parameter)
        {
            var sqlParam = (SqlParameter)parameter;

            return sqlParam.ToDiagnosticString();
        }
        
        public override bool CanEvaluateAsMultiParameters(object value)
        {
            return base.CanEvaluateAsMultiParameters(value) && 
                   !(value is IEnumerable<SqlDataRecord>);
        }
    }
}
