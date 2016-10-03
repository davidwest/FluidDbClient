
using System;
using System.Collections.Generic;
using System.Data;
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

        public static T Get<T>(this IDictionary<string, object> record, string fieldName, T dbNullSubstitute = default(T))
        {
            return record[fieldName].DbCast(dbNullSubstitute);
        }

        public static IDataRecord Copy(this IDataRecord rec)
        {
            return new DataRecord(rec);
        }

        public static Dictionary<string, object> ToDictionary(this IDataRecord rec)
        {
            var dictionary = new Dictionary<string, object>();

            for (var j = 0; j != rec.FieldCount; j++)
            {
                dictionary.Add(rec.GetName(j), rec.GetValue(j));
            }

            return dictionary;
        }

        public static IEnumerable<Dictionary<string, object>> ToDictionaries(this IEnumerable<IDataRecord> records)
        {
            return records.Select(rec => rec.ToDictionary());
        }

        public static IEnumerable<IDataRecord> Buffer(this IEnumerable<IDataRecord> source)
        {
            return source.Select(rec => rec.Copy());
        }
    }
}

