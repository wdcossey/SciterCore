using System;
using System.Runtime.InteropServices;

// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore.Interop
{
    public static partial class Sciter
    {
        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct SciterVersionApi
        {
#pragma warning disable 649
            public readonly int version;
            public readonly IntPtr unused;
            public readonly SciterApiDelegates.SciterVersion SciterVersion;
#pragma warning restore 649
        }
    }
}