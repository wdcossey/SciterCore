using System;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore.PlatformWrappers
{
    public static partial class SciterWindowWrapper
    {
        internal const SciterXDef.SCITER_CREATE_WINDOW_FLAGS DefaultCreateFlags =
            SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
            SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
            SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
            SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS;
        
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