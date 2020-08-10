using System;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SciterBehaviorAttribute : Attribute
    {

        public string Name { get; }

        public SciterBehaviorAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

    }
}
