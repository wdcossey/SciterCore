using System;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SciterCoreArchiveAttribute : Attribute
    {
        public string Uri { get; }
        
        public string ResourceName { get; }
        
        public string[] InitScripts { get; }

        public SciterCoreArchiveAttribute(string uri, string resourceName = "SciterResource", params string[] initScripts)
        {
            Uri = uri;
            ResourceName = resourceName;
            InitScripts = initScripts;
        }
    }
}