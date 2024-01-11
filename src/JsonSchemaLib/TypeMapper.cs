using System;
using System.Linq;
using System.Reflection;

namespace JsonSchema
{
    public static class TypeMapper
    {
        public static bool IsGuid(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(Guid), typeof(Guid?));
        }

        public static bool IsDateTime(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(DateTime), typeof(DateTime?));
        }

        public static bool IsByte(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(byte), typeof(byte?));
        }

        public static bool IsBoolean(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(bool), typeof(bool?));
        }

        public static bool IsString(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(string));
        }

        public static bool IsDecimal(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(float), typeof(float?))
                   || IsOfType(propInfo, typeof(decimal), typeof(decimal?))
                   || IsOfType(propInfo, typeof(double), typeof(double?));
        }

        public static bool IsInteger(PropertyInfo propInfo)
        {
            return IsOfType(propInfo, typeof(int), typeof(int?))
                   || IsOfType(propInfo, typeof(short), typeof(short?))
                   || IsOfType(propInfo, typeof(long), typeof(long?));
        }

        private static bool IsOfType(PropertyInfo propInfo, params Type[] types)
        {
            return types.Contains(propInfo.PropertyType);
        }
    }
}