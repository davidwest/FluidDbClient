
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.MultipleResultSets
{
    public static class DemoProcessResultSets1
    {
        private const string Script =

@"
SELECT * FROM Widget WHERE Name = @widgetName; 
SELECT * FROM Widget; 
SELECT * FROM Robot;";

        public static void Start()
        {
            var builder = new StringBuilder();

            Db.ProcessResultSets(Script, new {widgetName = "Fake Name - should return no results"},
                                 records => RenderWidgetRecords(builder, records),  // <-- 1
                                 records => RenderWidgetRecords(builder, records),  // <-- 2
                                 records => RenderRobotRecords(builder, records));  // <-- 3

            Debug.WriteLine(builder);
        }

        private static void RenderWidgetRecords(StringBuilder builder, IEnumerable<IDataRecord> records)
        {
            var hasRecords = false;

            foreach (var rec in records)
            {
                hasRecords = true;
                builder.AppendLine($"{rec["Name"],-30} {rec["GlobalId"]}");
            }

            if (!hasRecords)
            {
                builder.AppendLine("\n!!! No Records in this result !!!\n");
            }
        }

        private static void RenderRobotRecords(StringBuilder builder, IEnumerable<IDataRecord> records)
        {
            builder.AppendLine();

            foreach (var rec in records)
            {
                builder.AppendLine($"{rec["Name"], -30} {$"{rec.Get<DateTime>("DateBuilt"):D}", -40} {rec["Description"]}");
            }
        }
    }
}
