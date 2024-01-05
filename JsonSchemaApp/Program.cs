using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JsonSchemaApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var schema = new StringBuilder();
            
            schema.Append("{\n\r");

            schema = GenerateSchema(typeof(User), schema);

            schema.Append("\n\r}");

            Console.WriteLine(schema);
        }

        static StringBuilder GenerateSchema(Type type, StringBuilder schema, int level = 1)
        {
            var propPrefix = BuildPrefix(level);

            var info = type.GetProperties().Where(x => x.MemberType == MemberTypes.Property);

            var props = new List<string>();

            foreach (var propInfo in info)
            {
                if (IsOfType(propInfo, typeof(string)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":\"string\"");
                }
                else if (IsOfType(propInfo, typeof(bool), typeof(bool?)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":\"bool\"");
                }
                else if (IsOfType(propInfo, typeof(int), typeof(int?))
                    || IsOfType(propInfo, typeof(short), typeof(short?))
                    || IsOfType(propInfo, typeof(long), typeof(long?)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":int");
                }
                else if (IsOfType(propInfo, typeof(byte), typeof(byte?)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":byte");
                }
                else if (IsOfType(propInfo, typeof(float), typeof(float?))
                    || IsOfType(propInfo, typeof(decimal), typeof(decimal?))
                    || IsOfType(propInfo, typeof(double), typeof(double?)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":decimal");
                }
                else if (IsOfType(propInfo, typeof(DateTime), typeof(DateTime?)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":\"{DateTime.Now}\"");
                }
                else if (IsOfType(propInfo, typeof(Guid), typeof(Guid?)))
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":\"{Guid.NewGuid()}\"");
                }
                else if (propInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    var genericType = propInfo.PropertyType.GenericTypeArguments[0];
                    props.Add($"{propPrefix}\"{propInfo.Name}\":[\n\r{propPrefix + propPrefix}{{\r\n{GenerateSchema(genericType, new StringBuilder(), level + 2)}\n\r{propPrefix + propPrefix}}}\r\n{propPrefix}]");
                } else if (Type.GetTypeCode(propInfo.PropertyType) == TypeCode.Object)
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":{{\r\n{GenerateSchema(propInfo.PropertyType, new StringBuilder(), level + 2)}\n\r{propPrefix}}}");
                }
                else
                {
                    props.Add($"{propPrefix}\"{propInfo.Name}\":\"{propInfo.PropertyType}\"");
                }

            }

            schema.Append(string.Join(",\n\r", props));

            return schema;
        }

        private static bool IsOfType(PropertyInfo propInfo, params Type[] types)
        {
            return types.Contains(propInfo.PropertyType);
        }

        private static string BuildPrefix(int level)
        {
            var prefix = string.Empty;

            for (int i = 0; i < level; i++)
            {
                prefix += " ";
            }

            return prefix;
        }
    }

    class User {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public Address OtherAddress { get; set; }
    }

    class Address
    {
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
