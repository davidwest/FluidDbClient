using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.CommandsAndSessions
{
    public static class DemoCommand
    {
        /*---------------------------------------------------------------------------------------------
         
            Unless using a DbSession, DbCommand, or DbTransaction, invoking ManagedDbCommand.Execute() 
            will automatically wrap the execution inside a transaction and commit it when it is finished.

        -----------------------------------------------------------------------------------------------*/

        public static void Start()
        {
            ShowRobotNames();

            ReverseSomeRobotNames();

            ShowRobotNames();
        }

        private static void ReverseSomeRobotNames()
        {
            Db.Execute("UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Search + '%';", 
                       new { search = "Mystery Science Theater" });
        }

        private static void ShowRobotNames()
        {
            var names = GetRobotNames();

            var builder = new StringBuilder("\n");

            foreach (var name in names)
            {
                builder.AppendLine(name);
            }

            builder.AppendLine();

            Debug.WriteLine(builder);
        }

        private static IEnumerable<string> GetRobotNames()
        {
            return Db.GetResultSet("SELECT Name FROM Robot;").Select(rec => rec.Get<string>("Name"));
        }
    }
}
