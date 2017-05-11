using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.CommandsAndSessions
{
    public static class DemoExternalTransaction
    {
        public static void Start()
        {
            ShowRobotNames();

            ReverseSomeRobotNames();

            ShowRobotNames();
        }

        private static void ReverseSomeRobotNames()
        {
            using (var conn = Db.CreateConnection())
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    // --- operation 1 : write ---
                    Db.Execute(trans,
                               "UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Search + '%';",
                               new { search = "Mystery Science Theater" });


                    // --- operation 2 : read (shows that queries can be included in transactions) ---

                    var widgetRecords = Db.GetResultSet(trans, "SELECT * FROM Widget;");

                    Debug.WriteLine(widgetRecords.Count());


                    // --- operation 3 : write

                    Db.Execute(trans,
                               "UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Search + '%';",
                               new { search = "Portal" });

                    trans.Commit();
                }       
            }
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
            Debug.WriteLine("");
            return Db.GetResultSet("SELECT Name FROM Robot;").Select(rec => rec.Get<string>("Name"));
        }
    }
}
