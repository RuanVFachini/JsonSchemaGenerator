using System;
using System.Reflection;
using System.Text.Json;

namespace JsonSchema
{
    public interface ICustomTypeMapper
    {
        public Action<Utf8JsonWriter> MapperFrom(PropertyInfo propertyInfo);
    }
}