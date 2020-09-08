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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using SciterCore.Interop;
using System.Reflection;
// ReSharper disable RedundantTypeSpecificationInDefaultExpression

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SciterCore
{
	using FUNCTOR = Func<SciterValue[], SciterValue>;

	class SciterValueFunctor : SciterValue
	{
	}

	public class SciterValue
	{
		internal Interop.SciterValue.VALUE _data;
		private static readonly Sciter.SciterApi Api = Sciter.Api;

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
			Api.ValueInit(out var vcopy);
			Api.ValueCopy(out vcopy, ref _data);
			return vcopy;
		}

		public override string ToString()
		{
			var what = "";
			if (IsUndefined) what = nameof(IsUndefined);

			if (IsBool) what += $"{nameof(IsBool)} ({AsBoolean()})";

			if (IsInt) what += $"{nameof(IsInt)} ({AsInt32()})";

			if (IsFloat) what += $"{nameof(IsFloat)} ({AsDouble()})";
			
			if (IsString)
				what = $"{nameof(IsString)} ({AsString(string.Empty)})";

			if(IsSymbol) what = nameof(IsSymbol);
			if(IsErrorString) what = nameof(IsErrorString);
			if(IsDate) what = nameof(IsDate);
			if(IsCurrency) what = $"{nameof(IsCurrency)} ({AsCurrency()})";
			if(IsMap) what = nameof(IsMap);
			if(IsArray) what = nameof(IsArray);
			if(IsFunction) what = nameof(IsFunction);
			if(IsBytes) what = nameof(IsBytes);
			if(IsObject) what = nameof(IsObject);
			if(IsDomElement) what = nameof(IsDomElement);
			if(IsNativeFunction) what = nameof(IsNativeFunction);
			if(IsColor) what = $"{nameof(IsColor)} ({AsColor()})";
			if(IsDuration) what = $"{nameof(IsDuration)} ({AsDuration()})";
			if(IsAngle) what =  $"{nameof(IsAngle)} ({AsAngle()})";
			if(IsNull) what = nameof(IsNull);
			if(IsObjectNative) what = nameof(IsObjectNative);
			if(IsObjectArray) what = nameof(IsObjectArray);
			if(IsObjectFunction) what = nameof(IsObjectFunction);
			if(IsObjectObject) what = nameof(IsObjectObject);
			if(IsObjectClass) what = nameof(IsObjectClass);
			if(IsObjectError) what = nameof(IsObjectError);
#if WINDOWS
			if(IsDate)
				what += $" - {AsDateTime()}";
#endif
			if (IsMap)
			{
				what = $"{nameof(IsMap)} {Regex.Replace(ToJsonString(),@"\t|\n|\r","")}";
			}

			return what;// + " - SciterValue JSON: " + Regex.Replace(ToJSONString(), @"\t|\n|\r", "");
		}

		public SciterValue()
		{
			Api.ValueInit(out _data);
		}

		~SciterValue()
		{
			Api.ValueClear(out _data);
		}
		
		public SciterValue(SciterValue value)
		{
			Api.ValueInit(out _data);
			Api.ValueCopy(out _data, ref value._data);
		}
		
		public SciterValue(Interop.SciterValue.VALUE value)
		{
			Api.ValueInit(out _data);
			Api.ValueCopy(out _data, ref value);
		}

		public static SciterValue Create(bool value)
		{
			//Api.ValueInit(out _data); 
			//Api.ValueIntDataSet(ref _data, value ? 1 : 0, (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL, 0);
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, value ? 1 : 0, (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL, 0);
			return result;
		}

		public static SciterValue Create(int value)
		{
			//Api.ValueInit(out _data);
			//Api.ValueIntDataSet(ref _data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
			return result;
		}
		

		public static SciterValue Create(uint value)
		{
			//Api.ValueInit(out _data);
			//Api.ValueIntDataSet(ref _data, (int) value, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, (int)(value), (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
			return result;
		}

		public static SciterValue Create(double value)
		{
			//Api.ValueInit(out _data); 
			//Api.ValueFloatDataSet(ref _data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
			var result = new SciterValue();
			Api.ValueFloatDataSet(ref result._data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
			return result;
		}

		public static SciterValue Create(string value)
		{
			//Api.ValueInit(out _data); 
			//Api.ValueStringDataSet(ref _data, value, (uint) value.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
			var result = new SciterValue();
			Api.ValueStringDataSet(ref result._data, value, (uint) (value?.Length ?? 0), (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
			return result;
		}

		public static SciterValue Create(byte[] value)
		{
			//Api.ValueInit(out _data); 
			//Api.ValueBinaryDataSet(ref _data, value, (uint) value.Length, (uint) Interop.SciterValue.VALUE_TYPE.T_BYTES, 0);
			var result = new SciterValue();
			Api.ValueBinaryDataSet(ref result._data, value, (uint) (value?.Length ?? 0), (uint) Interop.SciterValue.VALUE_TYPE.T_BYTES, 0);
			return result;
		}

		public static SciterValue Create(DateTime value)
		{
			//Api.ValueInit(out _data); 
			//Api.ValueInt64DataSet(ref _data, value.ToFileTime(), (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
			var result = new SciterValue();
			Api.ValueInt64DataSet(ref result._data, value.ToFileTime(), (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
			return result;
		}
		
		public static SciterValue Create(SciterColor color)
		{
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, (int)color.Value, (uint) Interop.SciterValue.VALUE_TYPE.T_COLOR, 0);
			return result;
		}
		
		public static SciterValue Create(params SciterValue[] values)
		{
			var result = new SciterValue();
			for (var i = 0; i < values.Length; i++)
			{
				result.SetItem(i, values[i]);
			}
			return result;
		}
		
		private SciterValue(IConvertible value)
		{
			Api.ValueInit(out _data);

			switch (value)
			{
				case bool @bool:
					Api.ValueIntDataSet(ref _data, @bool ? 1 : 0, (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL,
						0);
					break;
				case byte @byte:
					Api.ValueIntDataSet(ref _data, @byte, (uint)Interop.SciterValue.VALUE_TYPE.T_INT, 0);
					break;
				case int @int:
					Api.ValueIntDataSet(ref _data, @int, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
					break;
				case uint @uint:
					Api.ValueIntDataSet(ref _data, (int)@uint, (uint)Interop.SciterValue.VALUE_TYPE.T_INT, 0);
					break;
				case double @double:
					Api.ValueFloatDataSet(ref _data, @double, (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
					break;
				case decimal @decimal:
					Api.ValueFloatDataSet(ref _data, (double)@decimal, (uint)Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
					break;
				case string @string:
					Api.ValueStringDataSet(ref _data, @string, (uint) @string.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
					break;
				case DateTime @dateTime:
					Api.ValueInt64DataSet(ref _data, @dateTime.ToFileTime(), (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
					break;
				case SciterColor @sciterColor:
					Api.ValueIntDataSet(ref _data, (int)sciterColor.Value, (uint) Interop.SciterValue.VALUE_TYPE.T_COLOR, 0);
					break;
				default:
					throw new Exception($"Can not create a SciterValue from type '{value.GetType()}'");
			}
		}
		
		private IConvertible FromSciterValue(SciterValue value)
		{
			var valueTypeResult = Api.ValueType(ref value._data, out var valueType, out var units).IsOk();

			if (!valueTypeResult)
				return null;
			
			switch ((Interop.SciterValue.VALUE_TYPE)valueType)
			{
				case Interop.SciterValue.VALUE_TYPE.T_BOOL:
					return value.AsBoolean();
				case Interop.SciterValue.VALUE_TYPE.T_INT:
					return value.AsInt32();
				case Interop.SciterValue.VALUE_TYPE.T_COLOR:
					return value.AsColor();
				//case Interop.SciterValue.VALUE_TYPE.T_UNDEFINED:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_NULL:
				//	break;
				case Interop.SciterValue.VALUE_TYPE.T_FLOAT:
					return value.AsDouble();
				case Interop.SciterValue.VALUE_TYPE.T_STRING:
					return value.AsString();
				case Interop.SciterValue.VALUE_TYPE.T_DATE:
					return value.AsDateTime();
				//case Interop.SciterValue.VALUE_TYPE.T_CURRENCY:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_LENGTH:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_ARRAY:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_MAP:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_FUNCTION:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_BYTES:
				//	return value.AsBytes();
				//case Interop.SciterValue.VALUE_TYPE.T_OBJECT:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_DOM_OBJECT:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_RESOURCE:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_DURATION:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_ANGLE:
				//	break;
				//case Interop.SciterValue.VALUE_TYPE.T_ASSET:
				//	break;
				default:
					throw new Exception($"Can not create a SciterValue from type '{value.GetType()}'");
			}
		}
		
		public SciterValue(Func<SciterValue[], SciterValue> func)
		{
			Interop.SciterValue.NATIVE_FUNCTOR_INVOKE fnfi;
			Interop.SciterValue.NATIVE_FUNCTOR_RELEASE fnfr;
			var fnfi_gch = new GCHandle();
			var fnfr_gch = new GCHandle();
			var func_gch = GCHandle.Alloc(func);

			fnfi = (IntPtr tag, uint argc, IntPtr argv, out Interop.SciterValue.VALUE retval) =>
			{
				// Get the list of SciterXValue.VALUE from the ptr
				var args = new SciterValue[argc];
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

			Api.ValueInit(out _data);
			Api.ValueNativeFunctorSet(ref _data, fnfi, fnfr, IntPtr.Zero);
		}
		
		public SciterValue(Action<SciterValue[]> func)
		{
			Interop.SciterValue.NATIVE_FUNCTOR_INVOKE fnfi;
			Interop.SciterValue.NATIVE_FUNCTOR_RELEASE fnfr;
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

			Api.ValueInit(out _data);
			Api.ValueNativeFunctorSet(ref _data, fnfi, fnfr, IntPtr.Zero);
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
		public static SciterValue Create(object obj)
		{
			var antiRecurse = new List<object>();
			return FromObjectRecurse(obj, ref antiRecurse, 0);
		}

		private static SciterValue FromObjectRecurse(object value, ref List<object> antiRecurse, int depth)
		{
			if(depth++ >= 10)
				throw new InvalidOperationException("Recursion too deep");

			if(value is IConvertible convertible)
				return new SciterValue(convertible);

			if(antiRecurse.Contains(value))
				throw new InvalidOperationException("Found recursive property");
			
			antiRecurse.Add(antiRecurse);

			var t = value.GetType();
			if(typeof(IEnumerable).IsAssignableFrom(t))
			{
				var valueArray = new SciterValue();
				var castMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast))
					.MakeGenericMethod(new Type[] { typeof(object) });
				var castedObject = (IEnumerable<object>) castMethod.Invoke(null, new object[] { value });

				foreach(var item in castedObject)
				{
					valueArray.Append(FromObjectRecurse(item, ref antiRecurse, depth));
				}
				return valueArray;
			}

			var result = new SciterValue();

			foreach(var prop in t.GetProperties())
			{
				if (!prop.CanRead) 
					continue;
				
				var val = prop.GetValue(value);
				result[prop.Name] = FromObjectRecurse(val, ref antiRecurse, depth);
			}

			return result;
		}

		/// <summary>
		/// Constructs a TIScript array T[] where T is a basic type like int or string
		/// </summary>
		public static SciterValue FromList<T>(params T[] value) where T : /*struct,*/ IConvertible
		{
			var sv = new SciterValue();
			if(value?.Any() != true)
			{
				Api.ValueIntDataSet(ref sv._data, 0, (uint) Interop.SciterValue.VALUE_TYPE.T_ARRAY, 0);
				return sv;
			}

			for (var i = 0; i < value.Length; i++)
			{
				sv.SetItem(i, new SciterValue(value[i]));
			}
			//foreach(var item in value)
			//{
			//	sv.SetItem(i++, new SciterValue(item));
			//}
			return sv;
		}

		/// <summary>
		/// Constructs a TIScript key-value object from a dictionary with string as keys and T as values, where T is a basic type like int or string
		/// </summary>
		public static SciterValue Create(IDictionary<string, IConvertible> value)
		{
			Debug.Assert(value != null);

			var sv = new SciterValue();
			foreach(var item in value)
				sv.SetItem(new SciterValue(item.Key), new SciterValue(item.Value));
			return sv;
		}

		/// <summary>
		/// Constructs a TIScript key-value object from a dictionary with string as keys and SciterValue as values
		/// </summary>
		public static SciterValue Create(IDictionary<string, SciterValue> value)
		{
			Debug.Assert(value != null);

			var sv = new SciterValue();
			foreach(var item in value)
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
			Api.ValueStringDataSet(ref sv._data, msg, (uint) msg.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_ERROR);
			return sv;
		}
		
		public static SciterValue MakeSymbol(string sym)
		{
			if(sym == null)
				return null;
			var sv = new SciterValue();
			Api.ValueStringDataSet(ref sv._data, sym, (uint)sym.Length, (uint)Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_SYMBOL);
			return sv;
		}
		
		//public static SciterValue MakeColor(uint abgr)
		//{
		//	var sv = new SciterValue();
		//	Api.ValueIntDataSet(ref sv._data, (int) abgr, (uint)Interop.SciterValue.VALUE_TYPE.T_COLOR, 0);
		//	return sv;
		//}
		
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Duration in seconds</param>
		/// <returns></returns>
		public static SciterValue MakeDuration(double value)
		{
			var sv = new SciterValue();
			Api.ValueFloatDataSet(ref sv._data, value, (uint)Interop.SciterValue.VALUE_TYPE.T_DURATION, 0);
			return sv;
		}
		
		public static SciterValue MakeAngle(double value)
		{
			var sv = new SciterValue();
			Api.ValueFloatDataSet(ref sv._data, value, (uint)Interop.SciterValue.VALUE_TYPE.T_ANGLE, 0);
			return sv;
		}
		
		public static SciterValue MakeCurrency(long value)
		{
			var result = new SciterValue();
			Api.ValueInt64DataSet(ref result._data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_CURRENCY, 0);
			return result;
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
		public bool IsResource => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_RESOURCE;
		public bool IsNativeFunction => Api.ValueIsNativeFunctor(ref _data) != 0;
		public bool IsColor => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_COLOR;
		public bool IsAsset => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_ASSET;
		public bool IsDuration => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_DURATION;
		public bool IsAngle => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_ANGLE;
		public bool IsNull => _data.t == (uint)Interop.SciterValue.VALUE_TYPE.T_NULL;
		public bool IsObjectNative => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_NATIVE;
		public bool IsObjectArray => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_ARRAY;
		public bool IsObjectFunction => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_FUNCTION;
		public bool IsObjectObject => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_OBJECT;
		public bool IsObjectClass => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_CLASS;
		public bool IsObjectError => _data.t == (uint) Interop.SciterValue.VALUE_TYPE.T_OBJECT && _data.u == (uint) Interop.SciterValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_ERROR;

		public bool AsBoolean(bool @default = false)
		{
			if (Api.ValueIntData(ref _data, out var pData) == (int) Interop.SciterValue.VALUE_RESULT.HV_OK)
			{
				return pData !=0;
			}
			return @default;
		}

		public int AsInt32(int @default = default(int))
        {
            return Api.ValueIntData(ref _data, out var pData)
	            .IsOk() ? pData : @default;
        }

		public uint AsUInt32(uint @default = default(uint))
        {
            return Api.ValueIntData(ref _data, out var pData)
	            .IsOk() ? unchecked((uint)pData) : @default;
        }


		public long AsInt64(long @default = default(long))
		{
			return Api.ValueInt64Data(ref _data, out var pData)
				.IsOk() ? pData : @default;
		}
		
		public double AsDouble(double @default = default(double))
        {
            return Api.ValueFloatData(ref _data, out var pData)
	            .IsOk() ? pData : @default;
        }

		public string AsString(string @default = default(string))
        {
            return Api.ValueStringData(ref _data, out var retPtr, out var retLength)
	            .IsOk() ? Marshal.PtrToStringUni(retPtr, (int) retLength) : @default;
        }

		public byte[] AsBytes()
		{
			if (!Api.ValueBinaryData(ref _data, out var retPtr, out var retLength).IsOk())
				return null;

			var ret = new byte[retLength];
			if (retLength > 0)
				Marshal.Copy(retPtr, ret, 0, (int) retLength);
			return ret;
		}

		public SciterColor AsColor()
		{
			if(!IsColor)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_COLOR));

			return new SciterColor(AsUInt32());
		}

		public double AsAngle()
		{
			if(!IsAngle)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_ANGLE));

			return AsDouble();
		}

		public double AsDuration()
		{
			if(!IsDuration)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_DURATION));

			return AsDouble();
		}

		public long AsCurrency()
		{
			if(!IsCurrency)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_CURRENCY));

			return AsInt64();
		}

//#if WINDOWS || OSX || NETCORE
		public DateTime AsDateTime(bool universalTime = true)
		{
			Api.ValueInt64Data(ref _data, out var v);
			var result = DateTime.FromFileTime(v);
			return universalTime ? result.ToUniversalTime() : result;
		}
//#endif
		
		public static SciterValue FromJsonString(string json, Interop.SciterValue.VALUE_STRING_CVT_TYPE ct = Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_JSON_LITERAL)
		{
			var sv = new SciterValue();
			Api.ValueFromString(ref sv._data, json, (uint) json.Length, (uint) ct);
			return sv;
		}

		public string ToJsonString(Interop.SciterValue.VALUE_STRING_CVT_TYPE how = Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_JSON_LITERAL)
		{
			if (how == Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_SIMPLE && IsString)
			{
				return AsString();
			}

			var sv = new SciterValue(this);
			Api.ValueToString(ref sv._data, how);
			return sv.AsString();
		}

		public void Clear()
		{
			Api.ValueClear(out _data);
		}

		public int Length
		{
			get
			{
				Api.ValueElementsCount(ref _data, out var count);
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
		
		public void SetItem(int i, SciterValue value)
		{
			var vr = Api.ValueNthElementValueSet(ref _data, i, ref value._data)
				.IsOk();
			Debug.Assert(vr);
		}

		public void SetItem(SciterValue key, SciterValue value)
		{
			var vr = Api.ValueSetValueToKey(ref _data, ref key._data, ref value._data)
				.IsOk();
			Debug.Assert(vr);
		}

		public void SetItem(string key, SciterValue value)
		{
			var symbol = SciterValue.MakeSymbol(key);
			var vr = Api.ValueSetValueToKey(ref _data, ref symbol._data, ref value._data)
				.IsOk();
			Debug.Assert(vr);
		}

		public void Append(SciterValue value)
		{
			Api.ValueNthElementValueSet(ref _data, Length, ref value._data);
		}

		public SciterValue GetItem(int n)
		{
			Api.ValueNthElementValue(ref _data, n, out var value);
			return new SciterValue(value);
		}

		public SciterValue GetItem(SciterValue key)
		{
			Api.ValueGetValueOfKey(ref _data, ref key._data, out var value);
			return new SciterValue(value);
		}

		public SciterValue GetItem(string key)
		{
			var symbol = SciterValue.MakeSymbol(key);
			Api.ValueGetValueOfKey(ref _data, ref symbol._data, out var value);
			return new SciterValue(value);
		}

		public SciterValue GetKey(int index)
		{
			Api.ValueNthElementKey(ref _data, index, out var value);
			return new SciterValue(value);
		}

		public IReadOnlyList<SciterValue> Keys
		{
			get
			{
				if(!IsObject && !IsMap)
					ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_OBJECT));

				if(IsObject)
					throw new ArgumentException($"Please call {nameof(Isolate)} for this {nameof(SciterValue)}");

				var result = new List<SciterValue>();
				
				for(var i = 0; i < Length; i++)
					result.Add(GetKey(i));
				
				return result;
			}
		}
		
		/*public void SetObjectData(IntPtr p)
		{
			Debug.Assert(data.u == (uint) SciterXValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_NATIVE);
			_api.ValueBinaryDataSet(ref data, );
		}*/
		
		public IntPtr GetObjectData()
		{
			Api.ValueBinaryData(ref _data, out var p, out var dummy);
			return p;
		}

		public SciterValue Call(IList<SciterValue> args, SciterValue self = null, string url_or_script_name = null)
		{
			if(!IsFunction && !IsObjectFunction)
				throw new Exception("Can't Call() this SciterValue because it is not a function");

			var rv = new SciterValue();
			Interop.SciterValue.VALUE[] arr_VALUE = args?.Select(sv => sv._data).ToArray();
			if(self == null)
				self = SciterValue.Undefined;

			var x = Api.ValueInvoke(ref _data, ref self._data, (uint) (args?.Count() ?? 0), args?.Count > 0 ? arr_VALUE : null, out rv._data, null);
			return rv;
		}

		public SciterValue Call(params SciterValue[] args)
		{
			return Call((IList<SciterValue>) args);
		}

		public void Isolate()
		{
			Api.ValueIsolate(ref _data);
		}
		
		public IEnumerable<SciterValue> AsEnumerable()
		{
			if(!IsArray && !IsObject && !IsMap)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_ARRAY), nameof(Interop.SciterValue.VALUE_TYPE.T_OBJECT));

			for(var i = 0; i < Length; i++)
				yield return this[i];
		}

		public IDictionary<SciterValue, SciterValue> AsValueDictionary()
		{
			if(!IsObject && !IsMap)
				ThrowTypeException($"{nameof(IDictionary)}<{nameof(SciterValue)}, {nameof(SciterValue)}>");

			var result = new Dictionary<SciterValue, SciterValue>();
			for(var i = 0; i < Length; i++)
			{
				result[GetKey(i)] = GetItem(i);
			}

			return result;
		}
		
		public IDictionary<string, IConvertible> AsDictionary()
		{
			if (!IsObject && !IsMap) 
				ThrowTypeException($"{nameof(IDictionary)}<{nameof(String)}, {nameof(IConvertible)}>");

			var result = new Dictionary<string, IConvertible>();
			for(var i = 0; i < Length; i++)
			{
				result.Add(GetKey(i).AsString(), FromSciterValue(GetItem(i)));
			}

			return result;
		}
		
		private static void ThrowTypeException(params string[] typeNames)
		{
			throw new InvalidOperationException($"{nameof(SciterValue)} is not of {nameof(Type)} `{string.Join(" | ", typeNames)}`");
		}
	}
}