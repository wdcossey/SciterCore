using System;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore.PlatformWrappers
{
    public static partial class SciterWindowWrapper
    {
        internal const CreateWindowFlags DefaultCreateFlags =
            CreateWindowFlags.Main |
            CreateWindowFlags.Titlebar |
            CreateWindowFlags.Resizeable |
            CreateWindowFlags.Controls;

        public static class NativeMethodWrapper
        {
            private static ISciterWindowWrapper _platformWrapper;

            public static ISciterWindowWrapper GetInterface()
            {
                if (_platformWrapper != null)
                    return _platformWrapper;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return _platformWrapper = new WindowsWrapper();

                //if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                //    return _platformWrapper = new MacOSWrapper>();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return _platformWrapper = new LinuxWrapper();

                throw new PlatformNotSupportedException();
            }
        }
    }
}