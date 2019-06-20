
using System.Data;
using System.Diagnostics;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.ParameterSpecification
{
    public class DemoNullParameters2
    {
        public static void Start()
        {
            /*--------------------------------------------------------------------------------------
                The SqlParamDef class is in the FluidDbClient Sql Server provider.

                It allows a Sql parameter to be *partially* defined : 
                    * name is NOT specified
                    * value can be specified, otherwise null assumed
                    * auxillary properties (direction, size, etc.) are optional

                Nulls are automatically converted into DBNull objects.
            ----------------------------------------------------------------------------------------*/

            var cmd = new ScriptDbCommand("UPDATE Robot SET DateDestroyed = @dd WHERE Name = @name;", 
                                          new { name = "Terminator", dd = new SqlParamDef(SqlDbType.Date) });
            
            // NOTE: the @dd parameter is now semantically correct even with the null value
            Debug.WriteLine(cmd.ToDiagnosticString());

            cmd.Execute();
        }
    }
}
