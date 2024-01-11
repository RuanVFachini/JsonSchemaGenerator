using System.Reflection;
using System.Text.Json;

namespace JsonSchema.Tests;

public class CustomTypeMapper : ICustomTypeMapper
{
    public Action<Utf8JsonWriter>? MapperFrom(PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType == typeof(string))
        {
            return writer =>
            {
                writer.WriteStartObject(propertyInfo.Name);
                writer.WriteString("Animal", "the dog");
                writer.WriteEndObject();
            };
        }

        return null;
    }
}