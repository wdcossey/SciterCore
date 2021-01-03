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
        public static class Callbacks
        {
            /// <summary>
            /// typedef bool  TISAPI tiscript_object_enum(tiscript_VM *c,tiscript_value key, tiscript_value tiscript_value, void* tag); // true - continue enumeartion
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <param name="tag"></param>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate bool tiscript_object_enum(IntPtr vmPtr, ScriptValue key, ScriptValue value, IntPtr tag);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tag"></param>
            /// <param name="pv"></param>
            /// <returns></returns>
            [Obsolete("Removed in Sciter v4.4.3.24", false)] 
            public delegate bool tiscript_stream_input(tiscript_stream tag, int pv);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tag"></param>
            /// <param name="v"></param>
            /// <returns></returns>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate bool tiscript_stream_output(out tiscript_stream tag, int v);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate string tiscript_stream_name(tiscript_stream tag);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tag"></param>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate void tiscript_stream_close(tiscript_stream tag);

            #region getter/setter implementation

            /// <summary>
            /// 
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate ScriptValue tiscript_get_prop(IntPtr vmPtr, ScriptValue obj);
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate void tiscript_set_prop(IntPtr vmPtr, ScriptValue obj, ScriptValue tiscript_value);
            
            #endregion

            #region native method implementation

            /// <summary>
            /// typedef tiscript_value TISAPI tiscript_method(tiscript_VM *c);
            /// </summary>
            /// <param name="vmPtr"></param>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate ScriptValue tiscript_method(IntPtr vmPtr);
            
            /// <summary>
            /// typedef tiscript_value TISAPI tiscript_tagged_method(tiscript_VM *c, tiscript_value self, void* tag);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="self"></param>
            /// <param name="tag"></param>
            [Obsolete("Removed in Sciter v4.4.3.24", false)]
            public delegate ScriptValue tiscript_tagged_method(IntPtr vmPtr, ScriptValue self, IntPtr tag);
            
            #endregion

        }

        [Obsolete]
        internal static class SciterScriptApiDelegates
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void Void();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.CreateVM))]
            public delegate IntPtr CreateVM(uint features = 0xffffffff, uint heapSize = 1024 * 1024,
                uint stackSize = 64 * 1024);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.DestroyVM))]
            public delegate void DestroyVM(IntPtr vmPtr);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.InvokeGC))]
            public delegate void InvokeGC(IntPtr pvm);

            /// <summary>
            /// void  (TISAPI *set_std_streams)(tiscript_VM* pvm, tiscript_stream* input, tiscript_stream* output, tiscript_stream* error);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="input"></param>
            /// <param name="output"></param>
            /// <param name="error"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.SetStdStreams))]
            public delegate void SetStdStreams(IntPtr vmPtr, IntPtr input, IntPtr output, IntPtr error);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetCurrentVm))]
            public delegate IntPtr GetCurrentVM();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetGlobalNS))]
            public delegate ScriptValue GetGlobalNS(IntPtr vmPtr);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetCurrentNS))]
            public delegate ScriptValue GetCurrentNS(IntPtr vmPtr);

            #region Is
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsInt))]
            public delegate bool IsInt(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsFloat))]
            public delegate bool IsFloat(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsSymbol))]
            public delegate bool IsSymbol(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsString))]
            public delegate bool IsString(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsArray))]
            public delegate bool IsArray(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsObject))]
            public delegate bool IsObject(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsNativeObject))]
            public delegate bool IsNativeObject(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsFunction))]
            public delegate bool IsFunction(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsNativeFunction))]
            public delegate bool IsNativeFunction(ScriptValue v);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsInstanceOf))]
            public delegate bool IsInstanceOf(ScriptValue v, ScriptValue cls);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsUndefined))]
            public delegate bool IsUndefined(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsNothing))]
            public delegate bool IsNothing(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsNull))]
            public delegate bool IsNull(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsTrue))]
            public delegate bool IsTrue(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsFalse))]
            public delegate bool IsFalse(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsClass))]
            public delegate bool IsClass(IntPtr vmPtr, ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsError))]
            public delegate bool IsError(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsBytes))]
            public delegate bool IsBytes(ScriptValue v);
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.IsDateTime))]
            public delegate bool IsDateTime(IntPtr vmPtr, ScriptValue v);

            #endregion

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetValueByPath))]
            public delegate bool GetValueByPath(IntPtr vmPtr, out ScriptValue ns,
                [MarshalAs(UnmanagedType.LPStr)] string path);


            #region Get

            [SciterStructMap(nameof(SciterScriptApi.GetIntValue))]
            public delegate bool GetIntValue(ScriptValue obj, out int pi);
            
            [SciterStructMap(nameof(SciterScriptApi.GetFloatValue))]
            public delegate bool GetFloatValue(ScriptValue obj, out double pf);
            
            [SciterStructMap(nameof(SciterScriptApi.GetBoolValue))]
            public delegate bool GetBoolValue(ScriptValue obj, out bool pf);
            
            [SciterStructMap(nameof(SciterScriptApi.GetSymbolValue))]
            public delegate bool GetSymbolValue(ScriptValue obj, out IntPtr psz);
            
            //  bool (TISAPI *get_string_value)(tiscript_value v, const WCHAR** pdata, unsigned* plength);
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            [SciterStructMap(nameof(SciterScriptApi.GetStringValue))]
            //public delegate bool GetStringValue(ScriptValue v, out string pdata, out uint pLength);
            public delegate bool GetStringValue(ScriptValue v, out IntPtr pdata, out uint pLength);


            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetBytes))]
            public delegate bool GetBytes(ScriptValue v, out IntPtr pb, out uint pbLength);
            
            [SciterStructMap(nameof(SciterScriptApi.GetDatetime))]
            public delegate bool GetDateTime(IntPtr vmPtr, ScriptValue v, out ulong dt);

            #endregion
            
            #region Value

            [SciterStructMap(nameof(SciterScriptApi.NothingValue))]
            public delegate ScriptValue NothingValue();
            
            [SciterStructMap(nameof(SciterScriptApi.UndefinedValue))]
            public delegate ScriptValue UndefinedValue();
            
            [SciterStructMap(nameof(SciterScriptApi.NullValue))]
            public delegate ScriptValue NullValue();
            
            [SciterStructMap(nameof(SciterScriptApi.BoolValue))]
            public delegate ScriptValue BoolValue(bool v);
            
            [SciterStructMap(nameof(SciterScriptApi.IntValue))]
            public delegate ScriptValue IntValue(int v);
            
            [SciterStructMap(nameof(SciterScriptApi.FloatValue))]
            public delegate ScriptValue FloatValue(double v);
            

            /// <summary>
            /// tiscript_value (TISAPI *string_value)(tiscript_VM*, const WCHAR* text, unsigned int text_length);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="text"></param>
            /// <param name="textLength"></param>
            [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
            [SciterStructMap(nameof(SciterScriptApi.StringValue))]
            public delegate ScriptValue StringValue(IntPtr vmPtr, string text, uint textLength);
            
            /// <summary>
            /// tiscript_value (TISAPI *symbol_value)(const char* zstr);
            /// </summary>
            /// <param name="zstr"></param>
            [SciterStructMap(nameof(SciterScriptApi.SymbolValue))]
            public delegate ScriptValue SymbolValue(string zstr);
            
            /// <summary>
            /// tiscript_value (TISAPI *bytes_value)(tiscript_VM*, const byte* data, unsigned int data_length);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="data"></param>
            /// <param name="dataLength"></param>
            [SciterStructMap(nameof(SciterScriptApi.BytesValue))]
            public delegate ScriptValue BytesValue(IntPtr vmPtr, IntPtr data, uint dataLength);
            
            /// <summary>
            /// tiscript_value (TISAPI *datetime_value)(tiscript_VM*, unsigned long long dt);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="dt"></param>
            [SciterStructMap(nameof(SciterScriptApi.DateTimeValue))]
            public delegate ScriptValue DateTimeValue(IntPtr vmPtr, ulong dt);

            #endregion

            [SciterStructMap(nameof(SciterScriptApi.To_String))]
            public delegate ScriptValue To_String(IntPtr vmPtr, ScriptValue v);

            #region define native class

            /// <summary>
            /// tiscript_value (TISAPI *define_class) (tiscript_VM* vm, tiscript_class_def* cls, tiscript_value zns );
            /// </summary>
            /// <param name="vmPtr">in this tiscript_VM</param>
            /// <param name="cls"></param>
            /// <param name="zns">in this namespace object (or 0 if global)</param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = false)]
            [SciterStructMap(nameof(SciterScriptApi.DefineClass))]
            public delegate ScriptValue DefineClass(IntPtr vmPtr, ref tiscript_class_def cls, IntPtr zns);
            //public delegate ScriptValue DefineClass(IntPtr vmPtr, ref tiscript_class_def cls, ScriptValue zns);
            //public delegate ScriptValue DefineClass(IntPtr vmPtr, IntPtr cls, IntPtr zns);

            #endregion
            
            #region object
            
            /// <summary>
            /// tiscript_value (TISAPI *create_object)(tiscript_VM*, tiscript_value of_class); // of_class == 0 - "Object"
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="ofClass"></param>
            [SciterStructMap(nameof(SciterScriptApi.CreateObject))]
            public delegate ScriptValue CreateObject(IntPtr vmPtr, ScriptValue ofClass);

            /// <summary>
            /// bool (TISAPI *set_prop)(tiscript_VM*,tiscript_value obj, tiscript_value key, tiscript_value tiscript_value);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            [SciterStructMap(nameof(SciterScriptApi.SetProp))]
            public delegate bool SetProp(IntPtr vmPtr, ScriptValue obj, ScriptValue key, ScriptValue value);
            
            /// <summary>
            /// tiscript_value (TISAPI *get_prop)(tiscript_VM*,tiscript_value obj, tiscript_value key);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="key"></param>
            [SciterStructMap(nameof(SciterScriptApi.GetProp))]
            public delegate ScriptValue GetProp(IntPtr vmPtr, ScriptValue obj, ScriptValue key);

            /// <summary>
            /// bool (TISAPI *for_each_prop)(tiscript_VM*, tiscript_value obj, tiscript_object_enum* cb, void* tag);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="cb"></param>
            /// <param name="tag"></param>
            [SciterStructMap(nameof(SciterScriptApi.ForEachProp))]
            public delegate bool ForEachProp(IntPtr vmPtr, ScriptValue obj, Callbacks.tiscript_object_enum cb, IntPtr tag);
            
            /// <summary>
            /// void* (TISAPI *get_instance_data)(tiscript_value obj);
            /// </summary>
            /// <param name="obj"></param>
            [SciterStructMap(nameof(SciterScriptApi.GetInstanceData))]
            public delegate IntPtr GetInstanceData(ScriptValue obj);
            
            /// <summary>
            /// void (TISAPI *set_instance_data)(tiscript_value obj, void* data);
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="data"></param>
            [SciterStructMap(nameof(SciterScriptApi.SetInstanceData))]
            public delegate void SetInstanceData(ScriptValue obj, IntPtr data);

            #endregion

            #region array

            /// <summary>
            /// tiscript_value    (TISAPI *create_array)(tiscript_VM*, unsigned of_size);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="ofSize"></param>
            /// <returns></returns>
            [SciterStructMap(nameof(SciterScriptApi.CreateArray))]
            public delegate ScriptValue CreateArray(IntPtr vmPtr, uint ofSize);

            /// <summary>
            /// bool (TISAPI *set_elem)(tiscript_VM*, tiscript_value obj, unsigned idx, tiscript_value tiscript_value);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="idx"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [SciterStructMap(nameof(SciterScriptApi.SetElem))]
            public delegate bool SetElem(IntPtr vmPtr, ScriptValue obj, uint idx, ScriptValue value);

            /// <summary>
            /// tiscript_value (TISAPI *get_elem)(tiscript_VM*, tiscript_value obj, unsigned idx);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="idx"></param>
            /// <returns></returns>
            [SciterStructMap(nameof(SciterScriptApi.GetElem))]
            public delegate ScriptValue GetElem(IntPtr vmPtr, ScriptValue obj, uint idx);

            /// <summary>
            /// tiscript_value (TISAPI *set_array_size)(tiscript_VM*, tiscript_value obj, unsigned of_size);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="ofSize"></param>
            /// <returns></returns>
            [SciterStructMap(nameof(SciterScriptApi.SetArraySize))]
            public delegate ScriptValue SetArraySize(IntPtr vmPtr, ScriptValue obj, uint ofSize);

            /// <summary>
            /// unsigned (TISAPI *get_array_size)(tiscript_VM*, tiscript_value obj);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <returns></returns>
            [SciterStructMap(nameof(SciterScriptApi.GetArraySize))]
            public delegate uint GetArraySize(IntPtr vmPtr, ScriptValue obj);

            #endregion

            #region eval

            /// <summary>
            /// bool (TISAPI *eval)(tiscript_VM*, tiscript_value ns, tiscript_stream* input, bool template_mode, tiscript_value* pretval);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="ns"></param>
            /// <param name="input"></param>
            /// <param name="input"></param>
            /// <param name="templateMode"></param>
            /// <param name="retValue"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.Eval))]
            public delegate bool Eval(IntPtr vmPtr, ScriptValue ns, IntPtr input, bool templateMode, out ScriptValue retValue);

            /// <summary>
            /// bool (TISAPI *eval_string)(tiscript_VM*, tiscript_value ns, const WCHAR* script, unsigned script_length, tiscript_value* pretval);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="ns"></param>
            /// <param name="script"></param>
            /// <param name="scriptLength"></param>
            /// <param name="retValue"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.EvalString))]
            public delegate bool EvalString(IntPtr vmPtr, IntPtr ns, [MarshalAs(UnmanagedType.LPWStr)] string script, uint scriptLength, out ScriptValue retValue);


            #endregion

            #region call function (method)

            /// <summary>
            /// bool     (TISAPI *call)(tiscript_VM*, tiscript_value obj, tiscript_value function, const tiscript_value* argv, unsigned argn, tiscript_value* pretval);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="function"></param>
            /// <param name="argv"></param>
            /// <param name="argn"></param>
            /// <param name="retValue"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.CallFunction))]
            public delegate bool CallFunction(IntPtr vmPtr, ScriptValue obj, ScriptValue function, IntPtr argv, uint argn, out ScriptValue retValue);

            #endregion

            #region compiled bytecodes

            /// <summary>
            /// bool (TISAPI *compile)( tiscript_VM* pvm, tiscript_stream* input, tiscript_stream* output_bytecodes, bool template_mode );
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="input"></param>
            /// <param name="output"></param>
            /// <param name="templateMode"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.CompileByteCode))]
            public delegate bool CompileByteCode(IntPtr vmPtr, tiscript_stream input, out tiscript_stream output, bool templateMode);

            /// <summary>
            /// bool (TISAPI *loadbc)( tiscript_VM* pvm, tiscript_stream* input_bytecodes );
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="input"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.LoadByteCode))]
            public delegate bool LoadByteCode(IntPtr vmPtr, tiscript_stream input);

            #endregion

            #region throw error

            /// <summary>
            ///  void     (TISAPI *throw_error)( tiscript_VM*, const WCHAR* error);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="error"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
            [SciterStructMap(nameof(SciterScriptApi.ThrowError))]
            public delegate void ThrowError(IntPtr vmPtr, [MarshalAs(UnmanagedType.LPWStr)] string error);

            #endregion

            #region arguments access

            /// <summary>
            /// unsigned (TISAPI *get_arg_count)( tiscript_VM* pvm );
            /// </summary>
            /// <param name="vmPtr"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetArgCount))]
            public delegate uint GetArgCount(IntPtr vmPtr);

            /// <summary>
            /// tiscript_value (TISAPI *get_arg_n)( tiscript_VM* pvm, unsigned n );
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="n"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetArgN))]
            public delegate ScriptValue GetArgN(IntPtr vmPtr, uint n);


            #endregion

            #region pins

            /// <summary>
            /// void (TISAPI *pin)(tiscript_VM*, tiscript_pvalue* pp);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="pp"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.Pin))]
            public delegate void Pin(IntPtr vmPtr, PinnedScriptValue pp);

            /// <summary>
            /// void (TISAPI *unpin)(tiscript_pvalue* pp);
            /// </summary>
            /// <param name="pp"></param>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.Unpin))]
            public delegate void Unpin(PinnedScriptValue pp);

            #endregion
            
            #region native_function_value / native_property_value,

            //create native_function_value and native_property_value,
            //use this if you want to add native functions/properties in runtime to exisiting classes or namespaces(including global ns)

            /// <summary>
            /// tiscript_value (TISAPI *native_function_value)(tiscript_VM* pvm, tiscript_method_def* p_method_def);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="p_method_def"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.NativeFunctionValue))]
            public delegate ScriptValue NativeFunctionValue(IntPtr vmPtr, ref tiscript_method_def p_method_def);

            /// <summary>
            /// tiscript_value (TISAPI *native_property_value)(tiscript_VM* pvm, tiscript_prop_def* p_prop_def);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="p_prop_def"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.NativePropertyValue))]
            public delegate ScriptValue NativePropertyValue(IntPtr vmPtr, tiscript_prop_def p_prop_def);

            #endregion

            /// <summary>
            /// bool (TISAPI *post)(tiscript_VM* pvm, tiscript_callback* pfunc, void* prm);
            /// <para>Schedule execution of the pfunc(prm) in the thread owning this VM. <br/>
            /// Used when you need to call scripting methods from threads other than main (GUI) thread <br/>
            /// It is safe to call tiscript functions inside the pfunc.</para>
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="pfunc"></param>
            /// <param name="prm"></param>
            /// <returns>'true' if scheduling of the call was accepted, 'false' when failure (VM has no dispatcher attached).</returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.Post))]
            public delegate bool Post(IntPtr vmPtr, IntPtr pfunc, IntPtr prm);

            /// <summary>
            /// bool  (TISAPI *set_remote_std_streams)(tiscript_VM* pvm, tiscript_pvalue* input, tiscript_pvalue* output, tiscript_pvalue* error);
            /// <para>Introduce alien VM to the host VM: <br/>
            /// Calls method found on "host_method_path" (if there is any) on the pvm_host
            /// notifying the host about other VM (alien) creation. Return value of script function "host_method_path" running in pvm_host is passed
            /// as a parameter of a call to function at "alien_method_path".<br/>
            /// One of possible uses of this function: <br/>
            /// Function at "host_method_path" creates async streams that will serve a role of stdin, stdout and stderr for the alien vm.<br/>
            /// This way two VMs can communicate with each other. <br/>
            /// unsigned (TISAPI *introduce_vm)(tiscript_VM* pvm_host, const char* host_method_path,  tiscript_VM* pvm_alien, const char* alien_method_path);
            /// </para>
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="input"></param>
            /// <param name="output"></param>
            /// <param name="error"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.SetRemoteStdStreams))]
            public delegate bool SetRemoteStdStreams(IntPtr vmPtr, IntPtr input, IntPtr output, IntPtr error);

            /// <summary>
            /// support of multi-return values from native functions, n here is a number 1..64
            /// </summary>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.MakeValList))]
            public delegate ScriptValue MakeValList(IntPtr vmPtr, int valc, ScriptValue va);

            /// <summary>
            /// int   (TISAPI *get_length)(tiscript_VM* pvm, tiscript_value obj );
            /// <para>returns number of props in object, elements in array, or bytes in byte array.</para>
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetLength))]
            public delegate int GetLength(IntPtr vmPtr, ScriptValue obj);

            /// <summary>
            /// bool  (TISAPI *get_next)(tiscript_VM* pvm, tiscript_value* obj, tiscript_value* pos, tiscript_value* val);
            /// <para>for( var val in coll ) {...}</para>
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="pos"></param>
            /// <param name="val"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetNext))]
            public delegate bool GetNext(IntPtr vmPtr, ScriptValue obj, ScriptValue pos, ScriptValue val);


            /// <summary>
            /// bool  (TISAPI *get_next_key_value)(tiscript_VM* pvm, tiscript_value* obj, tiscript_value* pos, tiscript_value* key, tiscript_value* val);
            /// <para>for( var (key,val) in coll ) {...}</para>
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="obj"></param>
            /// <param name="pos"></param>
            /// <param name="key"></param>
            /// <param name="val"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetNextKeyValue))]
            public delegate bool GetNextKeyValue(IntPtr vmPtr, ScriptValue obj, ScriptValue pos, ScriptValue key, ScriptValue val);


            //associate extra data pointer with the VM

            /// <summary>
            /// bool  (TISAPI *set_extra_data)(tiscript_VM* pvm, void* data);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.SetExtraData))]
            public delegate bool SetExtraData(IntPtr vmPtr, IntPtr data);

            /// <summary>
            /// void* (TISAPI *get_extra_data)(tiscript_VM* pvm);
            /// </summary>
            /// <param name="vmPtr"></param>
            /// <returns></returns>
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            [SciterStructMap(nameof(SciterScriptApi.GetExtraData))]
            public delegate IntPtr GetExtraData(IntPtr vmPtr);
        }
    }
}