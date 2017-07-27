using System;
using System.Diagnostics;
using FluidDbClient.Sandbox.Models;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public class DemoCombineStructuredDataAndMultiParam
    {
        public static void Start()
        {
            var widgetsToAdd = new[]
            {
                new Widget(0, Guid.NewGuid(), "Everything Juicer", "It juices everything"),
                new Widget(0, Guid.NewGuid(), "80-Tooth Gear", "Keep it grinding!")
            };

            var widgetsToDelete = new [] {"Spiralizer", "Logic Monkey"};
            
            Db.Execute(Script, new
            {
                widgetsToAdd = widgetsToAdd.ToStructuredData(),
                widgetsToDelete
            });

            Debug.WriteLine("\n!!! Inserted And Deleted Widgets !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Widget;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }

        private const string Script =
@"
INSERT INTO Widget (GlobalId, Name, Description) SELECT GlobalId, Name, Description FROM @widgetsToAdd;
DELETE FROM Widget WHERE Name IN (@widgetsToDelete);
";
    }
}
