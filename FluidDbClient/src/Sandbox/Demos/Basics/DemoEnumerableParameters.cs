using System;
using System.Diagnostics;
using System.Linq;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.Basics
{
    public static class DemoEnumerableParameters
    {
        public static void Start()
        {
            var names = new [] {"eve", "bender", "dalek", "wildcard"};
            var dates = new[] {new DateTime(1983, 6, 28), new DateTime(1992, 1, 25)};

            var result = 
                Db.GetResultSet("SELECT Name, Description FROM Robot WHERE Name IN (@names) OR DateBuilt IN (@dates);", new {names, dates})
                .Select(rec => rec.Get<string>("Name") + " : " + rec.Get<string>("Description")); 

            foreach (var item in result)
            {
                Debug.WriteLine(item);
            }
        }
    }
}
