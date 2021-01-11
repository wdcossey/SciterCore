#if NETCORE

using System;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SciterHostArchiveAttribute : Attribute
    {
        public string BaseUrl { get; }

        public SciterHostArchiveAttribute(string baseUrl = SciterArchive.DEFAULT_ARCHIVE_URI)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));
            
            BaseUrl = baseUrl;
        }
    }
}

#endif