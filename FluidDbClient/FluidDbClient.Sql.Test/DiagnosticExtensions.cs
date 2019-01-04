using System.Data;
using System.IO;

namespace FluidDbClient.Sql.Test
{
    public static class DiagnosticExtensions
    {
        public static string ToXml(this DataTable dt)
        {
            var writer = new StringWriter();
            dt.WriteXml(writer);
            return writer.ToString();
        }

        public static string ToXml(this DataSet ds)
        {
            var writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }
    }
}
