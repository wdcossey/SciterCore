#if NETCORE

using System;
using SciterTest.CoreForms.Extensions;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SciterHostWindowAttribute : Attribute
    {
        public string HomePage { get; }
        public Type Type { get; }

        public SciterHostWindowAttribute(Type type, string homePage = null)
        {
            HomePage = homePage;
            Type = type.Validate<SciterWindow>();
        }
    }
}

#endif