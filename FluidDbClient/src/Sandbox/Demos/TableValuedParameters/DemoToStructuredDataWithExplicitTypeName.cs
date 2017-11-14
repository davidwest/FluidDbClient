using System;
using System.Diagnostics;

using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public class RobotDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateBuilt { get; set; }
        public bool IsEvil { get; set; }
    }

    public static class DemoToStructuredDataWithExplicitTypeName
    {
        public static void Start()
        {
            var newRobots = new[]
            {
                new RobotDto {Name = "Smokey", Description = "The robot version of the bear?", DateBuilt = DateTime.Now},
                new RobotDto {Name = "Mr. Roboto", Description = "Domo aryigato, mr. roboto!", DateBuilt = new DateTime(1981,3,21)}
            };
            
            // note that using SELECT * is ok here because there is an exact match in the # of projected fields:

            Db.Execute("INSERT INTO Robot (Name, Description, DateBuilt, IsEvil) SELECT * FROM @data;", 
                       new
                       {
                           data = newRobots.ToStructuredData("NewRobots")
                       });
            
            Debug.WriteLine("\n!!! Inserted New Robots !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Robot;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
