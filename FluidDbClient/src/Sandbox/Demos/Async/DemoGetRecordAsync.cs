
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluidDbClient.Sandbox.Demos.Async
{
    public static class DemoGetRecordAsync
    {
        public static async Task StartAsync()
        {
            var query = new ScriptDbQuery("SELECT * FROM Robot WHERE Name = @name;", new {name = "Terminator"});

            var record = await query.GetRecordAsync();

            Debug.WriteLine($"{record["RobotId"], -6}   {record["Name"]}   {record["DateBuilt"]}");
        }
    }
}
