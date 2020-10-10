using System;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SciterCallbackWrapperAttribute : Attribute
    {
        public SciterCallbackWrapperAttribute()
        {
            
        }
    }
}