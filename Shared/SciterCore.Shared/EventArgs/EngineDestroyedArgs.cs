using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    /// <summary>
    /// SCN_ENGINE_DESTROYED
    /// </summary>
    public readonly struct EngineDestroyedArgs 
    {
        internal EngineDestroyedArgs(IntPtr windowHandle, uint code)
        : this()
        {
            WindowHandle = windowHandle;
            Code = (CallbackCode)unchecked((int)code);
        }

        public IntPtr WindowHandle { get; }
        
        public CallbackCode Code { get; }
    }
}