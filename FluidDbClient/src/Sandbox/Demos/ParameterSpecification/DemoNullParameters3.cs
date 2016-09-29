
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FluidDbClient.Sandbox.Demos.ParameterSpecification
{
    public class DemoNullParameters3
    {
        public static void Start()
        {
            var cmd = new ScriptDbCommand("UPDATE Robot SET DateDestroyed = @dd WHERE Name = @name;")
            {
                ["name"] = "Terminator"     // <-- can add parameters this way...
            };

            // ... and parameters can be explicitly added:
            // (Note that in doing so, DBNulls must be explicitly used as well!)

            cmd.AddParameters(new SqlParameter("dd", SqlDbType.Date) {Value = DBNull.Value});

            Debug.WriteLine(cmd.ToDiagnosticString());

            cmd.Execute();
        }
    }
}
