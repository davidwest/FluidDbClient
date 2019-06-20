
using System;
using System.Diagnostics;

namespace FluidDbClient.Sandbox.Demos.Basics
{
    public static class DemoGetScalar
    {
        public static void Start()
        {
            DemoTypicalGetScalar();
            DemoDbNullSubstitute();
            DemoDbNullSubstituteWhenNoResult();
        }

        private static void DemoTypicalGetScalar()
        {
            var query = new ScriptDbQuery("SELECT Name FROM Robot WHERE Name LIKE '%' + @name + '%';", new { name = "hal" });

            var name = query.GetScalar<string>();

            Debug.WriteLine(name);
        }

        private static void DemoDbNullSubstitute()
        {
            var query = new ScriptDbQuery("SELECT DateDestroyed FROM Robot WHERE Name LIKE '%' + @name + '%';", new { name = "hal" });

            // specify that DBNulls should be returned as the first day of 2020!

            var dateDestroyed = query.GetScalar(new DateTime(2020, 1, 1));

            Debug.WriteLine(dateDestroyed);
        }

        private static void DemoDbNullSubstituteWhenNoResult()
        {
            var query = new ScriptDbQuery("SELECT TOP(1) DateDestroyed FROM Robot WHERE Name LIKE '%' + @name + '%';", new { name = "no such name" });

            // specify that DBNulls should be returned as the first day of 2020!

            var dateDestroyed = query.GetScalar<DateTime>();

            Debug.WriteLine(dateDestroyed);
        }
    }
}
