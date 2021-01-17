#if NETCORE

using System;
using SciterTest.CoreForms.Extensions;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SciterHostWindowAttribute : Attribute
    {
        public string HomePage { get; }
        public int? Width { get; }
        public int? Height { get; }
        public string Title { get; }
        public Type Type { get; }

        public SciterHostWindowAttribute(Type type, string homePage = null)
        {
            HomePage = homePage;
            Type = type.Validate<SciterWindow>();
        }

        public SciterHostWindowAttribute(string homePage, int width, int height, string title)
        {
            HomePage = homePage;
            Width = width;
            Height = height;
            Title = title;
            Type = typeof(SciterWindow);
        }
        
    }
}

#endif