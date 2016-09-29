
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public static class SqlParameterDiagnosticExtensions
    {
        public static string ToDiagnosticString(this SqlParameter param)
        {
            var value = param.Value;

            var prefix = $"{param.ParameterName,-20} | {param.Direction, -15} | {param.SqlDbType,-10}";

            var type = param.SqlDbType;

            if (type == SqlDbType.Structured)
            {
                var records = value as IEnumerable<SqlDataRecord>;
                var rowCount = records?.Count() ?? 0;

                return $"{prefix} | table type: {param.TypeName, -30} | rows: {rowCount}";
            }

            if (type == SqlDbType.NVarChar || 
                type == SqlDbType.VarChar || 
                type == SqlDbType.Text || 
                type == SqlDbType.NChar || 
                type == SqlDbType.NText ||
                type == SqlDbType.Xml)
            {
                var sizeStr = GetSizeString(param.Size);

                var text = value as string;
                var excerpt = text != null ? Shorten(text, 20) : TryDbNull(value) ?? "???";

                return $"{prefix} | {excerpt, -30} | size limit: {sizeStr}";
            }

            if (type == SqlDbType.Binary || 
                type == SqlDbType.VarBinary || 
                type == SqlDbType.Image)
            {
                var sizeStr = GetSizeString(param.Size);

                var bytes = value as byte[];
                var dataStr = bytes != null ? bytes.Length.ToString() : TryDbNull(value) ?? "???";
                
                return $"{prefix} | data size:{dataStr, -10} | size limit: {sizeStr}";
            }

            var valStr = TryDbNull(value) ?? value?.ToString();

            if (type == SqlDbType.Decimal || 
                type == SqlDbType.Float || 
                type == SqlDbType.Real || 
                type == SqlDbType.Money ||
                type == SqlDbType.SmallMoney)
            {
                return $"{prefix} | {valStr, -15} | precision: {param.Precision} | scale: {param.Scale}";
            }

            return $"{prefix} | {valStr}";
        }

        private static string GetSizeString(int size)
        {
            return size == -1 ? "MAX" : size.ToString();
        }

        private static string Shorten(string source, int maxSize)
        {
            return source.Length < maxSize ? source : $"{new string(source.Take(maxSize).ToArray())}...";
        }

        private static string TryDbNull(object value)
        {
            return value is DBNull ? "DBNull" : null;
        }
    }
}
