/*
    This file is provided for backwards compatibility ONLY!!!

    delegates, methods, classes, structs, etc may be incomplete or will require additional work on your part!
*/

using System;
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
    public static partial class SciterScript
    {
        [Obsolete("Removed in Sciter v4.4.3.24", false)]
        [StructLayout(LayoutKind.Sequential)]
        public struct ScriptValue
        {
            ulong val;

#pragma warning disable 618
            
            public bool IsInt => Sciter.ScriptApi.IsInt(this);
            
            public bool IsFloat => Sciter.ScriptApi.IsFloat(this);
            
            public bool IsSymbol => Sciter.ScriptApi.IsSymbol(this);
		
            public bool IsString => Sciter.ScriptApi.IsString(this);

            public bool IsArray => Sciter.ScriptApi.IsArray(this);
		
            public bool IsObject => Sciter.ScriptApi.IsObject(this);
		
            public bool IsNativeObject => Sciter.ScriptApi.IsNativeObject(this);
		
            public bool IsFunction => Sciter.ScriptApi.IsFunction(this);
		
            public bool IsNativeFunction => Sciter.ScriptApi.IsNativeFunction(this);

            public bool IsInstanceOf(ScriptValue cls) => Sciter.ScriptApi.IsInstanceOf(this, cls);
		
            public bool IsUndefined => Sciter.ScriptApi.IsUndefined(this);
		
            public bool IsNothing => Sciter.ScriptApi.IsNothing(this);
		
            public bool IsNull => Sciter.ScriptApi.IsNull(this);
		
            public bool IsTrue => Sciter.ScriptApi.IsTrue(this);
		
            public bool IsFalse => Sciter.ScriptApi.IsFalse(this);
		
            public bool IsClass(IntPtr vmPtr) => Sciter.ScriptApi.IsClass(vmPtr, this);
		
            public bool IsError => Sciter.ScriptApi.IsError(this);
		
            public bool IsBytes => Sciter.ScriptApi.IsBytes(this);
		
            public bool IsDateTime(IntPtr vmPtr) => Sciter.ScriptApi.IsDateTime(vmPtr, this);
            
            public int Length => Sciter.ScriptApi.GetLength(this);
            
            
#pragma warning restore 618
        }
    }
}