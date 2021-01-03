/*
    This file is provided for backwards compatibility ONLY!!!

    delegates, methods, classes, structs, etc may be incomplete or will require additional work on your part!
*/

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SciterCore.Interop
{
    [Obsolete("Removed in Sciter v4.4.3.24", false)]
	public static partial class SciterScript
    {
		[Obsolete]
        internal static class UnsafeNativeMethods
		{
			public static ISciterScriptApi GetApiInterface(ISciterApi sciterApi)
			{
#pragma warning disable 618
				var sciterScriptApi = sciterApi.GetTIScriptApi();
#pragma warning restore 618
				
				return new NativeSciterScriptApiWrapper<SciterScriptApi>(sciterScriptApi);
			}

			private sealed class NativeSciterScriptApiWrapper<TStruct> : ISciterScriptApi
				where TStruct : struct
			{
				private IntPtr _apiPtr;

#pragma warning disable 649
				
				private readonly SciterScriptApiDelegates.CreateVM _createVM;
				private readonly SciterScriptApiDelegates.DestroyVM _destroyVM;
				private readonly SciterScriptApiDelegates.InvokeGC _invokeGc;
				private readonly SciterScriptApiDelegates.SetStdStreams _setStdStreams;
				private readonly SciterScriptApiDelegates.GetCurrentVM _getCurrentVM;
				private readonly SciterScriptApiDelegates.GetGlobalNS _getGlobalNS;
				private readonly SciterScriptApiDelegates.GetCurrentNS _getCurrentNS;
				
				private readonly SciterScriptApiDelegates.IsInt _isInt;
				private readonly SciterScriptApiDelegates.IsFloat _isFloat;
				private readonly SciterScriptApiDelegates.IsSymbol _isSymbol;
				private readonly SciterScriptApiDelegates.IsString _isString;
				private readonly SciterScriptApiDelegates.IsArray _isArray;
				private readonly SciterScriptApiDelegates.IsObject _isObject;
				private readonly SciterScriptApiDelegates.IsNativeObject _isNativeObject;
				private readonly SciterScriptApiDelegates.IsFunction _isFunction;
				private readonly SciterScriptApiDelegates.IsNativeFunction _isNativeFunction;

				private readonly SciterScriptApiDelegates.IsInstanceOf _isInstanceOf;
				private readonly SciterScriptApiDelegates.IsUndefined _isUndefined;
				private readonly SciterScriptApiDelegates.IsNothing _isNothing;
				private readonly SciterScriptApiDelegates.IsNull _isNull;
				private readonly SciterScriptApiDelegates.IsTrue _isTrue;
				private readonly SciterScriptApiDelegates.IsFalse _isFalse;
				private readonly SciterScriptApiDelegates.IsClass _isClass;
				private readonly SciterScriptApiDelegates.IsError _isError;
				private readonly SciterScriptApiDelegates.IsBytes _isBytes;
				private readonly SciterScriptApiDelegates.IsDateTime _isDateTime;
                
				private readonly SciterScriptApiDelegates.GetIntValue _getIntValue;
				private readonly SciterScriptApiDelegates.GetFloatValue _getFloatValue;
				private readonly SciterScriptApiDelegates.GetBoolValue _getBoolValue;
				private readonly SciterScriptApiDelegates.GetSymbolValue _getSymbolValue;
				private readonly SciterScriptApiDelegates.GetStringValue _getStringValue;
				private readonly SciterScriptApiDelegates.GetBytes _getBytes;
				private readonly SciterScriptApiDelegates.GetDateTime _getDateTime;

				private readonly SciterScriptApiDelegates.NothingValue _nothingValue;
				private readonly SciterScriptApiDelegates.UndefinedValue _undefinedValue;
				private readonly SciterScriptApiDelegates.NullValue _nullValue;
				private readonly SciterScriptApiDelegates.BoolValue _boolValue;
				private readonly SciterScriptApiDelegates.IntValue _intValue;
				private readonly SciterScriptApiDelegates.FloatValue _floatValue;
				private readonly SciterScriptApiDelegates.StringValue _stringValue;
				private readonly SciterScriptApiDelegates.SymbolValue _symbolValue;
				private readonly SciterScriptApiDelegates.BytesValue _bytesValue;
				private readonly SciterScriptApiDelegates.DateTimeValue _dateTimeValue;
				
				private readonly SciterScriptApiDelegates.To_String _toString;
                
				private readonly SciterScriptApiDelegates.DefineClass _defineClass;

				#region object

				private readonly SciterScriptApiDelegates.CreateObject _createObject;
				private readonly SciterScriptApiDelegates.SetProp _setProp;
				private readonly SciterScriptApiDelegates.GetProp _getProp;
				private readonly SciterScriptApiDelegates.ForEachProp _forEachProp;
				private readonly SciterScriptApiDelegates.GetInstanceData _getInstanceData;
				private readonly SciterScriptApiDelegates.SetInstanceData _setInstanceData;

				#endregion

				#region array

				private readonly SciterScriptApiDelegates.CreateArray _createArray;
                private readonly SciterScriptApiDelegates.SetElem _setElem;
                private readonly SciterScriptApiDelegates.GetElem _getElem;
                private readonly SciterScriptApiDelegates.SetArraySize _setArraySize;
				private readonly SciterScriptApiDelegates.GetArraySize _getArraySize;

				#endregion

				#region eval

                private readonly SciterScriptApiDelegates.Eval _eval;
                private readonly SciterScriptApiDelegates.EvalString _evalString;

				#endregion

				#region call function (method)

                private readonly SciterScriptApiDelegates.CallFunction _callFunction;

				#endregion

				#region compiled bytecodes

                private readonly SciterScriptApiDelegates.CompileByteCode _compileByteCode;
                private readonly SciterScriptApiDelegates.LoadByteCode _loadByteCode;

				#endregion

                #region throw error

				private readonly SciterScriptApiDelegates.ThrowError _throwError;

				#endregion


				#region  arguments access

                private readonly SciterScriptApiDelegates.GetArgCount _getArgCount;
                private readonly SciterScriptApiDelegates.GetArgN _getArgN;

				#endregion


				// path here is global "path" of the object, something like
				// "one"
				// "one.two", etc.
                private readonly SciterScriptApiDelegates.GetValueByPath _getValueByPath;


                #region pins

                private readonly SciterScriptApiDelegates.Pin _pin;

                private readonly SciterScriptApiDelegates.Unpin _unpin;

                #endregion
                
                #region native_function_value / native_property_value

				private readonly SciterScriptApiDelegates.NativeFunctionValue _nativeFunctionValue;
				private readonly SciterScriptApiDelegates.NativePropertyValue _nativePropertyValue;
                
                #endregion

				private readonly SciterScriptApiDelegates.Post _post;

                private readonly SciterScriptApiDelegates.SetRemoteStdStreams _setRemoteStdStreams;

                private readonly SciterScriptApiDelegates.MakeValList _makeValList;

                private readonly SciterScriptApiDelegates.GetLength _getLength;

                private readonly SciterScriptApiDelegates.GetNext _getNext;

                private readonly SciterScriptApiDelegates.GetNextKeyValue _getNextKeyValue;

                private readonly SciterScriptApiDelegates.SetExtraData _setExtraData;
				private readonly SciterScriptApiDelegates.GetExtraData _getExtraData;


#pragma warning restore 649

				internal NativeSciterScriptApiWrapper(IntPtr apiPtr)
				{
					_apiPtr = apiPtr;
					var @struct = Marshal.PtrToStructure<TStruct>(apiPtr);

					var fieldInfoDictionary = GetType()
						.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
						.Where(w => w.FieldType.GetCustomAttribute<SciterStructMapAttribute>() != null)
						.ToDictionary(key => key.FieldType.GetCustomAttribute<SciterStructMapAttribute>()?.Name,
							value => value);

					var fieldInfos = @struct.GetType().GetFields();
					foreach (var fieldInfo in fieldInfos)
					{
						if (!fieldInfoDictionary.ContainsKey(fieldInfo.Name))
							continue;
						fieldInfoDictionary[fieldInfo.Name].SetValue(this, fieldInfo.GetValue(@struct));
					}
				}

				public IntPtr CreateVM(uint features = 4294967295, uint heapSize = 1048576, uint stackSize = 65536) =>
					_createVM(features: features, heapSize: heapSize, stackSize: stackSize);
				
				public void DestroyVM(IntPtr vmPtr) =>
					_destroyVM(vmPtr);

				public void InvokeGC(IntPtr vmPtr) =>
					_invokeGc(vmPtr);

                public void SetStdStreams(IntPtr vmPtr, IntPtr input, IntPtr output, IntPtr error) =>
                    _setStdStreams(vmPtr, input, output, error);

				public IntPtr GetCurrentVM() => 
					_getCurrentVM();

				public ScriptValue GetGlobalNS(IntPtr vmPtr) =>
					_getGlobalNS(vmPtr);

				public ScriptValue GetCurrentNS(IntPtr vmPtr) =>
					_getCurrentNS(vmPtr);

				public bool IsInt(ScriptValue v) =>
					_isInt(v);

				public bool IsFloat(ScriptValue v) =>
					_isFloat(v);

				public bool IsSymbol(ScriptValue v) =>
					_isSymbol(v);
				
				public bool IsString(ScriptValue v) =>
					_isString(v);

				public bool IsArray(ScriptValue v) =>
					_isArray(v);

				public bool IsObject(ScriptValue v) =>
					_isObject(v);

				public bool IsNativeObject(ScriptValue v) =>
					_isNativeObject(v);

				public bool IsFunction(ScriptValue v) =>
					_isFunction(v);

				public bool IsNativeFunction(ScriptValue v) =>
					_isNativeFunction(v);

				public bool IsInstanceOf(ScriptValue v, ScriptValue cls) =>
					_isInstanceOf(v, cls);

				public bool IsUndefined(ScriptValue v) =>
					_isUndefined(v);

				public bool IsNothing(ScriptValue v) =>
					_isNothing(v);

				public bool IsNull(ScriptValue v) =>
					_isNull(v);

				public bool IsTrue(ScriptValue v) =>
					_isTrue(v);

				public bool IsFalse(ScriptValue v) =>
					_isFalse(v);

				public bool IsClass(IntPtr vmPtr, ScriptValue v) =>
					_isClass(vmPtr, v);

				public bool IsError(ScriptValue v) =>
					_isError(v);

				public bool IsBytes(ScriptValue v) =>
					_isBytes(v);

				public bool IsDateTime(IntPtr vmPtr, ScriptValue v) =>
					_isDateTime(vmPtr, v);

				public bool GetIntValue(ScriptValue v, out int pi) =>
					_getIntValue(v, out pi);

				public bool GetFloatValue(ScriptValue v, out double pf) =>
					_getFloatValue(v, out pf);

				public bool GetBoolValue(ScriptValue v, out bool pb) =>
					_getBoolValue(v, out pb);

				public bool GetSymbolValue(ScriptValue v, out string psz)
				{
					var result = _getSymbolValue(v, out var p);
					psz = result ? Marshal.PtrToStringAuto(p) : null;
					return result;
				}
				
				public bool GetStringValue(ScriptValue v, out string pdata, out int pLength)
				{
					var result = _getStringValue(v, out var pdataPtr, out var uLength);
					pdata = result ? Marshal.PtrToStringAuto(pdataPtr) : null;
					pLength = result ? System.Convert.ToInt32(uLength) : 0;
					return result;
				}
				
				public bool GetBytes(ScriptValue v, out byte[] pb, out int pbLen)
				{
					var result = _getBytes(v, out var pbPtr, out var uLength);
					pb = new byte[(int)uLength];
					if (result && pb.Length > 0)
						Marshal.Copy(pbPtr, pb, 0, pb.Length);
					pbLen = System.Convert.ToInt32(uLength);
					return result;
				}

				public bool GetDateTime(IntPtr vmPtr, ScriptValue v, out DateTime dateTime)
				{
					var result = _getDateTime(vmPtr, v, out var uDateTime);
					dateTime = result
						? DateTime.FromFileTime(System.Convert.ToInt64(uDateTime))
						: DateTime.MinValue;
					
					return result;
				}
                
				public ScriptValue NothingValue() =>
					_nothingValue();

				public ScriptValue UndefinedValue() =>
					_undefinedValue();

				public ScriptValue NullValue() =>
					_nullValue();

				public ScriptValue BoolValue(bool v) =>
					_boolValue(v);

				public ScriptValue IntValue(int v) =>
					_intValue(v);

				public ScriptValue FloatValue(double v) =>
					_floatValue(v);

				public ScriptValue StringValue(IntPtr vmPtr, string text, int textLength = 0) =>
					_stringValue(vmPtr, text, System.Convert.ToUInt32(textLength));

				public ScriptValue SymbolValue(string zstr) =>
					_symbolValue(zstr);

				public ScriptValue BytesValue(IntPtr vmPtr, byte[] data, int? dataLength = null)
				{
					var uPtr = Marshal.AllocHGlobal(dataLength ?? data.Length);
					try
					{
						Marshal.Copy(data, 0, uPtr, (dataLength ?? data.Length));
						return _bytesValue(vmPtr, uPtr, System.Convert.ToUInt32(dataLength ?? data.Length));
					}
					finally
					{
						Marshal.FreeHGlobal(uPtr);
					}
				}

				public ScriptValue DateTimeValue(IntPtr vmPtr, DateTime dateTime) =>
					_dateTimeValue(vmPtr,System.Convert.ToUInt64(dateTime.ToFileTime()));

				public ScriptValue ToString(IntPtr vmPtr, ScriptValue v) =>
					_toString(vmPtr, v);

				public ScriptValue DefineClass(IntPtr vmPtr, tiscript_class_def cls, ScriptValue zns)
				{ 
                    var valuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(zns));

                    try
                    {
                        Marshal.StructureToPtr(zns, valuePtr, false);

                        return _defineClass(vmPtr, ref cls, valuePtr);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(valuePtr);
                    }
				}

                public ScriptValue DefineClass(IntPtr vmPtr, tiscript_class_def cls, IntPtr zns)
                {
                    return _defineClass(vmPtr, ref cls, zns);
                }


                #region object

				public ScriptValue CreateObject(IntPtr vmPtr, ScriptValue ofClass) =>
					_createObject(vmPtr, ofClass);

				public bool SetProp(IntPtr vmPtr, ScriptValue obj, ScriptValue key,
					ScriptValue value) =>
					_setProp(vmPtr, obj, key, value);

				public ScriptValue GetProp(IntPtr vmPtr, ScriptValue obj, ScriptValue key) =>
					_getProp(vmPtr, obj, key);

				public bool ForEachProp(IntPtr vmPtr, ScriptValue obj, Callbacks.tiscript_object_enum cb,
					IntPtr tag) =>
					_forEachProp(vmPtr, obj, cb, tag);

				public IntPtr GetInstanceData(ScriptValue obj) =>
					_getInstanceData(obj);

				public void SetInstanceData(ScriptValue obj, IntPtr data) =>
					_setInstanceData(obj, data);

				#endregion

				#region array

				public ScriptValue CreateArray(IntPtr vmPtr, uint ofSize) =>
                    _createArray(vmPtr, ofSize);

                public bool SetElem(IntPtr vmPtr, ScriptValue obj, uint idx, ScriptValue value) =>
                    _setElem(vmPtr, obj, idx, value);

                public ScriptValue GetElem(IntPtr vmPtr, ScriptValue obj, uint idx) =>
                    _getElem(vmPtr, obj, idx);

                public ScriptValue SetArraySize(IntPtr vmPtr, ScriptValue obj, uint ofSize) =>
                    _setArraySize(vmPtr, obj, ofSize);

                public int GetArraySize(IntPtr vmPtr, SciterScript.ScriptValue obj) =>
                    (int)_getArraySize(vmPtr, obj);

				#endregion

				#region eval

				public bool Eval(IntPtr vmPtr, ScriptValue ns, IntPtr input, bool templateMode,
                    out ScriptValue retValue) => 
				    _eval(vmPtr, ns, input, templateMode, out retValue);

                public bool EvalString(IntPtr vmPtr, IntPtr ns, string script,
                    out ScriptValue retValue, int? scriptLength = null) =>
                    _evalString(vmPtr, ns, script, System.Convert.ToUInt32(scriptLength ?? script.Length), out retValue);

                public bool EvalString(IntPtr vmPtr, ScriptValue ns, string script,
                    out ScriptValue retValue, int? scriptLength)
                {
					var nsPointer = Marshal.AllocHGlobal(Marshal.SizeOf(ns));

                    try
                    {
                        Marshal.StructureToPtr(ns, nsPointer, false);
                        return EvalString(vmPtr, nsPointer, script, out retValue, scriptLength);
					}
                    finally
                    {
						Marshal.FreeHGlobal(nsPointer);
                    }
                }

				#endregion

				#region call function (method)

				public bool CallFunction(IntPtr vmPtr, ScriptValue obj, string funcname, out ScriptValue retValue, params ScriptValue[] argv)
                {

                    retValue = UndefinedValue();

					//inline value     call(HVM vm, value obj, const char* funcname, const value* argv = 0, unsigned argn = 0)
					//{
					//    value function = get_prop(vm, obj, funcname);
					//    if (is_function(function) || is_native_function(function))
					//        return call(vm, obj, function, argv, argn);
					//    else
					//        return v_undefined();
					//}

					var function = GetProp(vmPtr, obj, StringValue(vmPtr, funcname) );

                    if (!(function.IsFunction || function.IsNativeFunction))
                        return false;

                    return CallFunction(vmPtr, obj, function, out retValue, argv);
                }

                public bool CallFunction(IntPtr vmPtr, ScriptValue obj, ScriptValue function, out ScriptValue retValue, params ScriptValue[] argv)
                {

                    var argvPtr = IntPtr.Zero;

                    if (argv?.Length > 0)
                    {
                        var pointers = new IntPtr[argv.Length];
                        argvPtr = Marshal.AllocHGlobal(IntPtr.Size * pointers.Length);
                        for (var i = 0; i < argv.Length; i++)
                        {
                            pointers[i] = Marshal.AllocHGlobal(IntPtr.Size);
                            Marshal.StructureToPtr(argv[i], pointers[i], false);
                            Marshal.WriteIntPtr(argvPtr, i * IntPtr.Size, pointers[i]);

                            //pointers[i] = IntPtr.Zero;
                            //Marshal.WriteIntPtr(argvPtr, i * IntPtr.Size, pointers[i]);
                        }
                    }

                    try
                    {
                        return _callFunction(vmPtr, obj, function, argvPtr, (uint)(argv?.Length ?? 0), out retValue);
                    }
					finally
                    {
                        Marshal.FreeHGlobal(argvPtr);
                    }

                }
                
                #endregion

				#region compiled bytecodes

                public bool CompileByteCode(IntPtr vmPtr, SciterScript.tiscript_stream input,
                    out SciterScript.tiscript_stream output, bool templateMode) =>
                    _compileByteCode(vmPtr, input, out output, templateMode);

                public bool LoadByteCode(IntPtr vmPtr, SciterScript.tiscript_stream input) =>
                    _loadByteCode(vmPtr, input);

				#endregion

                #region throw error

				public void ThrowError(IntPtr vmPtr, string error) =>
					_throwError(vmPtr, error);

				#endregion

                #region arguments access

                public int GetArgCount(IntPtr vmPtr) => 
                    (int) _getArgCount(vmPtr);

                public ScriptValue GetArgN(IntPtr vmPtr, int n) =>
                    _getArgN(vmPtr, (uint) n);
                
                #endregion

                public bool GetValueByPath(IntPtr vmPtr, out ScriptValue ns, string path) =>
                    _getValueByPath(vmPtr, out ns, path);

                public void Pin(IntPtr vmPtr, PinnedScriptValue pp) =>
					_pin(vmPtr, pp);

				public void Unpin(PinnedScriptValue pp) =>
					_unpin(pp);

                public ScriptValue NativeFunctionValue(IntPtr vmPtr, tiscript_method_def p_method_def) =>
                    _nativeFunctionValue(vmPtr, ref p_method_def);

                public ScriptValue NativePropertyValue(IntPtr vmPtr, tiscript_prop_def p_prop_def) =>
                    _nativePropertyValue(vmPtr, p_prop_def);

                public bool Post(IntPtr vmPtr, IntPtr pfunc, IntPtr prm) =>
                    _post(vmPtr, pfunc, prm);

                public bool SetRemoteStdStreams(IntPtr vmPtr, IntPtr input, IntPtr output, IntPtr error) =>
                    _setRemoteStdStreams(vmPtr, input, output, error);

                public ScriptValue MakeValList(IntPtr vmPtr, int valc, ScriptValue va) =>
                    _makeValList(vmPtr, valc, va);

				public int GetLength(ScriptValue obj) =>
					_getLength(this.GetCurrentVM(), obj);

				public int GetLength(IntPtr vmPtr, ScriptValue obj) =>
					_getLength(vmPtr, obj);

                public bool GetNext(IntPtr vmPtr, ScriptValue obj, ScriptValue pos, ScriptValue val) =>
                    _getNext(vmPtr, obj, pos, val);


                public bool GetNextKeyValue(IntPtr vmPtr, ScriptValue obj, ScriptValue pos, ScriptValue key,
                    ScriptValue val) =>
                    _getNextKeyValue(vmPtr, obj, pos, key, val);


                public bool SetExtraData(IntPtr vmPtr, IntPtr data) =>
                    _setExtraData(vmPtr, data);


                public IntPtr GetExtraData(IntPtr vmPtr) =>
                    _getExtraData(vmPtr);

                /// <summary>
                /// 
                /// </summary>
                /// <param name="vmPtr"></param>
                /// <param name="pd"></param>
                /// <param name="zns">namespace object (or 0 if global)</param>
                public void DefineGlobalProperty(IntPtr vmPtr, tiscript_prop_def pd, IntPtr zns = default)
                {
	                
                }
                
                public bool DefineGlobalFunction(IntPtr vmPtr, tiscript_method_def md) =>
	                DefineGlobalFunction(vmPtr, md, GetGlobalNS(vmPtr));
                
                public bool DefineGlobalFunction(IntPtr vmPtr, tiscript_method_def md, ScriptValue zns)
                {
	                if( zns.IsNothing || zns.IsNothing || zns.IsUndefined ) 
		                zns = GetGlobalNS(vmPtr);

	                var key = StringValue(vmPtr, md.name);
	                var value = NativeFunctionValue(vmPtr, md);
	                
	                return SetProp(vmPtr,zns, key, value);
                }
                
                public bool DefineGlobalProperty(IntPtr vmPtr, tiscript_prop_def pd) =>
	                DefineGlobalProperty(vmPtr, pd, GetGlobalNS(vmPtr));
                
                public bool DefineGlobalProperty(IntPtr vmPtr, tiscript_prop_def pd, ScriptValue zns)
                {
	                if( zns.IsNothing || zns.IsNothing || zns.IsUndefined ) 
		                zns = GetGlobalNS(vmPtr);

	                var key = StringValue(vmPtr, pd.name);
	                var value = NativePropertyValue(vmPtr, pd);
	                
	                return SetProp(vmPtr,zns, key, value);
                }
			}
		}
    }
}