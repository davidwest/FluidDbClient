using System.Data;

namespace FluidDbClient.Sql
{
    public static class SqlDbTypeExtensions
    {
        public static bool CanSpecifyLength(this SqlDbType sqlType)
        {
            return sqlType == SqlDbType.Char ||
                   sqlType == SqlDbType.NChar ||
                   sqlType == SqlDbType.NVarChar ||
                   sqlType == SqlDbType.Text ||
                   sqlType == SqlDbType.NText ||
                   sqlType == SqlDbType.Binary ||
                   sqlType == SqlDbType.VarBinary;
        }

        public static bool CanSpecifyPrecision(this SqlDbType sqlType)
        {
            return sqlType == SqlDbType.Decimal;
        }
    }
}
