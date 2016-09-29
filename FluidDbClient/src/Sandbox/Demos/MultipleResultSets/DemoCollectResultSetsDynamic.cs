
using System.Diagnostics;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.MultipleResultSets
{
    public static class DemoCollectResultSetsDynamic
    {
        public static void Start()
        {
            /* ------------------------------------------------------------------------------------------
             
                Avoid using this in production code.
                It is much less efficient than collecting IDataRecords and strongly-type objects

            ---------------------------------------------------------------------------------------------*/

            var resultSets = Db.CollectResultSetsDynamic(2, "SELECT * FROM Robot; SELECT * FROM Widget;");

            var builder = new StringBuilder();

            foreach (var rec in resultSets[0])
            {
                builder.AppendLine($"{rec.RobotId, -6} {rec.Name}");
            }

            builder.AppendLine();

            foreach (var rec in resultSets[1])
            {
                builder.AppendLine($"{rec.WidgetId, -6} {rec.Name}");
            }

            Debug.WriteLine(builder);
        }
    }
}
