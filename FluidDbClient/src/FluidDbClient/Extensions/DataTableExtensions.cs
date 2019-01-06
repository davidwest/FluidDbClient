﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
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

        public static DataTable ToDataTable<T>(this IEnumerable<T> items, string tableName = null) where T : class
        {
            var type = typeof(T);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var dataTable = new DataTable(tableName ?? type.Name);

            foreach (var info in properties)
            {
                var column = new DataColumn(info.Name, info.PropertyType.GetPrimitiveTypeForSchema());

                dataTable.Columns.Add(column);
            }

            foreach (var item in items)
            {
                var values = new object[properties.Length];

                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
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
