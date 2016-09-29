
using System.Linq;
using System.Collections.Generic;
using System.Text;


namespace FluidDbClient
{
    internal static class InternalTextExtensions
    {
        // http://stackoverflow.com/questions/521146/c-sharp-split-string-but-keep-split-chars-separators
        public static IEnumerable<string> ToDelimitedArray(this string s, char[] delims)
        {
            int start = 0, index;

            while ((index = s.IndexOfAny(delims, start)) != -1)
            {
                if (index - start > 0)
                    yield return s.Substring(start, index - start);
                yield return s.Substring(index, 1);
                start = index + 1;
            }

            if (start < s.Length)
            {
                yield return s.Substring(start);
            }
        }


        public static string ConcatRepeat(this string original, int repetitions)
        {
            var builder = new StringBuilder(original);

            while (repetitions > 0)
            {
                builder.Append(original);
                repetitions--;
            }

            return builder.ToString();
        }


        public static string SerializeToString<T>(this IEnumerable<T> values)
        {
            return values.SerializeToString(string.Empty, string.Empty, string.Empty);
        }

        public static string SerializeToString<T>(this IEnumerable<T> values,
                                                  char separator,
                                                  string wrapperStart = "",
                                                  string wrapperEnd = "")
        {
            return values.SerializeToString(separator.ToString(), wrapperStart, wrapperEnd);
        }

        public static string SerializeToString<T>(this IEnumerable<T> values,
                                                  string separator,
                                                  string wrapperStart = "",
                                                  string wrapperEnd = "")
        {
            var list = values.ToList();

            if (list.Count == 0)
            {
                return string.Empty;
            }

            var result = list.Select(val => $"{wrapperStart}{val}{wrapperEnd}")
                             .Aggregate((a, b) => $"{a}{separator}{b}");

            return result;
        }


        public static string ToCsv<T>(this IEnumerable<T> values)
        {
            return SerializeToString(values, ", ");
        }

        public static string ToSingleQuotedCsv<T>(this IEnumerable<T> values)
        {
            return SerializeToString(values, ", ", "'", "'");
        }

        public static string ToDoubleQuotedCsv<T>(this IEnumerable<T> values)
        {
            return SerializeToString(values, ", ", "\"", "\"");
        }

        public static string Wrap(this string originalText, string prefix, string postfix)
        {
            return prefix + originalText + postfix;
        }

        public static string Wrap(this string originalText, string wrapper)
        {
            return Wrap(originalText, wrapper, wrapper);
        }

        public static string WrapSingleQuotes(this string originalText)
        {
            return Wrap(originalText, "'");
        }

        public static string WrapDoubleQuotes(this string originalText)
        {
            return Wrap(originalText, @"""");
        }
    }
}
