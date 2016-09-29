
using System;
using System.Data;
using System.Diagnostics;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.ScriptCompilation
{
    public static class DemoScriptCompilationObnoxious
    {
        /*---------------------------------------------------------------------------- 
            The script compilation below is nonsense. 
            Its purpose is to show how large, parameterized scripts can be assembled
            programatically where parameters can be generated with various levels of
            specificity.
        -----------------------------------------------------------------------------*/
        public static void Start()
        {
            var doc = GetObnoxiousScriptDoc();
            
            Display(doc);
        }

        private static DbScriptDocument GetObnoxiousScriptDoc()
        {
            var compiler = new DbScriptCompiler();

            for (var i = 0; i != 50; i++)
            {
                if (i % 2 == 0)
                {
                    compiler.Append("INSERT INTO Part (PartNumber, DriverId) VALUES ({0}, {{1}});", i + 11, Guid.NewGuid());
                }
                else
                {
                    // --- wanna set the sql type explicitly? ---
                    compiler.Append("UPDATE Widget SET Active = {{0}}, Name = {2}, Price = {3} WHERE WidgetId = {1};",
                                    true,
                                    17359 - i,
                                    new SqlParamDef(SqlDbType.VarChar, "default widget") { Size = 100 },
                                    12.99);
                }

                if (i % 5 == 0)
                {
                    compiler.Append("UPDATE Robot SET PrimaryUsage = {{0}}, Owner = {1}, DateCommissioned = {3}, DateDestroyed = {{4}} WHERE RobotId = {{2}};",
                                    null,
                                    null,
                                    i + 99,
                                    new SqlParamDef(SqlDbType.Date, DateTime.Now),
                                    DateTime.Now);
                }

                if (i % 13 == 0)
                {
                    compiler.Append("UPDATE Robot SET Motto = {{0}} WHERE Name LIKE {1} AND DateCommissioned > {2};",
                                    "Can't stop, won't stop",
                                    $"Roboto_{i}",
                                    DateTime.Now);
                }

                // --- need some comma-separated parameters? ---
                if (i % 17 == 0)
                {
                    compiler.Append("UPDATE Widget SET Active = {0} WHERE Name IN ({1});",
                                    false,
                                    new { name1 = "Spaghetti Masher", name2 = "Re-spooler", name3 = "Scuba Monkey" });
                }
            }

            return compiler.Compile();
        }

        private static void Display(DbScriptDocument doc)
        {
            Debug.WriteLine(doc.Text);

            foreach (var p in doc.Parameters)
            {
                Debug.WriteLine(DbRegistry.GetDatabase().Provider.TextInterpreter.GetDiagnosticString(p));
            }
        }
    }
}
