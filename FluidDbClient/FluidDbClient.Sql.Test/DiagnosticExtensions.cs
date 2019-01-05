using System.Data;
using System.IO;
using System.Text;

namespace FluidDbClient.Sql.Test
{
    public static class DiagnosticExtensions
    {
        public static string ToDiagnosticString(this DataTable dt)
        {
            var builder = new StringBuilder();

            foreach (var col in dt.GetDataColumns())
            {
                builder.AppendLine($"{col.ColumnName,-30} {col.DataType.Name}");
            }

            builder.AppendLine();

            var writer = new StringWriter(builder);
            dt.WriteXml(writer);
            return writer.ToString();
        }

        public static string ToDiagnosticString(this DataSet ds)
        {
            var writer = new StringWriter();
            ds.WriteXml(writer);
            return writer.ToString();
        }
    }
}
