
using System;
using System.Data;
using System.Diagnostics;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.ScriptCompilation
{
    public static class DemoScriptCompilationSimple
    {
        /*------------------------------------------------------------------------------------------------
            The script below is obviously not SQL.
            The point is to show how DbScriptCompiler works to build a dynamically parameterized script.

            The type parameter <TDatabase> can be dropped if targeting the default database.

            The resultant DbScriptDocument object can be applied to a query or command like this:

            DbManagedQuery.IncludeScriptDoc(doc);
            DbManagedCommand.IncludeScriptDoc(doc);

        --------------------------------------------------------------------------------------------------*/
        public static void Start()
        {
            var doc = 
                new DbScriptCompiler<AcmeDb>()
                .Append("This is a parameter: {0}.", "Hello")
                .Append("Here are two more parameters: {0} and {1}.", DateTime.Now, true)
                .Append("This is a literal: {{0}}.", Guid.NewGuid())
                .Append("Here's a combination: {0} with some {{1}}, add a little {2} and a touch of {{3}}.", 5, 10.6, "Wow", "Ain't it nice")
                .Append("Notice how the db provider is consulted so that parameter prefixes and literal values are as they should be!")
                .Append("Need some comma-separated parameters? Here ya go: {0}", new { name1 = "Thing1", name2 = "Thing2" })
                .Append("Watch DBNulls appear before your eyes: {0} {1}", null, null)
                .Append("Just make sure not to send in a null as a single parameter. That won't compute!")
                .Append("But (with Sql Provider) you can do this: {0}. No value specified = DBNull;", new SqlParamDef(SqlDbType.Date))
                .Compile();
            
            Debug.WriteLine(doc.Text);

            foreach (var p in doc.Parameters)
            {
                Debug.WriteLine(DbRegistry.GetDatabase<AcmeDb>().Provider.TextInterpreter.GetDiagnosticString(p));
            }
        }
    }
}
