using System;

namespace JsonSchema
{
    public class NotSupportedTypeException : Exception
    {
        public NotSupportedTypeException(Type type) 
            : base($"The type {type.Name} is not supported")
        {
        }
    }
}