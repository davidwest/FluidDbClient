

using System.Data;
using System.Diagnostics;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.ParameterSpecification
{
    public static class DemoOutputParameters
    {
        public static void Start()
        {
            /*------------------------------------------------------------------------
                The SqlParamDef class is in the FluidDbClient Sql Server provider.

                It allows a Sql parameter to be *partially* defined : 
                    * name is NOT specified
                    * value can be specified, otherwise null assumed
                    * auxillary properties (direction, size, etc.) are optional

                Nulls are automatically converted into DBNull objects.
            -------------------------------------------------------------------------*/

            var cmd = new ScriptDbCommand("SET @OutParam = @InParam * 2;", 
                                          new
                                          {
                                              inParam = 10,
                                              outParam = new SqlParamDef(SqlDbType.Int) {Direction = ParameterDirection.Output}
                                          });

            Debug.WriteLine(cmd.ToDiagnosticString());

            cmd.Execute();

            Debug.WriteLine(cmd["OutParam"]);

            Debug.WriteLine(cmd.ToDiagnosticString());
        }
    }
}
