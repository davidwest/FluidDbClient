
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluidDbClient.Sandbox.Demos.Async
{
    public static class DemoGetScalarAsync
    {
        public static async Task StartAsync()
        {
            var query = new ScriptDbQuery("SELECT DateDestroyed FROM Robot WHERE Name LIKE '%' + @name + '%';", new {name = "hal"});

            // note use of db null substitute value
            var dateDestroyed = await query.GetScalarAsync(new DateTime(1980, 1, 1));

            Debug.WriteLine(dateDestroyed);
        }
    }
}
