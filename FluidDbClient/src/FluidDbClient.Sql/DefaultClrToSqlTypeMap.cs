using System;
using System.Collections.Generic;
using System.Data;

namespace FluidDbClient.Sql
{
    public static class DefaultClrToSqlTypeMap
    {
        private static readonly Dictionary<Type, SqlDbType> Map = new Dictionary<Type, SqlDbType>
        {
            {typeof(bool), SqlDbType.Bit },
            {typeof(byte), SqlDbType.TinyInt },
            {typeof(char), SqlDbType.NChar },
            {typeof(short), SqlDbType.SmallInt },
            {typeof(int), SqlDbType.Int },
            {typeof(long), SqlDbType.BigInt },
            {typeof(decimal), SqlDbType.Decimal },
            {typeof(double), SqlDbType.Float },
            {typeof(float), SqlDbType.Real },
            {typeof(Guid), SqlDbType.UniqueIdentifier },
            {typeof(DateTime), SqlDbType.DateTime },
            {typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
            {typeof(TimeSpan), SqlDbType.Time },
            {typeof(string), SqlDbType.NVarChar },
            {typeof(char[]), SqlDbType.NVarChar },
            {typeof(byte[]), SqlDbType.VarBinary }
        };

        public static SqlDbType? GetSqlTypeFor(object value)
        {
            return GetSqlTypeFor(value.GetType());
        }

        public static SqlDbType? GetSqlTypeFor(Type clrType)
        {
            clrType = clrType.GetUnderlyingScalarFieldType();

            return Map.TryGetValue(clrType, out var value) ? (SqlDbType?)value : null;
        }
    }
}
