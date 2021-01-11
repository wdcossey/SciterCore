using System;

namespace SciterTest.CoreForms.Extensions
{
    public static class TypeExtensions
    {
        public static bool Implements<TType>(this Type type)
        {
            return type != null && typeof(TType).IsAssignableFrom(type);
        }
        
        public static Type Validate<TType>(this Type type, bool permitNull = true)
        {
            if (permitNull == false && type == null)
                throw new ArgumentNullException($"{nameof(type)}");
            
            if (type != null && type.Implements<TType>() == false)
                throw new InvalidOperationException($"`{nameof(type)}` must be assignable from `{typeof(TType)}`, got `{type}`.");

            return type;
        }
    }
}