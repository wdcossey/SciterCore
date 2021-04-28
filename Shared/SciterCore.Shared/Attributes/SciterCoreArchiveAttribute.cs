using System;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SciterCoreArchiveAttribute : Attribute
    {
        public string Uri { get; }
        public string ResourceName { get; }

        public SciterCoreArchiveAttribute(string uri, string resourceName)
        {
            Uri = uri;
            ResourceName = resourceName;
        }
    }
}