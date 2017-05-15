using System;
using System.Diagnostics;
using FluidDbClient.Sandbox.Models;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public static class DemoTableTypeMap
    {
        public static void Start()
        {
            InitializeData.Start();

            var map = new WidgetTableTypeMap();
            
            RegisterTableType(map);

            AddNewWidgets(map);

            Debug.WriteLine("\n!!! Inserted New Widgets !!!\n");

            foreach (var rec in Db.GetResultSet("SELECT * FROM Widget;"))
            {
                Debug.WriteLine(rec["Name"]);
            }
        }

        private static void AddNewWidgets(WidgetTableTypeMap map)
        {
            var widgets = new[]
            {
                new Widget(0, Guid.NewGuid(), "Everything Juicer", "It juices everything"),
                new Widget(0, Guid.NewGuid(), "80-Tooth Gear", "Keep it grinding!")
            }.ToStructuredData(map);

            Db.Execute("INSERT INTO Widget (GlobalId, Name, Description) SELECT GlobalId, Name, Description FROM @widgets;",
                        new { widgets });
        }

        private static void RegisterTableType(TableTypeMap map)
        {
            var script = TableTypeScriptFactory.CreateScriptFor(map);
            Debug.WriteLine(script);

            TableTypeRegistry.Register(map);
        }
    }
}
