using System;

namespace SciterCore.Convert
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class SciterPropertyName : Attribute
    {
        public string PropertyName { get; }

        public SciterPropertyName(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}