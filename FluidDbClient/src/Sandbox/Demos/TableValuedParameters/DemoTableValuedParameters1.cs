using System;
using System.Diagnostics;

using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public static class DemoTableValuedParameters1
    {
        public static void Start()
        {
            InitializeData.Start();
            Debug.WriteLine("\n!!! Initialized Data !!!\n");

            /*-----------------------------------------------------------------------------------
            
                This leverages a custom class that derives from StructuredDataBuilder.
                The meta data has already been defined, so we just have to fill in the values.

            ------------------------------------------------------------------------------------*/

            var data = 
                new NewRobotsDataBuilder()
                .AppendValues("Smokey", "The robot version of the bear", DateTime.Now, false)
                .AppendValues("Mr. Roboto", "Domo aryigato, mr. roboto!", new DateTime(1981, 3, 21), false)
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
