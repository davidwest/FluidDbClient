
using System.Diagnostics;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.MultipleResultSets
{
    public static class DemoCollectResultSets
    {
        public static void Start()
        {
            var resultSets = Db.CollectResultSets(2, "SELECT * FROM Robot; SELECT * FROM Widget;");

            var builder = new StringBuilder();

            foreach (var rec in resultSets[0])
            {
                builder.AppendLine(rec.Get<string>("Name"));
            }

            builder.AppendLine();

            foreach (var rec in resultSets[1])
            {
                builder.AppendLine(rec.Get<string>("Name"));
            }

            Debug.WriteLine(builder);
        }
    }
}
