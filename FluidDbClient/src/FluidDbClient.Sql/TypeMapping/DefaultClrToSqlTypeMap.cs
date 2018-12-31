using System;
using System.Collections.Generic;
using System.Data;
//using System.Reflection;

namespace FluidDbClient.Sql
{
    public static class DefaultClrToSqlTypeMap
    {
        private static readonly Dictionary<Type, SqlDbType> Map = new Dictionary<Type, SqlDbType>
        {
            {typeof(bool), SqlDbType.Bit },
            {typeof(bool?), SqlDbType.Bit },
            {typeof(byte), SqlDbType.TinyInt },
            {typeof(byte?), SqlDbType.TinyInt },
            {typeof(char), SqlDbType.VarChar },
            {typeof(char?), SqlDbType.VarChar },
            {typeof(short), SqlDbType.SmallInt },
            {typeof(short?), SqlDbType.SmallInt },
            {typeof(int), SqlDbType.Int },
            {typeof(int?), SqlDbType.Int },
            {typeof(long), SqlDbType.BigInt },
            {typeof(long?), SqlDbType.BigInt },
            {typeof(decimal), SqlDbType.Decimal },
            {typeof(decimal?), SqlDbType.Decimal },
            {typeof(double), SqlDbType.Float },
            {typeof(double?), SqlDbType.Float },
            {typeof(float), SqlDbType.Real },
            {typeof(float?), SqlDbType.Real },
            {typeof(Guid), SqlDbType.UniqueIdentifier },
            {typeof(Guid?), SqlDbType.UniqueIdentifier },
            {typeof(DateTime), SqlDbType.DateTime },
            {typeof(DateTime?), SqlDbType.DateTime },
            {typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
            {typeof(DateTimeOffset?), SqlDbType.DateTimeOffset },
            {typeof(string), SqlDbType.NVarChar },
            {typeof(char[]), SqlDbType.NVarChar },
            {typeof(byte[]), SqlDbType.VarBinary}
        };

        public static SqlDbType? GetSqlTypeFor(object value)
        {
            var clrType = value.GetType();

            return GetSqlTypeFor(clrType);
        }

        public static SqlDbType? GetSqlTypeFor(Type clrType)
        {
            if (clrType.IsEnum)
            {
                clrType = clrType.GetEnumUnderlyingType();
            }

            return Map.ContainsKey(clrType) ? (SqlDbType?)Map[clrType] : null;
        }
    }
}
