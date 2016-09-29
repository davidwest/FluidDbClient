
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.Async
{
    public static class DemoDbSessionAsync
    {
        public static async Task StartAsync()
        {
            await ShowRobotNamesAsync();

            await ChangeRobotNamesAsync();

            await ShowRobotNamesAsync();
        }

        private static async Task ChangeRobotNamesAsync()
        {
            using (var session = new DbSession())
            {
                // --- operation 1 : write ---
                await Db.ExecuteAsync(session, 
                                      "UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Excerpt + '%';", 
                                      new { excerpt = "Mystery Science Theater" });


                // --- operation 2 : read (shows that queries can be included in sessions) ---
                var widgets = await Db.CollectResultSetAsync(session, "SELECT * FROM Widget;");

                Debug.WriteLine(widgets.Count);


                // --- operation 3 : write
                await Db.ExecuteAsync(session, 
                                      "UPDATE Robot SET Name = REVERSE(Name) WHERE Description LIKE '%' + @Excerpt + '%';", 
                                      new { excerpt = "Portal" });

                session.Commit();
            }
        }

        private static async Task ShowRobotNamesAsync()
        {
            var names = await GetRobotNamesAsync();

            var builder = new StringBuilder("\n");

            foreach (var name in names)
            {
                builder.AppendLine(name);
            }

            builder.AppendLine();

            Debug.WriteLine(builder);
        }

        private static async Task<IReadOnlyList<string>>  GetRobotNamesAsync()
        {
            return await Db.CollectResultSetAsync("SELECT Name FROM Robot;", rec => rec.Get<string>("Name"));
        }
    }
}
