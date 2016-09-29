
using System.Diagnostics;
using System.Threading.Tasks;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.Async
{
    public static class DemoCollectResultSetAsync
    {
        public static async Task StartAsync()
        {
            var robots = await Db.CollectResultSetAsync("SELECT * FROM Robot", 
                                                        rec => rec.MapToRobot());
        
            foreach (var bot in robots)
            {
                Debug.WriteLine($"{bot.RobotId, -6} {bot.Name}");
            }
        }
    }
}
