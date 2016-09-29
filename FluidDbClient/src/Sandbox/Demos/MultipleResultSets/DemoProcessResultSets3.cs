
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Sandbox.Models;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.MultipleResultSets
{
    public static class DemoProcessResultSets3
    {
        public static void Start()
        {
            /*------------------------------------------------------------------------------------------
            
                Processing multiple result sets can be used to map (copy) each IDataRecord of each
                result set to a strongly-typed object and rendering collections for each type.

                Important: 
                Each process MUST consume the supplied enumerator (e.g. .ToArray()).

            ------------------------------------------------------------------------------------------*/
            Robot[] robots = null;
            Widget[] widgets = null;

            Db.ProcessResultSets("SELECT * FROM Robot; SELECT * FROM Widget; ",
                                 records => robots = records.Select(dr => dr.MapToRobot()).OrderBy(r => r.Name).ToArray(),       // <-- 1
                                 records => widgets = records.Select(dr => dr.MapToWidget()).OrderBy(w => w.Name).ToArray());    // <-- 2


            var builder = new StringBuilder();

            foreach (var bot in robots)
            {
                builder.AppendLine($"{bot.RobotId, -6} {bot.Name}");
            }

            builder.AppendLine();

            foreach (var widget in widgets)
            {
                builder.AppendLine($"{widget.WidgetId} {widget.Name}");
            }

            Debug.WriteLine(builder);
        }
    }
}
