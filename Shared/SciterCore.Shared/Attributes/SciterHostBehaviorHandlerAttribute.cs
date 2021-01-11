#if NETCORE

using System;
using SciterTest.CoreForms.Extensions;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SciterHostBehaviorHandlerAttribute : Attribute
    {
        public Type Type { get; }

        public SciterHostBehaviorHandlerAttribute(Type type)
        {
            Type = type.Validate<SciterEventHandler>();
        }
    }
}

#endif