using System;

// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public class EngineDestroyedEventArgs : EventArgs
    {
        internal EngineDestroyedEventArgs(IntPtr windowHandle, uint code)
        {
            Window = new SciterWindow(windowHandle);
            Code = (CallbackCode)unchecked((int)code);
        }

        public SciterWindow Window { get; }

        public CallbackCode Code { get; }
    }
}