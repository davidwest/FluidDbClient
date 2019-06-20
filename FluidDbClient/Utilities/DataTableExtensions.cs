using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace FluidDbClient
{
    public static class DataTableExtensions
    {
        public static IEnumerable<DataColumn> GetDataColumns(this DataTable table)
        {
            for (var i = 0; i != table.Columns.Count; i++)
            {
                yield return table.Columns[i];
            }
        }
        
        public static string ToDiagnosticString(this DataTable dt)
        {
            var builder = new StringBuilder();

            builder.AppendLine();

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
            var builder = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                builder.AppendLine(dt.ToDiagnosticString());
            }

            return builder.ToString();
        }
    }
}
