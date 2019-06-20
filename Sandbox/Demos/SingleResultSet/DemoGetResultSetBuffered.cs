
using System.Diagnostics;
using System.Linq;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.SingleResultSet
{
    public static class DemoGetResultSetBuffered
    {
        public static void Start()
        {
            /*------------------------------------------------------------------------
                .Buffer() extension is just shorthand for .Select(dr => dr.Copy())

                For each enumeration, the current state of the data reader is copied into
                an object that shares the same interface as the data reader : IDataRecord

                This means we can "suspend" object mapping and continue working with
                IEnumerable<IDataRecord> in any subsequent LINQ operations.

                In the following example, if we excluded the .Buffer() call, this exception 
                would be thrown when the .OrderBy() method is reached: 
                
                "Invalid attempt to call MetaData when reader is closed."
            --------------------------------------------------------------------------*/

            var groupedRecords = 
                Db.GetResultSet("SELECT * FROM Robot;")
                .Buffer()
                .OrderBy(rec => rec.Get<string>("Name"))
                .ToLookup(rec => rec.Get<string>("Name").First());

            foreach (var grp in groupedRecords)
            {
                Debug.WriteLine($"\n\n*** {grp.Key} ***");

                foreach (var rec in grp)
                {
                    Debug.WriteLine(rec["Name"]);
                }
            }
        }
    }
}
