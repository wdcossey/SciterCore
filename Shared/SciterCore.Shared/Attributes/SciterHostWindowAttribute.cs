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
        
        public Type WindowType { get; }

        public SciterHostWindowAttribute(Type type, string homePage = null)
        {
            HomePage = homePage;
            WindowType = type.Validate<SciterWindow>();
        }

        public SciterHostWindowAttribute(string homePage = null, int width = 800, int height = 600, string title = null)
        {
            HomePage = homePage;
            Width = width;
            Height = height;
            Title = title;
        }
        
    }
}

#endif