using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
    public interface ISciterScriptApi
    {
	    public IntPtr CreateVM(uint features = 0xffffffff, uint heapSize = 1024 * 1024,
			uint stackSize = 64 * 1024);
		
		public void DestroyVM(IntPtr vmPtr);
		
		public void InvokeGC(IntPtr vmPtr);
		
		public void SetStdStreams(IntPtr vmPtr, IntPtr input, IntPtr output, IntPtr error);
		
		public IntPtr GetCurrentVM();
		
		public SciterScript.ScriptValue GetGlobalNS(IntPtr vmPtr);
		
		public SciterScript.ScriptValue GetCurrentNS(IntPtr vmPtr);
 
		public bool IsInt(SciterScript.ScriptValue v);
		
		public bool IsFloat(SciterScript.ScriptValue v);
		
		public bool IsSymbol(SciterScript.ScriptValue v);
		
		public bool IsString(SciterScript.ScriptValue v);

		public bool IsArray(SciterScript.ScriptValue v);
		
		public bool IsObject(SciterScript.ScriptValue v);
		
		public bool IsNativeObject(SciterScript.ScriptValue v);
		
		public bool IsFunction(SciterScript.ScriptValue v);
		
		public bool IsNativeFunction(SciterScript.ScriptValue v);
		
		public bool IsInstanceOf(SciterScript.ScriptValue v, SciterScript.ScriptValue cls);
		
		public bool IsUndefined(SciterScript.ScriptValue v);
		
		public bool IsNothing(SciterScript.ScriptValue v);
		
		public bool IsNull(SciterScript.ScriptValue v);
		
		public bool IsTrue(SciterScript.ScriptValue v);
		
		public bool IsFalse(SciterScript.ScriptValue v);
		
		public bool IsClass(IntPtr vmPtr, SciterScript.ScriptValue v);
		
		public bool IsError(SciterScript.ScriptValue v);
		
		public bool IsBytes(SciterScript.ScriptValue v);
		
		public bool IsDateTime(IntPtr vmPtr, SciterScript.ScriptValue v);
 
		#region Get
		
		public bool GetIntValue(SciterScript.ScriptValue v, out int pi);
		
		public bool GetFloatValue(SciterScript.ScriptValue v, out double pf);
		
		public bool GetBoolValue(SciterScript.ScriptValue v, out bool pb);
		
		public bool GetSymbolValue(SciterScript.ScriptValue v, out string psz);
		
		public bool GetStringValue(SciterScript.ScriptValue v, out string pdata, out int pLength);
		
		public bool GetBytes(SciterScript.ScriptValue v, out byte[] pb, out int pbLen);
		
		public bool GetDateTime(IntPtr vmPtr, SciterScript.ScriptValue v, out DateTime pi);
 
		#endregion
		
		public SciterScript.ScriptValue NothingValue();
		public SciterScript.ScriptValue UndefinedValue();
		public SciterScript.ScriptValue NullValue();
		public SciterScript.ScriptValue BoolValue(bool v);
		public SciterScript.ScriptValue IntValue(int v);
		public SciterScript.ScriptValue FloatValue(double v);
		public SciterScript.ScriptValue StringValue(IntPtr vmPtr, string text, int textLength = 0);
		public SciterScript.ScriptValue SymbolValue(string text);
		public SciterScript.ScriptValue BytesValue(IntPtr vmPtr, byte[] data, int? dataLength = null);
		public SciterScript.ScriptValue DateTimeValue(IntPtr vmPtr, DateTime dateTime);
 
		public SciterScript.ScriptValue ToString(IntPtr vmPtr, SciterScript.ScriptValue v);

		// define native class
		public SciterScript.ScriptValue DefineClass(IntPtr vmPtr, SciterScript.tiscript_class_def cls, SciterScript.ScriptValue zns);
		public SciterScript.ScriptValue DefineClass(IntPtr vmPtr, SciterScript.tiscript_class_def cls, IntPtr zns);

		#region object

		public SciterScript.ScriptValue CreateObject(IntPtr vmPtr, SciterScript.ScriptValue ofClass);
		public bool SetProp(IntPtr vmPtr, SciterScript.ScriptValue obj, SciterScript.ScriptValue key,
            SciterScript.ScriptValue value);
		public SciterScript.ScriptValue GetProp(IntPtr vmPtr, SciterScript.ScriptValue obj, SciterScript.ScriptValue key);

		public bool ForEachProp(IntPtr vmPtr, SciterScript.ScriptValue obj, SciterScript.Callbacks.tiscript_object_enum cb, IntPtr tag);

		public IntPtr GetInstanceData(SciterScript.ScriptValue obj);
		public void SetInstanceData(SciterScript.ScriptValue obj, IntPtr data);

        #endregion

		#region array

		public SciterScript.ScriptValue CreateArray(IntPtr vmPtr, uint ofSize);
		public bool SetElem(IntPtr vmPtr, SciterScript.ScriptValue obj, uint idx, SciterScript.ScriptValue value);
		public SciterScript.ScriptValue GetElem(IntPtr vmPtr, SciterScript.ScriptValue obj, uint idx);
		public SciterScript.ScriptValue SetArraySize(IntPtr vmPtr, SciterScript.ScriptValue obj, uint ofSize);
		public int GetArraySize(IntPtr vmPtr, SciterScript.ScriptValue obj);

		#endregion

		#region eval
		
        public bool Eval(IntPtr vmPtr, SciterScript.ScriptValue ns, IntPtr input, bool templateMode, out SciterScript.ScriptValue retValue);

		public bool EvalString(IntPtr vmPtr, SciterScript.ScriptValue ns, string script, 
			out SciterScript.ScriptValue retValue, int? scriptLength = null);

		public bool EvalString(IntPtr vmPtr, IntPtr ns, string script, 
			out SciterScript.ScriptValue retValue, int? scriptLength = null);

		#endregion

		#region call function (method)
		public bool CallFunction(IntPtr vmPtr, SciterScript.ScriptValue obj, string funcname,
            out SciterScript.ScriptValue retValue, params SciterScript.ScriptValue[] argv);

		public bool CallFunction(IntPtr vmPtr, SciterScript.ScriptValue obj, SciterScript.ScriptValue function,
            out SciterScript.ScriptValue retValue, params SciterScript.ScriptValue[] argv);

        #endregion

		#region compiled bytecodes

		public bool CompileByteCode(IntPtr vmPtr, SciterScript.tiscript_stream input, out SciterScript.tiscript_stream output, bool templateMode);

		public bool LoadByteCode(IntPtr vmPtr, SciterScript.tiscript_stream input);

		#endregion

		#region throw error

		public void ThrowError(IntPtr vmPtr, string error);

		#endregion

		#region arguments access

		public int GetArgCount(IntPtr vmPtr);

		public SciterScript.ScriptValue GetArgN(IntPtr vmPtr, int n);
        
        #endregion

		// path here is global "path" of the object, something like
		// "one"
		// "one.two", etc.
		public bool GetValueByPath(IntPtr vmPtr, out SciterScript.ScriptValue ns, string path);


        #region pins

		public void Pin(IntPtr vmPtr, SciterScript.PinnedScriptValue pp);

		public void Unpin(SciterScript.PinnedScriptValue pp);

		#endregion


		#region native_function_value / native_property_value,

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="p_method_def"></param>
		/// <returns></returns>
		public SciterScript.ScriptValue NativeFunctionValue(IntPtr vmPtr, SciterScript.tiscript_method_def p_method_def);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="p_prop_def"></param>
		/// <returns></returns>
		public SciterScript.ScriptValue NativePropertyValue(IntPtr vmPtr, SciterScript.tiscript_prop_def p_prop_def);

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="pfunc"></param>
		/// <param name="prm"></param>
		/// <returns></returns>
		public bool Post(IntPtr vmPtr, IntPtr pfunc, IntPtr prm);

		public bool SetRemoteStdStreams(IntPtr vmPtr, IntPtr input, IntPtr output, IntPtr error);

		/// <summary>
		/// support of multi-return values from native functions, n here is a number 1..64
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="valc"></param>
		/// <param name="va"></param>
		/// <returns></returns>
		public SciterScript.ScriptValue MakeValList(IntPtr vmPtr, int valc, SciterScript.ScriptValue va);

		/// <summary>
		/// returns number of props in object, elements in array, or bytes in byte array.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int GetLength(SciterScript.ScriptValue obj);

		/// <summary>
		/// returns number of props in object, elements in array, or bytes in byte array.
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int GetLength(IntPtr vmPtr, SciterScript.ScriptValue obj);

		/// <summary>
		/// for( var val in coll ) {...}
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="obj"></param>
		/// <param name="pos"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool GetNext(IntPtr vmPtr, SciterScript.ScriptValue obj, SciterScript.ScriptValue pos, SciterScript.ScriptValue val);

		/// <summary>
		/// for( var (key,val) in coll ) {...}
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="obj"></param>
		/// <param name="pos"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public bool GetNextKeyValue(IntPtr vmPtr, SciterScript.ScriptValue obj, SciterScript.ScriptValue pos, SciterScript.ScriptValue key, SciterScript.ScriptValue val);

		/// <summary>
		/// associate extra data pointer with the VM
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool SetExtraData(IntPtr vmPtr, IntPtr data);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <returns></returns>
		public IntPtr GetExtraData(IntPtr vmPtr);


		/// <summary>
		/// defines native function that can be accessed globally
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="pd"></param>
		/// <returns></returns>
		bool DefineGlobalFunction(IntPtr vmPtr, SciterScript.tiscript_method_def md);


		/// <summary>
		/// defines native function that can be accessed globally
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="pd"></param>
		/// <param name="zns">namespace object (or 0 if global)</param>
		/// <returns></returns>
		bool DefineGlobalFunction(IntPtr vmPtr, SciterScript.tiscript_method_def md, SciterScript.ScriptValue zns);
		
		/// <summary>
		/// defines native controlled property (variable if you wish) that can be accessed globally
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="pd"></param>
		/// <returns></returns>
		bool DefineGlobalProperty(IntPtr vmPtr, SciterScript.tiscript_prop_def pd);

		/// <summary>
		/// defines native controlled property (variable if you wish) that can be accessed globally
		/// </summary>
		/// <param name="vmPtr"></param>
		/// <param name="pd"></param>
		/// <param name="zns">namespace object (or 0 if global)</param>
		/// <returns></returns>
		bool DefineGlobalProperty(IntPtr vmPtr, SciterScript.tiscript_prop_def pd, SciterScript.ScriptValue zns);
    }
}