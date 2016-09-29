
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.MultipleResultSets
{
    public static class DemoProcessResultSets2
    {
        private const string Script =

@"
SELECT * FROM Widget WHERE Name = @widgetName; 
SELECT * FROM Widget; 
SELECT * FROM Robot;";

        public static void Start()
        {
            var builder = new StringBuilder();
            
            /*-----------------------------------------------------------------------------
                .Buffer() extension is just shorthand for .Select(dr => dr.Copy())

                For each enumeration, the current state of the data reader is copied into
                an object that shares the same interface as the data reader : IDataRecord

                This means we can "suspend" object mapping and continue working with
                IEnumerable<IDataRecord> in any subsequent LINQ operations.

                In the following example, if we excluded the .Buffer() call, this exception 
                would be thrown when an .OrderBy() method is reached: 
                
                "Invalid attempt to call MetaData when reader is closed."
            -------------------------------------------------------------------------------*/

            Db.ProcessResultSets(Script, new {widgetName = "Fake Name - should return no results"},
                                 records => OrderAndRenderWidgetRecords(builder, records.Buffer()), // <-- 1
                                 records => OrderAndRenderWidgetRecords(builder, records.Buffer()), // <-- 2
                                 records => OrderAndRenderRobotRecords(builder, records.Buffer())); // <-- 3

            Debug.WriteLine(builder);
        }

        private static void OrderAndRenderWidgetRecords(StringBuilder builder, IEnumerable<IDataRecord> records)
        {
            var hasRecords = false;

            foreach (var rec in records.OrderBy(r => r.Get<string>("Name")))
            {
                hasRecords = true;
                builder.AppendLine($"{rec["Name"],-30} {rec["GlobalId"]}");
            }

            if (!hasRecords)
            {
                builder.AppendLine("\n!!! No Records in this result !!!\n");
            }
        }

        private static void OrderAndRenderRobotRecords(StringBuilder builder, IEnumerable<IDataRecord> records)
        {
            builder.AppendLine();

            foreach (var rec in records.OrderBy(r => r.Get<DateTime>("DateBuilt")))
            {
                builder.AppendLine($"{rec["Name"], -30} {$"{rec.Get<DateTime>("DateBuilt"):D}", -40} {rec["Description"]}");
            }
        }
    }
}
