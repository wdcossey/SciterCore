#if NETCORE

using System;
using SciterTest.CoreForms.Extensions;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SciterHostEventHandlerAttribute : Attribute
    {
        public Type Type { get; }

        public SciterHostEventHandlerAttribute(Type type)
        {
            Type = type.Validate<SciterEventHandler>();
        }
    }
}

#endif