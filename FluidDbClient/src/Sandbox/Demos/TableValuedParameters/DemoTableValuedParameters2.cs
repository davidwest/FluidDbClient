using System;
using System.Diagnostics;

using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public class RobotDto
    {
        private string _name;
        private string _description;

        public string Name
        {
            get { return _name; }
            set { _name = (value ?? string.Empty).Trim(); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = (value ?? string.Empty).Trim(); }
        }

        public DateTime DateBuilt { get; set; }
        public bool IsEvil { get; set; }
    }

    public static class DemoTableValuedParameters2
    {
        public static void Start()
        {
            InitializeData.Start();
            Debug.WriteLine("\n!!! Initialized Data !!!\n");

            /*-------------------------------------------------------------------------
            
                This works directly with the default StructuredDataBuilder class.
                Meta data is derived from the object's property names and values.
                This uses strongly-typed objects.

            ---------------------------------------------------------------------------*/
            
            var data = new[]
            {
                new RobotDto {Name = "Smokey", Description = "The robot version of the bear?", DateBuilt = DateTime.Now},
                new RobotDto {Name = "Mr. Roboto", Description = "Domo aryigato, mr. roboto!", DateBuilt = new DateTime(1981,3,21)}
            }.ToStructuredData("NewRobots");
            
            Db.Execute("INSERT INTO Robot (Name, Description, DateBuilt, IsEvil) SELECT Name, Description, DateBuilt, IsEvil FROM @data;", new { data });

            Debug.WriteLine("\n!!! Inserted New Robots !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Robot;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
