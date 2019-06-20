
using System.Diagnostics;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.Basics
{
    public static class DemoGetSingleRecord
    {
        public static void Start()
        {
            var rec = Db.GetRecord("SELECT * FROM Robot WHERE Name = @name;", new {name = "Bender"});

            Debug.WriteLine($"{rec["RobotId"], -6} {rec["Name"], -15} {rec["Description"]}");
        }
    }
}
