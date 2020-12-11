using System;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore.PlatformWrappers
{
    public static partial class SciterWindowWrapper
    {
        internal static class NativeMethodWrapper
        {
            public static ISciterWindowWrapper GetInterface()
            {
                //var sciterApiPtr = SciterAPI();
                
                //TODO: this should not new up every call.
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return new WindowsWrapper();

                //if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                //    return new NativeWrapper<Sciter.MacOsSciterApi>();
//
                //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                //    return new NativeWrapper<Sciter.LinuxSciterApi>();

                throw new PlatformNotSupportedException();
            }
        }
    }
}