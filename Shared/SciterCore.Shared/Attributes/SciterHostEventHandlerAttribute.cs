using System;
using SciterTest.CoreForms.Extensions;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SciterHostEventHandlerAttribute : Attribute
    {
        public Type EventHandlerType { get; }

        public SciterHostEventHandlerAttribute(Type eventHandlerType)
        {
            EventHandlerType = eventHandlerType.Validate<SciterEventHandler>();
        }
    }
}