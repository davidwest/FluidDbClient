
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluidDbClient.Sandbox.Demos.Async
{
    public static class DemoCollectResultSetsAsync
    {
        public static async Task StartAsync()
        {
            var query = new ScriptDbQuery("SELECT * FROM Robot; SELECT * FROM Widget;");

            var records = await query.CollectResultSetsAsync(2);
        
            foreach (var rec in records[0])
            {
                Debug.WriteLine(rec["Name"]);
            }

            Debug.WriteLine("\n");

            foreach (var rec in records[1])
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
