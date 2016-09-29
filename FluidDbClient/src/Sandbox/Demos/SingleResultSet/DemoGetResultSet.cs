
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.SingleResultSet
{
    public static class DemoGetResultSet
    {
        public static void Start()
        {
            /*-------------------------------------------------------------------------
                The filter-via-join operation shown is a contrivance for the demo.
                The point is to show the .GetResultSet() method within an arbitrarily
                complex LINQ expression.
            ---------------------------------------------------------------------------*/

            var specificNames = new[] {"Clank", "R2-D2", "HAL 9000"};

            var filteredRobots =
                from rec in Db.GetResultSet("SELECT * FROM Robot;")
                let robot = new
                {
                    RobotId = rec.Get<int>("RobotId"),
                    Name = rec.Get<string>("Name"),
                    Description = rec.Get<string>("Description"),
                    IsEvil = rec.Get<bool>("IsEvil")
                }
                join name in specificNames on robot.Name equals name
                orderby robot.Name
                select robot;

            var builder = new StringBuilder();

            foreach (var bot in filteredRobots)
            {
                builder.AppendLine($"{bot.RobotId, -6} {bot.Name, -15} {bot.Description, -50} {bot.IsEvil}");
            }

            Debug.WriteLine(builder);
        }
    }
}
