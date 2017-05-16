using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace FluidDbClient
{
    public static class ModelMappingExtensions
    {
        private static readonly char[] Delimeters = { '_', '-' };

        public static T Map<T>(this IDataRecord r) where T : class, new()
        {
            var obj = new T();
            var type = obj.GetType();

            var typeName = type.Name;

            var namedValues =
                Enumerable.Range(0, r.FieldCount)
                .Select(i => new
                {
                    CandidateNames = GetCandidateNames(typeName, r.GetName(i)),
                    Value = GetEffectiveValue(r.GetValue(i))
                });

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pair in namedValues)
            {
                var matchingProperty = properties.FirstOrDefault(p => pair.CandidateNames.Contains(p.Name));
                if (matchingProperty == null) continue;

                matchingProperty.SetValue(obj, pair.Value, null);
            }

            return obj;
        }

        public static IEnumerable<T> Map<T>(this IEnumerable<IDataRecord> records) where T : class, new()
        {
            return records.Select(r => r.Map<T>());
        }

        public static T1 Map<T1, T2>(this IDataRecord r,
                                     Func<IDataRecord, T1> map1,
                                     Func<IDataRecord, T2> map2,
                                     Action<T1, T2> combine)
        {
            var t1 = map1(r);

            combine(t1, map2(r));

            return t1;
        }

        public static T1 Map<T1, T2>(this IDataRecord record, Action<T1, T2> combine) 
            where T1 : class, new()
            where T2 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), combine);
        }

        public static IEnumerable<T1> Map<T1, T2>(this IEnumerable<IDataRecord> records, Action<T1, T2> combine)
            where T1 : class, new()
            where T2 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static T1 Map<T1, T2, T3>(this IDataRecord r,
                                         Func<IDataRecord, T1> map1,
                                         Func<IDataRecord, T2> map2,
                                         Func<IDataRecord, T3> map3,
                                         Action<T1, T2, T3> combine)
        {
            var t1 = map1(r);

            combine(t1, map2(r), map3(r));

            return t1;
        }

        public static T1 Map<T1, T2, T3>(this IDataRecord record, Action<T1, T2, T3> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), r => r.Map<T3>(), combine);
        }

        public static IEnumerable<T1> Map<T1, T2, T3>(this IEnumerable<IDataRecord> records, Action<T1, T2, T3> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static T1 Map<T1, T2, T3, T4>(this IDataRecord r,
                                             Func<IDataRecord, T1> map1,
                                             Func<IDataRecord, T2> map2,
                                             Func<IDataRecord, T3> map3,
                                             Func<IDataRecord, T4> map4,
                                             Action<T1, T2, T3, T4> combine)
        {
            var t1 = map1(r);

            combine(t1, map2(r), map3(r), map4(r));

            return t1;
        }

        public static T1 Map<T1, T2, T3, T4>(this IDataRecord record, Action<T1, T2, T3, T4> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), r => r.Map<T3>(), r => r.Map<T4>(), combine);
        }

        public static IEnumerable<T1> Map<T1, T2, T3, T4>(this IEnumerable<IDataRecord> records, Action<T1, T2, T3, T4> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }


        private static object GetEffectiveValue(object fieldValue)
        {
            if (fieldValue is DBNull) return null;

            return fieldValue;
        }

        private static IEnumerable<string> GetCandidateNames(string typeName, string fieldName)
        {
            var typeNameIndex = fieldName.IndexOf(typeName, StringComparison.OrdinalIgnoreCase);

            if (typeNameIndex == 0 && fieldName.Length > typeName.Length)
            {
                var nextChar = fieldName[typeName.Length];

                var readFromIndex =
                    Delimeters.Any(d => d == nextChar) && fieldName.Length > typeName.Length + 1
                        ? typeName.Length + 1
                        : typeName.Length;

                var simpleName = fieldName.Substring(readFromIndex);

                yield return simpleName;
            }

            yield return fieldName;
        }
    }
}
