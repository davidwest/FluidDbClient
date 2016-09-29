
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;


namespace FluidDbClient
{
    public static class DataMappingExtensions
    {
        public static T DbCast<T>(this object value, T dbNullSubstitute = default(T))
        {
            return value is DBNull ? dbNullSubstitute : (T)value;
        }


        public static T Get<T>(this IDataRecord record, string fieldName, T dbNullSubstitute = default(T))
        {
            return record[fieldName].DbCast(dbNullSubstitute);
        }

        public static void Get<T>(this IDataRecord record, string fieldName, Action<object> cast, T dbNullSubstitute = default(T))
        {
            var value = record[fieldName];
            cast(value is DBNull ? dbNullSubstitute : value);
        }

        public static IEnumerable<IDataRecord> Buffer(this IEnumerable<IDataRecord> source)
        {
            return source.Select(rec => rec.Copy());
        }

        public static IEnumerable<dynamic> BufferDynamic(this IEnumerable<IDataRecord> source)
        {
            return source.Select(rec => rec.ToDynamic());
        }

        public static IDataRecord Copy(this IDataRecord rec)
        {
            return new DataRecord(rec);
        }

        public static dynamic ToDynamic(this IDataRecord rec)
        {
            dynamic expando = new ExpandoObject();
            var map = (IDictionary<string, object>) expando;

            for (var i = 0; i != rec.FieldCount; i++)
            {
                var val = rec[i];

                if (val is DBNull)
                {
                    val = null;
                }

                map[rec.GetName(i)] = val;
            }

            return expando;
        }
    }
}

