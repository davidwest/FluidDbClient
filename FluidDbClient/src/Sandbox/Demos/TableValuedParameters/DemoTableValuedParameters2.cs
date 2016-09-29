
using System;
using System.Diagnostics;

using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public static class DemoTableValuedParameters2
    {
        public static void Start()
        {
            InitializeData.Start();
            Debug.WriteLine("\n!!! Initialized Data !!!\n");

            /*-----------------------------------------------------------------------------------
            
                This works directly with the default StructuredDataBuilder<TDatabase> class.
                Meta data is derived from the object's property names and values.

            ------------------------------------------------------------------------------------*/

            var data = 
                new StructuredDataBuilder("NewRobots")
                .Append(new {Name = "Smokey", Description = "The robot version of the bear", DateBuilt = DateTime.Now, IsEvil = false})
                .Append(new {Name = "Mr. Roboto", Description = "Domo aryigato, mr. roboto!", DateBuilt = new DateTime(1981, 3, 21), IsEvil = false})
                .Build();
            
            Db.Execute("INSERT INTO Robot (Name, Description, DateBuilt, IsEvil) SELECT * FROM @data;", new { data });

            Debug.WriteLine("\n!!! Inserted New Robots !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Robot;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
