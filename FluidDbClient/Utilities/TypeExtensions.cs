using System;

namespace FluidDbClient
{
    public static class TypeExtensions
    {
        public static Type GetUnderlyingScalarFieldType(this Type t)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;

            if (t.IsEnum)
            {
                t = t.GetEnumUnderlyingType();
            }
            
            return t;
        }
    }
}
