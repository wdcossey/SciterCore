// Copyright 2016 Ramon F. Mendes
//
// This file is part of SciterSharp.
// 
// SciterSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SciterSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SciterSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SciterCore.Interop;
using System.Reflection;

namespace SciterCore
{
	using FUNCTOR = Func<SciterValue[], SciterValue>;

	class SciterValueFunctor : SciterValue
	{
	}

	public class SciterValue
	{
		private Interop.SciterValue.VALUE _data;
		private static Sciter.ISciterAPI _api = Sciter.SciterApi;

		public static readonly SciterValue Undefined;
		public static readonly SciterValue Null;

		static SciterValue()
		{
			Undefined = new SciterValue();
			Null = new SciterValue
			{
				_data = {t = (uint) Interop.SciterValue.VALUE_TYPE.T_NULL}
			};
		}

		public static Interop.SciterValue.VALUE[] ToVALUEArray(SciterValue[] values)
		{
			return values.Select(sv => sv._data).ToArray();
		}

		public Interop.SciterValue.VALUE ToVALUE()
		{
			_api.ValueInit(out var vcopy);
			_api.ValueCopy(out vcopy, ref _data);
			return vcopy;
		}

		public override string ToString()
		{
			var what = "";
			if(IsUndefined) what = nameof(IsUndefined);
			if(IsBool) what = nameof(IsBool);
			if(IsInt) what = nameof(IsInt);
			if(IsFloat) what = nameof(IsFloat);
			if(IsString) what = $"{nameof(IsString)} {Get("")}";
			if(IsSymbol) what = nameof(IsSymbol);
			if(IsErrorString) what = nameof(IsErrorString);
			if(IsDate) what = nameof(IsDate);
			if(IsCurrency) what = nameof(IsCurrency);
			if(IsMap) what = nameof(IsMap);
			if(IsArray) what = nameof(IsArray);
			if(IsFunction) what = nameof(IsFunction);
			if(IsBytes) what = nameof(IsBytes);
			if(IsObject) what = nameof(IsObject);
			if(IsDomElement) what = nameof(IsDomElement);
			if(IsNativeFunction) what = nameof(IsNativeFunction);
			if(IsColor) what = nameof(IsColor);
			if(IsDuration) what = nameof(IsDuration);
			if(IsAngle) what = nameof(IsAngle);
			if(IsNull) what = nameof(IsNull);
			if(IsObjectNative) what = nameof(IsObjectNative);
			if(IsObjectArray) what = nameof(IsObjectArray);
			if(IsObjectFunction) what = nameof(IsObjectFunction);
			if(IsObjectObject) what = nameof(IsObjectObject);
			if(IsObjectClass) what = nameof(IsObjectClass);
			if(IsObjectError) what = nameof(IsObjectError);
#if WINDOWS
			if(IsDate)
				what += " - " + GetDate().ToString();
#endif
			if (IsMap)
			{
				what = $"{nameof(IsMap)} {Regex.Replace(ToJsonString(),@"\t|\n|\r","")}";
			}

			return what;// + " - SciterValue JSON: " + Regex.Replace(ToJSONString(), @"\t|\n|\r", "");
		}

		public SciterValue()
		{
			_api.ValueInit(out _data);
		}

		~SciterValue()
		{
			_api.ValueClear(out _data);
		}
		
		public SciterValue(SciterValue value)
		{
			_api.ValueInit(out _data);
			_api.ValueCopy(out _data, ref value._data);
		}
		
		public SciterValue(Interop.SciterValue.VALUE srcv)
		{
			_api.ValueInit(out _data);
			_api.ValueCopy(out _data, ref srcv);
		}

		public SciterValue(bool v)
		{
			_api.ValueInit(out _data); _api.ValueIntDataSet(ref _data, v ? 1 : 0, (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL, 0);
		}

		public SciterValue(int v)
		{
			_api.ValueInit(out _data); _api.ValueIntDataSet(ref _data, v, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
		}

		public SciterValue(uint v)
		{
			_api.ValueInit(out _data); _api.ValueIntDataSet(ref _data, (int) v, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
		}

		public SciterValue(double v)
		{
			_api.ValueInit(out _data); _api.ValueFloatDataSet(ref _data, v, (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
		}

		public SciterValue(string str)
		{
			_api.ValueInit(out _data); _api.ValueStringDataSet(ref _data, str, (uint) str.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
		}

		public SciterValue(byte[] bs)
		{
			_api.ValueInit(out _data); _api.ValueBinaryDataSet(ref _data, bs, (uint) bs.Length, (uint) Interop.SciterValue.VALUE_TYPE.T_BYTES, 0);
		}

		public SciterValue(DateTime dt)
		{
			_api.ValueInit(out _data); _api.ValueInt64DataSet(ref _data, dt.ToFileTime(), (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
		}
		
		public SciterValue(IEnumerable<SciterValue> col)
		{
			_api.ValueInit(out _data);
			var i = 0;
			foreach(var item in col)
				SetItem(i++, item);
		}
		
		private SciterValue(IConvertible ic)
		{
			_api.ValueInit(out _data);

			switch (ic)
			{
				case bool @bool:
					_api.ValueIntDataSet(ref _data, @bool ? 1 : 0, (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL,
						0);
					break;
				case byte @byte:
					_api.ValueIntDataSet(ref _data, @byte, (uint)Interop.SciterValue.VALUE_TYPE.T_INT, 0);
					break;
				case int @int:
					_api.ValueIntDataSet(ref _data, @int, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
					break;
				case uint @uint:
					_api.ValueIntDataSet(ref _data, (int)@uint, (uint)Interop.SciterValue.VALUE_TYPE.T_INT, 0);
					break;
				case double @double:
					_api.ValueFloatDataSet(ref _data, @double, (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
					break;
				case decimal @decimal:
					_api.ValueFloatDataSet(ref _data, (double)@decimal, (uint)Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
					break;
				case string @string:
					_api.ValueStringDataSet(ref _data, @string, (uint) @string.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
					break;
				case DateTime @dateTime:
					_api.ValueInt64DataSet(ref _data, @dateTime.ToFileTime(), (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
					break;
				default:
					throw new Exception("Can not create a SciterValue from type '" + ic.GetType() + "'");
			}
		}
		
		public SciterValue(Func<SciterValue[], SciterValue> func)
		{
			Interop.SciterValue.FPTR_NATIVE_FUNCTOR_INVOKE fnfi;
			Interop.SciterValue.FPTR_NATIVE_FUNCTOR_RELEASE fnfr;
			var fnfi_gch = new GCHandle();
			var fnfr_gch = new GCHandle();
			var func_gch = GCHandle.Alloc(func);

			fnfi = (IntPtr tag, uint argc, IntPtr argv, out Interop.SciterValue.VALUE retval) =>
			{
				// Get the list of SciterXValue.VALUE from the ptr
				SciterValue[] args = new SciterValue[argc];
				for(var i = 0; i < argc; i++)
					args[i] = new SciterValue((Interop.SciterValue.VALUE)Marshal.PtrToStructure(IntPtr.Add(argv, i * Marshal.SizeOf(typeof(Interop.SciterValue.VALUE))), typeof(Interop.SciterValue.VALUE)));

				retval = func(args).ToVALUE();
				return true;
			};

			fnfr = (IntPtr tag) =>
			{
				// seems to never be called -> Sciter engine bug
				fnfi_gch.Free();
				fnfr_gch.Free();
				func_gch.Free();
				return true;
			};

			fnfi_gch = GCHandle.Alloc(fnfi, GCHandleType.Normal);
			fnfr_gch = GCHandle.Alloc(fnfr, GCHandleType.Normal);
			func_gch = GCHandle.Alloc(func, GCHandleType.Normal);

			_api.ValueInit(out _data);
			_api.ValueNativeFunctorSet(ref _data, fnfi, fnfr, IntPtr.Zero);
		}
		
		public SciterValue(Action<SciterValue[]> func)
		{
			Interop.SciterValue.FPTR_NATIVE_FUNCTOR_INVOKE fnfi;
			Interop.SciterValue.FPTR_NATIVE_FUNCTOR_RELEASE fnfr;
			var fnfi_gch = new GCHandle();
			var fnfr_gch = new GCHandle();
			var func_gch = GCHandle.Alloc(func);

			fnfi = (IntPtr tag, uint argc, IntPtr argv, out Interop.SciterValue.VALUE retval) =>
			{
				// Get the list of SciterXValue.VALUE from the ptr
				SciterValue[] args = new SciterValue[argc];
				for(var i = 0; i < argc; i++)
					args[i] = new SciterValue((Interop.SciterValue.VALUE)Marshal.PtrToStructure(IntPtr.Add(argv, i * Marshal.SizeOf(typeof(Interop.SciterValue.VALUE))), typeof(Interop.SciterValue.VALUE)));

				func(args);
				retval = new Interop.SciterValue.VALUE();
				return true;
			};

			fnfr = (IntPtr tag) =>
			{
				// seems to never be called -> Sciter engine bug
				fnfi_gch.Free();
				fnfr_gch.Free();
				func_gch.Free();
				return true;
			};

			fnfi_gch = GCHandle.Alloc(fnfi, GCHandleType.Normal);
			fnfr_gch = GCHandle.Alloc(fnfr, GCHandleType.Normal);
			func_gch = GCHandle.Alloc(func, GCHandleType.Normal);

			_api.ValueInit(out _data);
			_api.ValueNativeFunctorSet(ref _data, fnfi, fnfr, IntPtr.Zero);
		}

		public static SciterValue CreateFunctor(object f)
		{
			var sv = new SciterValue();
			List<MethodInfo> l = new List<MethodInfo>();
			foreach(var mi in f.GetType().GetMethods())
			{
				var mparams = mi.GetParameters();
				if(mi.Attributes.HasFlag(MethodAttributes.Public) && mi.ReturnType==typeof(SciterValue) && mparams.Length==1 && mparams[0].ParameterType==typeof(SciterValue[]))
				{
					sv[mi.Name] = new SciterValueFunctor();
				}
			}
			return sv;
		}

		/// <summary>
		/// Create from an arbitrary object, looping recursively through its public properties
		/// </summary>
		public static SciterValue FromObject(object obj)
		{
			List<object> anti_recurse = new List<object>();
			return FromObjectRecurse(obj, anti_recurse, 0);
		}

		private static SciterValue FromObjectRecurse(object obj, List<object> anti_recurse, int deep)
		{
			if(deep++ == 10)
				throw new Exception("Recursion too deep");

			if(obj is IConvertible)
				return new SciterValue(obj as IConvertible);

			if(anti_recurse.Contains(obj))
				throw new Exception("Found recursive property");
			anti_recurse.Add(anti_recurse);

			var t = obj.GetType();
			if(t.GetInterface("IEnumerable") != null)
			{
				var sv_arr = new SciterValue();
				var castMethod = typeof(Enumerable).GetMethod("Cast")
					.MakeGenericMethod(new Type[] { typeof(object) });
				var castedObject = (IEnumerable<object>) castMethod.Invoke(null, new object[] { obj });

				foreach(var item in castedObject)
				{
					sv_arr.Append(FromObjectRecurse(item, anti_recurse, deep));
				}
				return sv_arr;
			}

			var sv = new SciterValue();
			foreach(var prop in t.GetProperties())
			{
				if(prop.CanRead)
				{
					var val = prop.GetValue(obj);
					sv[prop.Name] = FromObjectRecurse(val, anti_recurse, deep);
				}
			}
			return sv;
		}

		/// <summary>
		/// Constructs a TIScript array T[] where T is a basic type like int or string
		/// </summary>
		public static SciterValue FromList<T>(IEnumerable<T> list) where T : /*struct,*/ IConvertible
		{
			Debug.Assert(list != null);

			var sv = new SciterValue();
			if(list.Count()==0)
			{
				_api.ValueIntDataSet(ref sv._data, 0, (uint) Interop.SciterValue.VALUE_TYPE.T_ARRAY, 0);
				return sv;
			}

			var i = 0;
			foreach(var item in list)
			{
				sv.SetItem(i++, new SciterValue(item));
			}
			return sv;
		}

		/// <summary>
		/// Constructs a TIScript key-value object from a dictionary with string as keys and T as values, where T is a basic type like int or string
		/// </summary>
		public static SciterValue FromDictionary<T>(IDictionary<string, T> dic) where T : /*struct,*/ IConvertible
		{
			Debug.Assert(dic != null);

			var sv = new SciterValue();
			foreach(var item in dic)
				sv.SetItem(new SciterValue(item.Key), new SciterValue(item.Value));
			return sv;
		}

		/// <summary>
		/// Constructs a TIScript key-value object from a dictionary with string as keys and SciterValue as values
		/// </summary>
		public static SciterValue FromDictionary(IDictionary<string, SciterValue> dic)
		{
			Debug.Assert(dic != null);

			var sv = new SciterValue();
			foreach(var item in dic)
				sv.SetItem(new SciterValue(item.Key), new SciterValue(item.Value));
			return sv;
		}

		/// <summary>
		/// Returns SciterValue representing error.
		/// If such value is used as a return value from native function the script runtime will throw an error in script rather than returning that value.
		/// </summary>
		public static SciterValue MakeError(string msg)
		{
			if(msg==null)
				return null;

			var sv = new SciterValue();
			_api.ValueStringDataSet(ref sv._data, msg, (uint) msg.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_ERROR);
			return sv;
		}
		
		public static SciterValue MakeSymbol(string sym)
		{
			if(sym == null)
				return null;
			var sv = new SciterValue();
			_api.ValueStringDataSet(ref sv._data, sym, (uint)sym.Length, (uint)Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_SYMBOL);
			return sv;
		}
		
		public static SciterValue MakeColor(uint abgr)
		{
			var sv = new SciterValue();
			_api.ValueIntDataSet(ref sv._data, (int) abgr, (uint)Interop.SciterValue.VALUE_TYPE.T_COLOR, 0);
			return sv;
		}
		
		public static SciterValue MakeDuration(double seconds)
		{
			var sv = new SciterValue();
			_api.ValueFloatDataSet(ref sv._data, seconds, (uint)Interop.SciterValue.VALUE_TYPE.T_DURATION, 0);
			return sv;
		}
		
		public static SciterValue MakeAngle(double seconds)
		{
			var sv = new SciterValue();
			_api.ValueFloatDataSet(ref sv._data, seconds, (uint)Interop.SciterValue.VALUE_TYPE.T_ANGLE, 0);
			return sv;
		}

		public bool IsUndefined => _data.t == (uint)Interop.SciterValue.VALUE_TYPE.T_UNDEFINED;
		public bool IsBool => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL;
		public bool IsInt => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_INT;
		public bool IsFloat => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT;
		public bool IsString => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_STRING;
		public bool IsSymbol => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_STRING && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_SYMBOL;
		public bool IsErrorString => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_STRING && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_ERROR;
		public bool IsDate => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_DATE;
		public bool IsCurrency => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_CURRENCY;
		public bool IsMap => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_MAP;
		public bool IsArray => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_ARRAY;
		public bool IsFunction => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_FUNCTION;
		public bool IsBytes => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_BYTES;
		public bool IsObject => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT;
		public bool IsDomElement => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_DOM_OBJECT;
		public bool IsNativeFunction => _api.ValueIsNativeFunctor(ref _data) != 0;
		public bool IsColor => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_COLOR;
		public bool IsDuration => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_DURATION;
		public bool IsAngle => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_ANGLE;
		public bool IsNull => _data.t == (uint)Interop.SciterValue.VALUE_TYPE.T_NULL;
		public bool IsObjectNative => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_NATIVE;
		public bool IsObjectArray => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_ARRAY;
		public bool IsObjectFunction => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_FUNCTION;
		public bool IsObjectObject => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_OBJECT;
		public bool IsObjectClass => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_CLASS;
		public bool IsObjectError => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_ERROR;

		public bool Get(bool @default = false)
		{
			if (_api.ValueIntData(ref _data, out var pData) == (int) Interop.SciterValue.VALUE_RESULT.HV_OK)
			{
				return pData !=0;
			}
			return @default;
		}

		public int Get(int @default = 0)
		{
			if (_api.ValueIntData(ref _data, out var pData) == (int) Interop.SciterValue.VALUE_RESULT.HV_OK)
			{
				return pData;
			}
			return @default;
		}

		public double Get(double @default = 0d)
		{
			if (_api.ValueFloatData(ref _data, out var pData) == (int) Interop.SciterValue.VALUE_RESULT.HV_OK)
			{
				return pData;
			}
			return @default;
		}

		public string Get(string @default = "")
		{
			if (_api.ValueStringData(ref _data, out var retPtr, out var retLength) ==
			    (int) Interop.SciterValue.VALUE_RESULT.HV_OK)
			{
				return Marshal.PtrToStringUni(retPtr, (int) retLength);
			}
			return @default;
		}

		public byte[] GetBytes()
		{
			if (_api.ValueBinaryData(ref _data, out var retPtr, out var retLength) !=
			    (int) Interop.SciterValue.VALUE_RESULT.HV_OK)
			{
				return null;
			}
			
			byte[] ret = new byte[retLength];
			Marshal.Copy(retPtr, ret, 0, (int) retLength);
			return ret;
		}

		public RGBAColor GetColor()
		{
			Debug.Assert(IsColor);
			return new RGBAColor((uint) Get(0));
		}

		public double GetAngle()
		{
			Debug.Assert(IsAngle);
			return Get(0d);
		}

		public double GetDuration()
		{
			Debug.Assert(IsDuration);
			return Get(0d);
		}

#if WINDOWS || OSX
		public DateTime GetDate()
		{
			_api.ValueInt64Data(ref _data, out var v);
			return DateTime.FromFileTime(v);
		}
#endif
		
		public static SciterValue FromJsonString(string json, Interop.SciterValue.VALUE_STRING_CVT_TYPE ct = Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_JSON_LITERAL)
		{
			var sv = new SciterValue();
			_api.ValueFromString(ref sv._data, json, (uint) json.Length, (uint) ct);
			return sv;
		}

		public string ToJsonString(Interop.SciterValue.VALUE_STRING_CVT_TYPE how = Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_JSON_LITERAL)
		{
			if (how == Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_SIMPLE && IsString)
			{
				return Get("");
			}

			var sv = new SciterValue(this);
			_api.ValueToString(ref sv._data, how);
			return sv.Get("");
		}

		public void Clear()
		{
			_api.ValueClear(out _data);
		}

		public int Length
		{
			get
			{
				_api.ValueElementsCount(ref _data, out var count);
				return count;
			}
		}


		public SciterValue this[int key]
		{
			get => GetItem(key);
			set => SetItem(key, value);
		}

		public SciterValue this[string key]
		{
			get => GetItem(key);
			set => SetItem(key, value);
		}


		public void SetItem(int i, SciterValue val)
		{
			var vr = _api.ValueNthElementValueSet(ref _data, i, ref val._data);
			Debug.Assert(vr == Interop.SciterValue.VALUE_RESULT.HV_OK);
		}

		public void SetItem(SciterValue key, SciterValue val)
		{
			var vr = _api.ValueSetValueToKey(ref _data, ref key._data, ref val._data);
			Debug.Assert(vr == Interop.SciterValue.VALUE_RESULT.HV_OK);
		}

		public void SetItem(string key, SciterValue val)
		{
			var svkey = SciterValue.MakeSymbol(key);
			var vr = _api.ValueSetValueToKey(ref _data, ref svkey._data, ref val._data);
			Debug.Assert(vr == Interop.SciterValue.VALUE_RESULT.HV_OK);
		}

		public void Append(SciterValue val)
		{
			_api.ValueNthElementValueSet(ref _data, Length, ref val._data);
		}

		public SciterValue GetItem(int n)
		{
			var val = new SciterValue();
			_api.ValueNthElementValue(ref _data, n, out val._data);
			return val;
		}

		public SciterValue GetItem(SciterValue vkey)
		{
			var vret = new SciterValue();
			_api.ValueGetValueOfKey(ref _data, ref vkey._data, out vret._data);
			return vret;
		}

		public SciterValue GetItem(string strkey)
		{
			var vret = new SciterValue();
			var vkey = SciterValue.MakeSymbol(strkey);
			_api.ValueGetValueOfKey(ref _data, ref vkey._data, out vret._data);
			return vret;
		}

		public SciterValue GetKey(int n)
		{
			var vret = new SciterValue();
			_api.ValueNthElementKey(ref _data, n, out vret._data);
			return vret;
		}

		public List<SciterValue> Keys
		{
			get
			{
				if(!IsObject && !IsMap)
					throw new ArgumentException("This SciterValue is not an object");
				if(IsObject)
					throw new ArgumentException("Plz, call Isolate() for this SciterValue");
				List<SciterValue> keys = new List<SciterValue>();
				for(var i = 0; i < Length; i++)
					keys.Add(GetKey(i));
				return keys;
			}
		}
		/*public void SetObjectData(IntPtr p)
		{
			Debug.Assert(data.u == (uint) SciterXValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_NATIVE);
			_api.ValueBinaryDataSet(ref data, );
		}*/
		public IntPtr GetObjectData()
		{
			_api.ValueBinaryData(ref _data, out var p, out var dummy);
			return p;
		}


		public SciterValue Call(IList<SciterValue> args, SciterValue self = null, string url_or_script_name = null)
		{
			if(!IsFunction && !IsObjectFunction)
				throw new Exception("Can't Call() this SciterValue because it is not a function");

			var rv = new SciterValue();
			Interop.SciterValue.VALUE[] arr_VALUE = args.Select(sv => sv._data).ToArray();
			if(self == null)
				self = SciterValue.Undefined;

			_api.ValueInvoke(ref _data, ref self._data, (uint) args.Count, args.Count==0 ? null : arr_VALUE, out rv._data, null);
			return rv;
		}

		public SciterValue Call(params SciterValue[] args)
		{
			return Call((IList<SciterValue>) args);
		}

		public void Isolate()
		{
			_api.ValueIsolate(ref _data);
		}


		public IEnumerable<SciterValue> AsEnumerable()
		{
			if(!IsArray && !IsObject && !IsMap)
				throw new ArgumentException("This SciterValue is not an array or object");
			for(var i = 0; i < Length; i++)
			{
				yield return this[i];
			}
		}

		public IDictionary<SciterValue, SciterValue> AsDictionary()
		{
			if(!IsObject && !IsMap)
				throw new ArgumentException("This SciterValue is not an object");

			Dictionary<SciterValue, SciterValue> dic = new Dictionary<SciterValue, SciterValue>();
			for(var i = 0; i < Length; i++)
			{
				dic[GetKey(i)] = GetItem(i);
			}

			return dic;
		}
	}
}