
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.CommandsAndSessions
{
    public static class DemoSession
    {
        /*-----------------------------------------------------------------------------------------------------
        
            DbSessions allow us to group commands and queries into a single atomic transaction without using
            DbConnections and DbTransactions directly.

            Typical Usage:

            using (var session = new DbSession<TDatabase>()
            {
                <put queries & commands here, each specifying the session variable>

                session.Commit();
            }

            The generic type parameter TDatabase can be dropped if you're targeting the default database.
            IsolationLevel can optionally be supplied in the DbSession constructor (default: ReadCommitted).

            IMPORTANT:
            Any IsolationLevel specified by ManagedDbCommand.Execute(IsolationLevel level) will be ignored
            if the command is using a DbSession; execution will defer to the IsolationLevel of the DbSession.

        ------------------------------------------------------------------------------------------------------*/

        public static void Start()
        {
            ShowRobotNames();

            ReverseSomeRobotNames();
            
            ShowRobotNames();
        }

        private static void ReverseSomeRobotNames()
        {
            using (var session = new DbSession())
            {
                // --- operation 1 : write ---
                Db.Execute(session, 
                           "UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Search + '%';", 
                           new {search = "Mystery Science Theater"});
                

                // --- operation 2 : read (shows that queries can be included in sessions) ---
                
                var widgetRecords = Db.GetResultSet(session, "SELECT * FROM Widget;");

                Debug.WriteLine(widgetRecords.Count());


                // --- operation 3 : write
                
                Db.Execute(session, 
                           "UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Search + '%';", 
                           new {search = "Portal"});

                session.Commit();
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
            return Db.GetResultSet("SELECT Name FROM Robot;").Select(rec => rec.Get<string>("Name"));
        }
    }
}
