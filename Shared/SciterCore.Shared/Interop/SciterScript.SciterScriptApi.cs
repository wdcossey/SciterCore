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
        [Obsolete]
        [StructLayout(LayoutKind.Sequential)]
		internal readonly struct SciterScriptApi
		{
			public readonly SciterScriptApiDelegates.CreateVM CreateVM;
			public readonly SciterScriptApiDelegates.DestroyVM DestroyVM;
			public readonly SciterScriptApiDelegates.InvokeGC InvokeGC;
			public readonly SciterScriptApiDelegates.SetStdStreams SetStdStreams;
			public readonly SciterScriptApiDelegates.GetCurrentVM GetCurrentVm;
			public readonly SciterScriptApiDelegates.GetGlobalNS GetGlobalNS;
			public readonly SciterScriptApiDelegates.GetCurrentNS GetCurrentNS;

			#region is
			
			public readonly SciterScriptApiDelegates.IsInt IsInt;
			public readonly SciterScriptApiDelegates.IsFloat IsFloat;
			public readonly SciterScriptApiDelegates.IsSymbol IsSymbol;
			public readonly SciterScriptApiDelegates.IsString IsString;
			public readonly SciterScriptApiDelegates.IsArray IsArray;
			public readonly SciterScriptApiDelegates.IsObject IsObject;
			public readonly SciterScriptApiDelegates.IsNativeObject IsNativeObject;
			public readonly SciterScriptApiDelegates.IsFunction IsFunction;
			public readonly SciterScriptApiDelegates.IsNativeFunction IsNativeFunction;
			public readonly SciterScriptApiDelegates.IsInstanceOf IsInstanceOf;
			public readonly SciterScriptApiDelegates.IsUndefined IsUndefined;
			public readonly SciterScriptApiDelegates.IsNothing IsNothing;
			public readonly SciterScriptApiDelegates.IsNull IsNull;
			public readonly SciterScriptApiDelegates.IsTrue IsTrue;
			public readonly SciterScriptApiDelegates.IsFalse IsFalse;
			public readonly SciterScriptApiDelegates.IsClass IsClass;
			public readonly SciterScriptApiDelegates.IsError IsError;
			public readonly SciterScriptApiDelegates.IsBytes IsBytes;
			public readonly SciterScriptApiDelegates.IsDateTime IsDateTime;
			
			#endregion

			#region get
			
			public readonly SciterScriptApiDelegates.GetIntValue GetIntValue;
			public readonly SciterScriptApiDelegates.GetFloatValue GetFloatValue;
			public readonly SciterScriptApiDelegates.GetBoolValue GetBoolValue;
			public readonly SciterScriptApiDelegates.GetSymbolValue GetSymbolValue;
			public readonly SciterScriptApiDelegates.GetStringValue GetStringValue;
			public readonly SciterScriptApiDelegates.GetBytes GetBytes;
			public readonly SciterScriptApiDelegates.GetDateTime GetDatetime;
			
			#endregion

			#region value

			public readonly SciterScriptApiDelegates.NothingValue NothingValue;
			public readonly SciterScriptApiDelegates.UndefinedValue UndefinedValue;
			public readonly SciterScriptApiDelegates.NullValue NullValue;
			public readonly SciterScriptApiDelegates.BoolValue BoolValue;
			public readonly SciterScriptApiDelegates.IntValue IntValue;
			public readonly SciterScriptApiDelegates.FloatValue FloatValue;
			public readonly SciterScriptApiDelegates.StringValue StringValue;
			public readonly SciterScriptApiDelegates.SymbolValue SymbolValue;
			public readonly SciterScriptApiDelegates.BytesValue BytesValue;
			public readonly SciterScriptApiDelegates.DateTimeValue DateTimeValue;

			#endregion
			
			public readonly SciterScriptApiDelegates.To_String To_String;

			#region define native class
			
			public readonly SciterScriptApiDelegates.DefineClass DefineClass;
			
			#endregion

			#region object
			
			public readonly SciterScriptApiDelegates.CreateObject CreateObject;
			public readonly SciterScriptApiDelegates.SetProp SetProp;
			public readonly SciterScriptApiDelegates.GetProp GetProp;
			public readonly SciterScriptApiDelegates.ForEachProp ForEachProp;
			public readonly SciterScriptApiDelegates.GetInstanceData GetInstanceData;
			public readonly SciterScriptApiDelegates.SetInstanceData SetInstanceData;

			#endregion
			
			#region array
			
			public readonly SciterScriptApiDelegates.CreateArray CreateArray;
			public readonly SciterScriptApiDelegates.SetElem SetElem;
			public readonly SciterScriptApiDelegates.GetElem GetElem;
			public readonly SciterScriptApiDelegates.SetArraySize SetArraySize;
			public readonly SciterScriptApiDelegates.GetArraySize GetArraySize;

			#endregion

			#region eval
			
			public readonly SciterScriptApiDelegates.Eval Eval;
			public readonly SciterScriptApiDelegates.EvalString EvalString;

			#endregion
			
			#region call function (method)
			
			public readonly SciterScriptApiDelegates.CallFunction CallFunction;

			#endregion

			#region compiled bytecodes
			
			public readonly SciterScriptApiDelegates.CompileByteCode CompileByteCode;
			public readonly SciterScriptApiDelegates.LoadByteCode LoadByteCode;
			
			#endregion
			
			#region throw error
			
			public readonly SciterScriptApiDelegates.ThrowError ThrowError;

			#endregion
			
			#region  arguments access
			
			public readonly SciterScriptApiDelegates.GetArgCount GetArgCount;
			public readonly SciterScriptApiDelegates.GetArgN GetArgN;

			#endregion
			
			// path here is global "path" of the object, something like
			// "one"
			// "one.two", etc.
			public readonly SciterScriptApiDelegates.GetValueByPath GetValueByPath;

			#region pins
			
			public readonly SciterScriptApiDelegates.Pin Pin;

			public readonly SciterScriptApiDelegates.Unpin Unpin;
			
			#endregion

			// create native_function_value and native_property_value,
			// use this if you want to add native functions/properties in runtime to exisiting classes or namespaces (including global ns)
			public readonly SciterScriptApiDelegates.NativeFunctionValue NativeFunctionValue;
			public readonly SciterScriptApiDelegates.NativePropertyValue NativePropertyValue;

			// Schedule execution of the pfunc(prm) in the thread owning this VM.
			// Used when you need to call scripting methods from threads other than main (GUI) thread
			// It is safe to call tiscript functions inside the pfunc.
			// returns 'true' if scheduling of the call was accepted, 'false' when failure (VM has no dispatcher attached).
			public readonly SciterScriptApiDelegates.Post Post;

			public readonly SciterScriptApiDelegates.SetRemoteStdStreams SetRemoteStdStreams;

			// support of multi-return values from native functions, n here is a number 1..64
			public readonly SciterScriptApiDelegates.MakeValList MakeValList;

			// returns number of props in object, elements in array, or bytes in byte array.
			public readonly SciterScriptApiDelegates.GetLength GetLength;

			// for( var val in coll ) {...}
			public readonly SciterScriptApiDelegates.GetNext GetNext;

			// for( var (key,val) in coll ) {...}
			public readonly SciterScriptApiDelegates.GetNextKeyValue GetNextKeyValue;

			// associate extra data pointer with the VM
			public readonly SciterScriptApiDelegates.SetExtraData SetExtraData;
			public readonly SciterScriptApiDelegates.GetExtraData GetExtraData;
		}
	}
}