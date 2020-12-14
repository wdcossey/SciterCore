using System;

namespace SciterCore.Interop
{
    [AttributeUsage(AttributeTargets.Delegate)]
    internal class SciterStructMapAttribute : Attribute
    {
        public SciterStructMapAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}