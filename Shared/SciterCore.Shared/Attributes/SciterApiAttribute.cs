using System;
using System.Runtime.InteropServices;

namespace SciterCore.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    // ReSharper disable InconsistentNaming
    internal class SciterApiOSPlatformAttribute : Attribute
    {
        public SciterOSPlatform Platform { get; }

        public SciterApiOSPlatformAttribute(SciterOSPlatform osPlatform)
        {
            Platform = osPlatform;
        }
    }
    // ReSharper restore InconsistentNaming

    [AttributeUsage(AttributeTargets.Field)]
    internal class SciterApiMinVersionAttribute : Attribute
    {
        public Version Version { get; }

        public SciterApiMinVersionAttribute(int major, int minor, int build, int revision)
        {
            Version = new Version(major, minor, build, revision);
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    internal class SciterApiMaxVersionAttribute : Attribute
    {
        public Version Version { get; }

        public SciterApiMaxVersionAttribute(int major, int minor, int build, int revision)
        {
            Version = new Version(major, minor, build, revision);
        }
    }
}