using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace JsonSchema
{
    public static class JsonSchemaGenerator
    {
        public static string GenerateFor<T>(ICustomTypeMapper customTypeMapper = null)
        {
            return GenerateFor(typeof(T), customTypeMapper);
        }
        
        public static string GenerateFor(Type type, ICustomTypeMapper customTypeMapper)
        {
            using var stream = new MemoryStream();
            using var jsonWriter = new Utf8JsonWriter(stream);
            
            jsonWriter.WriteStartObject();
            
            GenerateSchema(type, jsonWriter, customTypeMapper);
            
            jsonWriter.WriteEndObject();
            
            jsonWriter.Flush();
            
            return Encoding.UTF8.GetString(stream.ToArray());
            
        }
        
        private static Utf8JsonWriter GenerateSchema(
            Type type, 
            Utf8JsonWriter writer,
            ICustomTypeMapper customTypeMapper)
        {
            var info = type.GetProperties().Where(x => x.MemberType == MemberTypes.Property);
            
            foreach (var propInfo in info)
            {
                var customMapping = customTypeMapper?.MapperFrom(propInfo);
                if (customMapping != null)
                {
                    customMapping(writer);
                    writer.Flush();
                } 
                else if (TypeMapper.IsString(propInfo))
                {
                    writer.WriteString(propInfo.Name, "string");
                    writer.Flush();
                }
                else if (TypeMapper.IsBoolean(propInfo))
                {
                    writer.WriteBoolean(propInfo.Name, true);
                    writer.Flush();
                }
                else if (TypeMapper.IsInteger(propInfo))
                {
                    writer.WriteNumber(propInfo.Name, new Random().Next(1, 100));
                    writer.Flush();
                }
                else if (TypeMapper.IsByte(propInfo))
                {
                    writer.WriteNumber(propInfo.Name, byte.MaxValue);
                    writer.Flush();
                }
                else if (TypeMapper.IsDecimal(propInfo))
                {
                    writer.WriteNumber(propInfo.Name, float.MaxValue);
                    writer.Flush();
                }
                else if (TypeMapper.IsDateTime(propInfo))
                {
                    writer.WriteString(propInfo.Name, DateTime.UtcNow);
                    writer.Flush();
                }
                else if (TypeMapper.IsGuid(propInfo))
                {
                    writer.WriteString(propInfo.Name, Guid.NewGuid());
                    writer.Flush();
                }
                else if (propInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    var genericType = propInfo.PropertyType.GenericTypeArguments[0];
                    
                    writer.WriteStartArray(propInfo.Name);
                    
                    writer.WriteStartObject();

                    GenerateSchema(genericType, writer, customTypeMapper);
                    
                    writer.WriteEndObject();
                    
                    writer.WriteEndArray();
                } else if (Type.GetTypeCode(propInfo.PropertyType) == TypeCode.Object)
                {
                    writer.WriteStartObject(propInfo.Name);

                    GenerateSchema(propInfo.PropertyType, writer, customTypeMapper);
                    
                    writer.WriteEndObject();
                }
                else
                {
                    throw new NotSupportedTypeException(propInfo.PropertyType);
                }
            }
        
            return writer;
        }
    }
}