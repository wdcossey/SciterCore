using System;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SciterFunctionNameAttribute : Attribute
    {
        public string FunctionName { get; }

        public SciterFunctionNameAttribute(string functionName)
        {
            FunctionName = functionName;
        }
    }
}