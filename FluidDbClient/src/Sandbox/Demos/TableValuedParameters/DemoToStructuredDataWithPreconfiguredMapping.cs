using System;
using System.Diagnostics;

using FluidDbClient.Shell;
using FluidDbClient.Sql;
using FluidDbClient.Sandbox.Models;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public static class DemoToStructuredDataWithPreconfiguredMapping
    {
        public static void Start()
        {
            var robots = new[]
            {
                new RobotGeneralPurposeDto
                {
                    Name = "Smokey",
                    Description = "The robot version of the bear?",
                    DateBuilt = DateTime.Now,
                    ExtraPropertyOne = "This property (and the next) will not be included in the TVP",
                    ExtraPropertyTwo = Guid.NewGuid()
                },
                new RobotGeneralPurposeDto
                {
                    Name = "Mr. Roboto",
                    Description = "Domo aryigato, mr. roboto!",
                    DateBuilt = new DateTime(1981,3,21),
                    ExtraPropertyOne = "This property (and the next) will not be included in the TVP",
                    ExtraPropertyTwo = Guid.NewGuid()
                }
            };
            
            // NOTE: The SELECT must be explicit because RobotId is part of the table type definition.
            //       In this case, all RobotId values are default int (0).

            Db.Execute("INSERT INTO Robot (Name, Description, DateBuilt, IsEvil) SELECT Name, Description, DateBuilt, IsEvil FROM @data;", 
                       new
                       {
                           data = robots.ToStructuredData()
                       });
            
            Debug.WriteLine("\n!!! Inserted New Robots !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Robot;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }
    }
}
