/*
    This file is provided for backwards compatibility ONLY!!!

    delegates, methods, classes, structs, etc may be incomplete or will require additional work on your part!
*/

using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
    public static partial class SciterScript
    {
        [Obsolete]
        [StructLayout(LayoutKind.Sequential)]
        public struct tiscript_class_def
        {
            public string name;
            public IntPtr methods;    // with these methods
            public IntPtr props;      // with these properties
            public IntPtr consts;     // with these constants (if any)
            public IntPtr get_item;   // var v = obj[idx]
            public IntPtr set_item;   // obj[idx] = v
            public IntPtr finalizer;  // destructor of native objects
            public IntPtr iterator;   // for(var el in collecton) handler
            public IntPtr on_gc_copy; // called by GC to notify that 'self' is moved to new location
            public IntPtr prototype;  // superclass, prototype for the class (or 0)
        }

        [Obsolete]
        [StructLayout(LayoutKind.Sequential)]
        public struct tiscript_method_def
        {
            public IntPtr dispatch;
            public IntPtr id;
            public string name;
            public Callbacks.tiscript_method  handler;  // or tiscript_tagged_method if tag is not 0
            public IntPtr tag;
            public IntPtr    payload;  // must be zero
        }

        [Obsolete]
        [StructLayout(LayoutKind.Sequential)]
        public struct tiscript_prop_def
        {
            /// <summary>
            /// a.k.a. VTBL
            /// </summary>
            public IntPtr dispatch;
            public uint id;
            public string name;
            public Callbacks.tiscript_get_prop getter;
            public Callbacks.tiscript_set_prop setter;
            public IntPtr tag;
        }
        
        [Obsolete]
        [StructLayout(LayoutKind.Sequential)]
        public struct tiscript_stream_vtbl
        {
            public IntPtr input;
            public IntPtr output;
            public IntPtr get_name;
            public IntPtr close;
        }

        [Obsolete]
        [StructLayout(LayoutKind.Sequential)]
        public struct tiscript_stream
        {
            public tiscript_stream_vtbl _vtbl;
        }

        /// <summary>
        /// pinned <see cref="ScriptValue"/>, val here will survive GC.
        /// </summary>
        [Obsolete("Removed in Sciter v4.4.3.24", false)]
        [StructLayout(LayoutKind.Sequential)]
        public readonly struct PinnedScriptValue
        {
            private readonly ScriptValue value;
            private readonly IntPtr vmPtr;
            private readonly IntPtr d1;
            private readonly IntPtr d2;

            public PinnedScriptValue(IntPtr vmPtr)
            {
                this.vmPtr = vmPtr;
                value = default;
                d1 = default;
                d2 = default;
            }
            
            public PinnedScriptValue(IntPtr vmPtr, ScriptValue value)
                : this(vmPtr)
            {
                this.value = value;
            }

            public IntPtr GetVM() =>
                vmPtr;
            
            public ScriptValue Value() => 
                value;
        }
    }
}