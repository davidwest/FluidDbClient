
using System.Data;
using System.Diagnostics;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.ParameterSpecification
{
    public static class DemoReturnParameters
    {
        public static void Start()
        {
            /*----------------------------------------------------
                Contents of "MultiplyByTwo" stored procedure:

                @inVal INT

                RETURN @inVal * 2;
            -----------------------------------------------------*/

            /*------------------------------------------------------------------------
                The SqlParamDef class is in the FluidDbClient Sql Server provider.

                It allows a Sql parameter to be *partially* defined : 
                    * name is NOT specified
                    * value can be specified, otherwise null assumed
                    * auxillary properties (direction, size, etc.) are optional

                Nulls are automatically converted into DBNull objects.
            -------------------------------------------------------------------------*/

            var cmd = 
                new StoredProcedureDbCommand("MultiplyByTwo",
                                             new
                                             {
                                                 inVal = 10,
                                                 retVal = new SqlParamDef(SqlDbType.Int) { Direction = ParameterDirection.ReturnValue }
                                             });

            Debug.WriteLine(cmd.ToDiagnosticString());

            cmd.Execute();

            Debug.WriteLine(cmd["retVal"]);

            Debug.WriteLine(cmd.ToDiagnosticString());
        }

    }
}
