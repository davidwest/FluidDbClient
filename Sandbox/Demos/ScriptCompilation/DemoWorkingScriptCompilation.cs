
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.ScriptCompilation
{
    public static class DemoWorkingScriptCompilation
    {
        public static void Start()
        {
            var evilRobotIds = GetEvilRobotIds();

            var structuredData = GetStructuredData(evilRobotIds);

            var query = GetQueryWithScriptCompilation(structuredData);

            Debug.WriteLine(query.ToDiagnosticString());

            var resultSets = query.CollectResultSets(3);

            var result = BindResultsToString(resultSets);
            
            Debug.WriteLine(result);
        }


        private static ManagedDbQuery GetQueryWithScriptCompilation(StructuredData robotIdData)
        {
            // select 1 (static)
            var query = new ScriptDbQuery("SELECT * FROM Robot WHERE DateBuilt < @maxDate;",
                                          new { maxDate = new DateTime(1980, 1, 1) });

            // selects 2 & 3 (using script compilation)
            var doc =
                new DbScriptCompiler()
                .Append("SELECT * FROM Widget WHERE Name = {0};", "Whirligig")
                .Append("SELECT * FROM Robot r INNER JOIN {0} d ON r.RobotId = d.Id;", robotIdData)
                .Compile();

            /*----------------------------------------------------------------------------
                1: This could have been constructed specifying the database explicitly:

                        new DbScriptCompiler<AcmeDb>()

                2: joining on pre-fetched data is a contrivance for this demo.
                The point is to show that table-value parameters are successfully 
                routed through the DbScriptCompiler to the db provider (Sql Server in 
                this case).
            ------------------------------------------------------------------------------*/

            query.IncludeScriptDoc(doc);

            return query;
        }


        private static IEnumerable<int> GetEvilRobotIds()
        {
            return Db.GetResultSet("SELECT RobotId FROM Robot WHERE IsEvil = 1;")
                     .Select(rec => rec.Get<int>("RobotId"));
        }


        private static StructuredData GetStructuredData(IEnumerable<int> robotIds)
        {
            // "IntegerIds" is the name of the sql server custom table type

            var builder = new StructuredDataBuilder("IntegerIds");

            foreach (var robotId in robotIds)
            {
                builder.Append(new { Id = robotId });
            }

            var data = builder.Build();

            return data;
        }

        
        private static string BindResultsToString(List<IDataRecord>[] resultSets)
        {
            var builder = new StringBuilder();

            builder.AppendLine("\n*** Result 0 ***");
            foreach (var rec in resultSets[0])
            {
                builder.AppendLine($"{rec["RobotId"], -6} {rec["Name"]}");
            }

            builder.AppendLine("\n\n*** Result 1 ***");
            var singleRec = resultSets[1].Single();
            
            builder.AppendLine($"{singleRec["WidgetId"], -6} {singleRec["Name"]}\n");


            builder.AppendLine("\n*** Result 2 ***");
            foreach (var record in resultSets[2])
            {
                builder.AppendLine($"{record["RobotId"], -6} {record["Name"]}");
            }

            return builder.ToString();
        }
    }
}
