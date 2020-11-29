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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using SciterCore.Interop;
using System.Reflection;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable RedundantTypeSpecificationInDefaultExpression
// ReSharper disable ArgumentsStyleNamedExpression
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
				_data =
				{
					t = (uint) Interop.SciterValue.VALUE_TYPE.T_NULL
				}
			};
		}

		public Interop.SciterValue.VALUE ToVALUE()
		{
			Api.ValueInit(out var copy);
			Api.ValueCopy(out copy, ref _data);
			return copy;
		}

		#region Override(s)

		[ExcludeFromCodeCoverage]
		public override string ToString()
		{
			var what = "";
			if (IsUndefined) 
				what = nameof(IsUndefined);

			if (IsBool) 
				what += $"{nameof(IsBool)} ({AsBooleanInternal()})";

			if (IsInt) 
				what += $"{nameof(IsInt)} ({AsInt32Internal()})";

			if (IsFloat) 
				what += $"{nameof(IsFloat)} ({AsDoubleInternal()})";
			
			if (IsString)
				what = $"{nameof(IsString)} ({AsStringInternal(string.Empty)})";

			if (IsSymbol)
			{
				what = nameof(IsSymbol);
				if (IsString)
					what += $" ({AsStringInternal(string.Empty)})";
			}
			
			if (IsErrorString) 
				what = nameof(IsErrorString);
			
			if (IsDate) 
				what = $"{nameof(IsDate)} ({AsInt64Internal()})";
			
			if (IsCurrency) 
				what = $"{nameof(IsCurrency)} ({AsCurrencyInternal()})";
			
			if (IsMap) 
				what = nameof(IsMap);
			
			if (IsArray) 
				what = nameof(IsArray);
			
			if (IsFunction) 
				what = nameof(IsFunction);
			
			if (IsBytes) 
				what = nameof(IsBytes);
			
			if (IsObject) 
				what = nameof(IsObject);
			
			if (IsDomElement) 
				what = nameof(IsDomElement);
			
			if (IsNativeFunction) 
				what = nameof(IsNativeFunction);
			
			if (IsColor) 
				what = $"{nameof(IsColor)} ({AsColorInternal()})";
			
			if (IsDuration) 
				what = $"{nameof(IsDuration)} ({AsDurationInternal()})";
			
			if (IsAngle) 
				what =  $"{nameof(IsAngle)} ({AsAngleInternal()})";
			
			if (IsNull) 
				what = nameof(IsNull);
			
			if (IsObjectNative) 
				what = nameof(IsObjectNative);
			
			if (IsObjectArray) 
				what = nameof(IsObjectArray);
			
			if (IsObjectFunction) 
				what = nameof(IsObjectFunction);
			
			if (IsObjectObject) 
				what = nameof(IsObjectObject);
			
			if (IsObjectClass) 
				what = nameof(IsObjectClass);
			
			if (IsObjectError) 
				what = nameof(IsObjectError);
			
			if (IsMap)
			{
				what = $"{nameof(IsMap)} {Regex.Replace(this.AsJsonString(),@"\t|\n|\r","")}";
			}

			return what;// + " - SciterValue JSON: " + Regex.Replace(ToJSONString(), @"\t|\n|\r", "");
		}

		#endregion
		
		#region Constructor(s)
		
		public SciterValue()
		{
			Api.ValueInit(out _data);
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
		
		#endregion

		~SciterValue()
		{
			Api.ValueClear(out _data);
		}

		#region Create
		
		public static SciterValue Create(bool value)
		{
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, value ? 1 : 0, (uint) Interop.SciterValue.VALUE_TYPE.T_BOOL, 0);
			return result;
		}

		public static SciterValue Create(int value)
		{
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
			return result;
		}
		

		public static SciterValue Create(uint value)
		{
			var result = new SciterValue();
			Api.ValueIntDataSet(ref result._data, (int)(value), (uint) Interop.SciterValue.VALUE_TYPE.T_INT, 0);
			return result;
		}

		public static SciterValue Create(double value)
		{
			var result = new SciterValue();
			Api.ValueFloatDataSet(ref result._data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
			return result;
		}

		public static SciterValue Create(string value)
		{
			var result = new SciterValue();
			Api.ValueStringDataSet(ref result._data, value, (uint) (value?.Length ?? 0), (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
			return result;
		}

		public static SciterValue Create(byte[] value)
		{
			var result = new SciterValue();
			Api.ValueBinaryDataSet(ref result._data, value, (uint) (value?.Length ?? 0), (uint) Interop.SciterValue.VALUE_TYPE.T_BYTES, 0);
			return result;
		}

		public static SciterValue Create(long value)
		{
			var result = new SciterValue();
			Api.ValueInt64DataSet(ref result._data, value, (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
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
		
		///<summary>
		/// Creates a <see cref="SciterValue"/> from an arbitrary object, looping recursively through its public properties
		///</summary>
		public static SciterValue Create(object obj)
		{
			var antiRecurse = new List<object>();
			return FromObjectRecurse(obj, ref antiRecurse, 0);
		}

		/// <summary>
		/// Constructs a TIScript key-value object from a dictionary with string as keys and SciterValue as values
		/// </summary>
		public static SciterValue Create(IDictionary<string, SciterValue> value)
		{
			Debug.Assert(value != null);

			var result = new SciterValue();
			// ReSharper disable once UseDeconstruction
			foreach(var item in value)
				result.SetItem(new SciterValue(item.Key), new SciterValue(item.Value));
			return result;
		}
		
		#endregion
		
		#region Private Methods
		
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
				case float @float:
					Api.ValueFloatDataSet(ref _data, (float)@float, (uint)Interop.SciterValue.VALUE_TYPE.T_FLOAT, 0);
					break;
				case string @string:
					Api.ValueStringDataSet(ref _data, @string, (uint) @string.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_STRING);
					break;
				case long int64:
					Api.ValueInt64DataSet(ref _data, int64, (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
					break;
				case DateTime dateTime:
					Api.ValueInt64DataSet(ref _data, dateTime.ToFileTime(), (uint)Interop.SciterValue.VALUE_TYPE.T_DATE, 0);
					break;
				case SciterColor sciterColor:
					Api.ValueIntDataSet(ref _data, (int)sciterColor.Value, (uint) Interop.SciterValue.VALUE_TYPE.T_COLOR, 0);
					break;
				case null:
					Api.ValueClear(out _data);
					_data.t = (uint) Interop.SciterValue.VALUE_TYPE.T_NULL;
					break;
				default:
					throw new Exception($"Can not create a SciterValue from type '{value.GetType()}'");
			}
		}
		
		private IConvertible FromSciterValue(SciterValue value)
		{
			var valueTypeResult = Api.ValueType(ref value._data, out var valueType, out var _)
				.IsOk();
			
			if (!valueTypeResult)
				return null;
			
			switch ((Interop.SciterValue.VALUE_TYPE)valueType)
			{
				case Interop.SciterValue.VALUE_TYPE.T_NULL:
					return null;
				case Interop.SciterValue.VALUE_TYPE.T_BOOL:
					return value.AsBooleanInternal();
				case Interop.SciterValue.VALUE_TYPE.T_INT:
					return value.AsInt32Internal();
				case Interop.SciterValue.VALUE_TYPE.T_COLOR:
					return value.AsColorInternal();
				//case Interop.SciterValue.VALUE_TYPE.T_UNDEFINED:
				//	break;
				case Interop.SciterValue.VALUE_TYPE.T_FLOAT:
					return value.AsDoubleInternal();
				case Interop.SciterValue.VALUE_TYPE.T_STRING:
					return value.AsStringInternal();
				case Interop.SciterValue.VALUE_TYPE.T_DATE:
					return value.AsInt64Internal();
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
		
		private static SciterValue FromObjectRecurse(object value, ref List<object> antiRecurse, int depth)
		{
			if(depth++ >= 10)
				throw new InvalidOperationException("Recursion too deep");
			
			if (value == null)
				return SciterValue.Null;
			
			if (value is IConvertible convertible)
				return new SciterValue(convertible);

			if (antiRecurse.Contains(value))
				throw new InvalidOperationException("Found recursive property");
			
			antiRecurse.Add(antiRecurse);

			var t = value.GetType();
			if(typeof(IEnumerable).IsAssignableFrom(t))
			{
				var valueArray = new SciterValue();
				var castMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast))
					?.MakeGenericMethod(typeof(object));
				var castedObject = (IEnumerable<object>) castMethod?.Invoke(null, new[] { value });

				// ReSharper disable once InvertIf
				if (castedObject != null)
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

		#endregion
		
		//TODO: Clean this up
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
					args[i] = new SciterValue(Marshal.PtrToStructure<Interop.SciterValue.VALUE>(IntPtr.Add(argv, i * Marshal.SizeOf(typeof(Interop.SciterValue.VALUE)))));

				retval = func.Invoke(args).ToVALUE();
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
		
		//TODO: Clean this up
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
					args[i] = new SciterValue(Marshal.PtrToStructure<Interop.SciterValue.VALUE>(IntPtr.Add(argv, i * Marshal.SizeOf(typeof(Interop.SciterValue.VALUE)))));

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

		//TODO: Clean this up
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
		
		
		///// <summary>
		///// Constructs a TIScript array T[] where T is a basic type like int or string
		///// </summary>
		//public static SciterValue FromList<T>(params T[] value) where T : /*struct,*/ IConvertible
		//{
		//	var result = new SciterValue();
		//	if(value?.Any() != true)
		//	{
		//		Api.ValueIntDataSet(ref result._data, 0, (uint) Interop.SciterValue.VALUE_TYPE.T_ARRAY, 0);
		//		return result;
		//	}
//
		//	for (var i = 0; i < value.Length; i++)
		//	{
		//		result.SetItem(i, new SciterValue(value[i]));
		//	}
		//	//foreach(var item in value)
		//	//{
		//	//	sv.SetItem(i++, new SciterValue(item));
		//	//}
		//	return result;
		//}

		/// <summary>
		/// Constructs a TIScript array T[] where T is a basic type like int or string
		/// </summary>
		public static SciterValue FromList<T>(IEnumerable<T> value) where T : /*struct,*/ IConvertible
		{
			var result = new SciterValue();
			var convertibles = value as T[] ?? value.ToArray();
			
			if(convertibles?.Any() != true)
			{
				Api.ValueIntDataSet(ref result._data, 0, (uint) Interop.SciterValue.VALUE_TYPE.T_ARRAY, 0);
				return result;
			}

			var i = 0;
			foreach (var convertible in convertibles)
				result.SetItem(i++, new SciterValue(convertible));
			
			return result;
		}

		/// <summary>
		/// Constructs a TIScript key-value object from a dictionary with string as keys and T as values, where T is a basic type like int or string
		/// </summary>
		public static SciterValue Create(IDictionary<string, IConvertible> value)
		{
			Debug.Assert(value != null);

			var result = new SciterValue();
			foreach(var item in value)
				result.SetItem(new SciterValue(item.Key), new SciterValue(item.Value));
			return result;
		}

		#region Make
		
		///<summary>
		/// Returns SciterValue representing error.
		/// If such value is used as a return value from native function the script runtime will throw an error in script rather than returning that value.
		///</summary>
		public static SciterValue MakeError(string msg)
		{
			if(string.IsNullOrWhiteSpace(msg))
				return null;

			var result = new SciterValue();
			Api.ValueStringDataSet(ref result._data, msg, (uint) msg.Length, (uint) Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_ERROR);
			return result;
		}

		public static SciterValue MakeSymbol(string sym)
		{
			if(string.IsNullOrWhiteSpace(sym))
				return null;

			var result = new SciterValue();
			Api.ValueStringDataSet(ref result._data, sym, (uint)sym.Length, (uint)Interop.SciterValue.VALUE_UNIT_TYPE_STRING.UT_STRING_SYMBOL);
			return result;
		}
		
		//public static SciterValue MakeColor(uint abgr)
		//{
		//	var sv = new SciterValue();
		//	Api.ValueIntDataSet(ref sv._data, (int) abgr, (uint)Interop.SciterValue.VALUE_TYPE.T_COLOR, 0);
		//	return sv;
		//}

		/// <summary>
		/// Creates a <see cref="SciterValue"/> (<see cref="SciterCore.Interop.SciterValue.VALUE_TYPE.T_DURATION"/>) from a <see cref="Double"/>
		/// </summary>
		/// <param name="value">Duration in seconds</param>
		/// <returns></returns>
		public static SciterValue MakeDuration(double value)
		{
			var sv = new SciterValue();
			Api.ValueFloatDataSet(ref sv._data, value, (uint)Interop.SciterValue.VALUE_TYPE.T_DURATION, 0);
			return sv;
		}
		
		/// <summary>
		/// Creates a <see cref="SciterValue"/> (<see cref="SciterCore.Interop.SciterValue.VALUE_TYPE.T_ANGLE"/>) from a <see cref="Double"/>
		/// </summary>
		/// <param name="value">Angle in degrees (i.e. 0, 25.5, -90, etc)</param>
		/// <returns></returns>
		public static SciterValue MakeAngle(double value)
		{
			var sv = new SciterValue();
			Api.ValueFloatDataSet(ref sv._data, value, (uint)Interop.SciterValue.VALUE_TYPE.T_ANGLE, 0);
			return sv;
		}

		/// <summary>
		/// Creates a <see cref="SciterValue"/> (<see cref="SciterCore.Interop.SciterValue.VALUE_TYPE.T_CURRENCY"/>) from a <see cref="Int64"/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SciterValue MakeCurrency(long value)
		{
			var result = new SciterValue();
			Api.ValueInt64DataSet(ref result._data, value, (uint) Interop.SciterValue.VALUE_TYPE.T_CURRENCY, 0);
			return result;
		}
		
		/// <summary>
		/// Creates a <see cref="SciterValue"/> (<see cref="SciterCore.Interop.SciterValue.VALUE_TYPE.T_DATE"/>) from a <see cref="DateTime"/>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SciterValue MakeDateTime(DateTime value)
		{
			return Create(value.ToFileTime());
		}

		#endregion

		#region Is Type
		
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

		#endregion

		#region To

		//TODO: Clean this up
		public object ToObject()
		{
			if (IsUndefined) 
				return null;

			if (IsBool) 
				return AsBooleanInternal();

			if (IsInt) 
				return AsInt32Internal();

			if (IsFloat) 
				return AsDoubleInternal();
			
			if (IsString)
				return AsStringInternal(string.Empty);

			if (IsSymbol)
				return AsStringInternal(string.Empty);
			
			//if (IsErrorString) 
			//	what = nameof(IsErrorString);
			
			if (IsDate) 
				return AsInt64Internal();
			
			if (IsCurrency) 
				return AsCurrencyInternal();
			
			//if (IsMap) 
			//	what = nameof(IsMap);
			
			//if (IsArray) 
			//	what = nameof(IsArray);
			
			//if (IsFunction) 
			//	what = nameof(IsFunction);
			
			//if (IsBytes) 
			//	what = nameof(IsBytes);
			
			//if (IsObject) 
			//	what = nameof(IsObject);
			
			//if (IsDomElement) 
			//	what = nameof(IsDomElement);
			
			//if (IsNativeFunction) 
			//	what = nameof(IsNativeFunction);
			
			if (IsColor) 
				return AsColorInternal();
			
			if (IsDuration) 
				return AsDurationInternal();
			
			if (IsAngle) 
				return AsAngleInternal();
			
			if (IsNull) 
				return null;
			
			return null;
			
			//if (IsObjectNative) 
			//	what = nameof(IsObjectNative);
			
			//if (IsObjectArray) 
			//	what = nameof(IsObjectArray);
			
			//if (IsObjectFunction) 
			//	what = nameof(IsObjectFunction);
			
			//if (IsObjectObject) 
			//	what = nameof(IsObjectObject);
			
			//if (IsObjectClass) 
			//	what = nameof(IsObjectClass);
			
			//if (IsObjectError) 
			//	what = nameof(IsObjectError);
			
			//if (IsMap)
			//{
			//	what = $"{nameof(IsMap)} {Regex.Replace(this.AsJsonString(),@"\t|\n|\r","")}";
			//}

			//return what;// + " - SciterValue JSON: " + Regex.Replace(ToJSONString(), @"\t|\n|\r", "");
		}

		#endregion
		
		#region As

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Boolean"/>
		/// </summary>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool AsBooleanInternal(bool @default = default(bool))
		{
			TryAsBooleanInternal(value: out var result, @default: @default);
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Boolean"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool TryAsBooleanInternal(out bool value, bool @default = default(bool))
		{
			var result = Api.ValueIntData(ref _data, out var pData)
				.IsOk();
			value = result ? pData != 0 : @default;
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Int32"/>
		/// </summary>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal int AsInt32Internal(int @default = default(int))
        {
	        TryAsInt32Internal(value: out var result, @default: @default);
	        return result;
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Int32"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool TryAsInt32Internal(out int value, int @default = default(int))
		{
			var result = Api.ValueIntData(ref _data, out var pData)
				.IsOk();
			value = result ? pData : @default;
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="UInt32"/>
		/// </summary>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal uint AsUInt32Internal(uint @default = default(uint))
        {
	        TryAsUInt32Internal(value: out var result, @default: @default);
	        return result;
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="UInt32"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool TryAsUInt32Internal(out uint value, uint @default = default(uint))
        {
	        var result = Api.ValueIntData(ref _data, out var pData)
	            .IsOk();
	        value = result ? unchecked((uint)pData) : @default;
            return result;
        }
		
		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Int64"/>
		/// </summary>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal long AsInt64Internal(long @default = default(long))
		{
			TryAsInt64Internal(value: out var result, @default: @default);
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Int64"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool TryAsInt64Internal(out long value, long @default = default(long))
		{
			var result = Api.ValueInt64Data(ref _data, out var pData)
				.IsOk();
			value = result ? pData : @default;
			return result;
		}
		
		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Double"/>
		/// </summary>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal double AsDoubleInternal(double @default = default(double))
        {
	        TryAsDoubleInternal(value: out var result, @default: @default);
	        return result;
        }
		
		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Double"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool TryAsDoubleInternal(out double value, double @default = default(double))
        {
	        var result = Api.ValueFloatData(ref _data, out var pData)
	            .IsOk();
	        value = result ? pData : @default;
	        return result;
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="String"/>
		/// </summary>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal string AsStringInternal(string @default = default(string))
		{
			TryAsStringInternal(out var result, @default: @default);
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="String"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		internal bool TryAsStringInternal(out string value, string @default = default(string))
        {
            var result = Api.ValueStringData(ref _data, out var retPtr, out var retLength)
	            .IsOk();
            value = result ? Marshal.PtrToStringUni(retPtr, (int) retLength) : @default;
            return result;
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="byte"/> <see cref="Array"/>
		/// </summary>
		/// <returns></returns>
		internal byte[] AsBytesInternal()
		{
			TryAsBytesInternal(out var result);
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="byte"/> <see cref="Array"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		internal bool TryAsBytesInternal(out byte[] value)
		{
			var result = Api.ValueBinaryData(ref _data, out var retPtr, out var retLength)
				.IsOk();
			value = result ? new byte[retLength] : null;
			if (result && retLength > 0)
				Marshal.Copy(retPtr, value, 0, unchecked((int)retLength));
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="SciterColor"/>
		/// </summary>
		/// <returns></returns>
		internal SciterColor AsColorInternal()
		{
			TryAsColorInternal(out var result);
			return result;
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="SciterColor"/>
		/// </summary>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		internal bool TryAsColorInternal(out SciterColor value)
		{
			if(!IsColor)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_COLOR));

			var result = TryAsUInt32Internal(out var intValue);
			
			value = result ? new SciterColor(intValue) : SciterColor.Transparent;
			return result;
		}

		internal double AsAngleInternal()
		{
			TryAsAngleInternal(out var result);
			return result;
		}

		internal bool TryAsAngleInternal(out double value)
		{
			if(!IsAngle)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_ANGLE));

			return TryAsDoubleInternal(out value);
		}

		internal double AsDurationInternal()
		{
			TryAsDurationInternal(out var result);
			return result;
		}

		internal bool TryAsDurationInternal(out double value)
		{
			if(!IsDuration)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_DURATION));

			return TryAsDoubleInternal(out value);
		}

		internal long AsCurrencyInternal()
		{
			TryAsCurrencyInternal(out var result);
			return result;
		}

		internal bool TryAsCurrencyInternal(out long value)
		{
			if(!IsCurrency)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_CURRENCY));

			return TryAsInt64Internal(out value);
		}

//#if WINDOWS || OSX || NETCORE

		internal DateTime AsDateTimeInternal(bool universalTime = true)
		{
			TryAsDateTimeInternal(out var result, universalTime: universalTime);
			return result;
		}

		internal bool TryAsDateTimeInternal(out DateTime value, bool universalTime = true)
		{
			var result = TryAsInt64Internal(out var intValue);
			
			value = result ? DateTime.FromFileTime(intValue) : DateTime.MinValue;

			if (universalTime)
				value = value.ToUniversalTime();

			return result;
		}
		
//#endif
		
		internal IEnumerable<SciterValue> AsEnumerableInternal()
		{
			if(!IsArray && !IsObject && !IsMap)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_ARRAY), nameof(Interop.SciterValue.VALUE_TYPE.T_OBJECT));

			for(var i = 0; i < Length; i++)
				yield return this[i];
		}

		internal IDictionary<SciterValue, SciterValue> AsValueDictionaryInternal()
		{
			if(!IsObject && !IsMap)
				ThrowTypeException($"{nameof(IDictionary)}<{nameof(SciterValue)}, {nameof(SciterValue)}>");

			var result = new Dictionary<SciterValue, SciterValue>();
			for(var i = 0; i < Length; i++)
			{
				result[GetKeyInternal(i)] = GetItemInternal(i);
			}

			return result;
		}
		
		internal IDictionary<string, IConvertible> AsDictionaryInternal()
		{
			if (!IsObject && !IsMap) 
				ThrowTypeException($"{nameof(IDictionary)}<{nameof(String)}, {nameof(IConvertible)}>");

			var result = new Dictionary<string, IConvertible>();
			for(var i = 0; i < Length; i++)
			{
				result.Add(GetKeyInternal(i).AsStringInternal(), FromSciterValue(GetItemInternal(i)));
			}

			return result;
		}
		
		internal SciterElement AsElementInternal()
		{
			if(!IsResource && !IsDomElement)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_RESOURCE), nameof(Interop.SciterValue.VALUE_TYPE.T_DOM_OBJECT));
			
			return new SciterElement(this.GetObjectData());
		}
		
		public static SciterValue FromJsonString(string json, StringConversionType conversionType = StringConversionType.JsonLiteral)
		{
			var result = new SciterValue();
			Api.ValueFromString(ref result._data, json, (uint) json.Length, (uint)(SciterCore.Interop.SciterValue.VALUE_STRING_CVT_TYPE)conversionType);
			return result;
		}

		internal string AsJsonStringInternal(StringConversionType conversionType = StringConversionType.JsonLiteral)
		{
			TryAsJsonStringInternal(value: out var result, conversionType: conversionType);
			return result;
		}

		internal bool TryAsJsonStringInternal(out string value, StringConversionType conversionType = StringConversionType.JsonLiteral)
		{
			value = null;
			
			if (conversionType == StringConversionType.Simple && IsString)
				return TryAsStringInternal(out value);

			var sciterValue = new SciterValue(this);
			var result = Api.ValueToString(ref sciterValue._data, (Interop.SciterValue.VALUE_STRING_CVT_TYPE)(uint)conversionType)
				.IsOk();

			value = result ? sciterValue.AsStringInternal() : null;
			
			return result;
		}

		#endregion
		
		#region Clear
		
		internal void ClearInternal()
		{
			TryClearInternal();
		}
		
		internal bool TryClearInternal()
		{
			return Api.ValueClear(out _data).IsOk();
		}
		
		#endregion

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
			get => GetItemInternal(key);
			set => SetItemInternal(key, value);
		}

		public SciterValue this[string key]
		{
			get => GetItemInternal(key);
			set => SetItemInternal(key, value);
		}

		#region Item Operations
		
		internal void SetItemInternal(int index, SciterValue value)
		{
			TrySetItemInternal(index: index, value: value);
		}
		
		internal bool TrySetItemInternal(int index, SciterValue value)
		{
			return Api.ValueNthElementValueSet(ref _data, index, ref value._data)
				.IsOk();
		}

		internal void SetItemInternal(SciterValue key, SciterValue value)
		{
			TrySetItemInternal(key: key, value: value);
		}

		internal bool TrySetItemInternal(SciterValue key, SciterValue value)
		{
			return Api.ValueSetValueToKey(ref _data, ref key._data, ref value._data)
				.IsOk();
		}

		internal void SetItemInternal(string key, SciterValue value)
		{
			TrySetItemInternal(key: key, value: value);
		}

		internal bool TrySetItemInternal(string key, SciterValue value)
		{
			var symbol = MakeSymbol(key);
			return Api.ValueSetValueToKey(ref _data, ref symbol._data, ref value._data)
				.IsOk();
		}

		internal void AppendInternal(SciterValue value)
		{
			TryAppendInternal(value);
		}

		internal bool TryAppendInternal(SciterValue value)
		{
			return Api.ValueNthElementValueSet(ref _data, Length, ref value._data)
				.IsOk();
		}

		internal SciterValue GetItemInternal(int index)
		{
			TryGetItemInternal(value: out var result, index: index);
			return result;
		}

		internal bool TryGetItemInternal(out SciterValue value, int index)
		{
			var result = Api.ValueNthElementValue(ref _data, index, out var intValue)
				.IsOk();
			value = result ? new SciterValue(intValue) : Null;
			return result;
		}

		internal SciterValue GetItemInternal(SciterValue key)
		{
			TryGetItemInternal(out var result, key);
			return result;
		}

		internal bool TryGetItemInternal(out SciterValue value, SciterValue key)
		{
			var result = Api.ValueGetValueOfKey(ref _data, ref key._data, out var intValue)
				.IsOk();
			value = result ? new SciterValue(intValue) : Null;
			return result;
		}

		internal SciterValue GetItemInternal(string key)
		{
			TryGetItemInternal(out var result, key);
			return result;
		}

		internal bool TryGetItemInternal(out SciterValue value, string key)
		{
			var symbol = MakeSymbol(key);
			value = Null;
			return TryGetItemInternal(out value, symbol);;
		}

		#endregion
		
		#region Keys
		
		internal SciterValue GetKeyInternal(int index)
		{
			TryGetKeyInternal(value: out var result, index: index);
			return result;
		}
		
		internal bool TryGetKeyInternal(out SciterValue value, int index)
		{
			var result = Api.ValueNthElementKey(ref _data, index, out var intValue)
				.IsOk();

			value = result ? new SciterValue(intValue) : Null;
			
			return result;
		}

		public IReadOnlyList<SciterValue> Keys => GetKeysInternal();

		internal IReadOnlyList<SciterValue> GetKeysInternal()
		{
			if(!IsObject && !IsMap)
				ThrowTypeException(nameof(Interop.SciterValue.VALUE_TYPE.T_OBJECT));

			if(IsObject)
				throw new InvalidOperationException($"Please call {nameof(SciterCore.SciterValueExtensions.Isolate)} for this {nameof(SciterValue)}");

			var result = new List<SciterValue>();
				
			for(var i = 0; i < Length; i++)
				result.Add(GetKeyInternal(i));
				
			return result;
		}
		
		
		#endregion
		
		/*public void SetObjectData(IntPtr p)
		{
			Debug.Assert(data.u == (uint) SciterXValue.VALUE_UNIT_TYPE_OBJECT.UT_OBJECT_NATIVE);
			_api.ValueBinaryDataSet(ref data, );
		}*/
		
		#region GetObjectData
		
		internal IntPtr GetObjectDataInternal()
		{
			TryGetObjectDataInternal(out var result);
			return result;
		}
		
		internal bool TryGetObjectDataInternal(out IntPtr value)
		{
			return Api.ValueBinaryData(ref _data, out value, out var dummy)
				.IsOk();
		}
		
		#endregion

		#region Invoke
		
		internal SciterValue InvokeInternal(IList<SciterValue> args, SciterValue self = null, string urlOrScriptName = null)
		{
			TryInvokeInternal(value: out var value, args: args, self: self, urlOrScriptName: urlOrScriptName);
			return value;
		}

		internal bool TryInvokeInternal(out SciterValue value, IList<SciterValue> args, SciterValue self = null, string urlOrScriptName = null)
		{
			if(!IsFunction && !IsObjectFunction)
				throw new Exception("Can't Call() this SciterValue because it is not a function");

			value = new SciterValue();
			
			var valueArray = args?.Select(sv => sv._data).ToArray();
			
			self ??= Undefined;

			var result = Api.ValueInvoke(ref _data, ref self._data, (uint) (args?.Count() ?? 0), args?.Count > 0 ? valueArray : null, out value._data, null)
				.IsOk();

			return result;
		}

		internal SciterValue InvokeInternal(params SciterValue[] args)
		{
			return InvokeInternal(args: (IList<SciterValue>) args);
		}

		internal bool TryInvokeInternal(out SciterValue value, params SciterValue[] args)
		{
			return TryInvokeInternal(value: out value, args: (IList<SciterValue>) args);
		}
		
		#endregion

		#region Isolate

		internal bool TryIsolateInternal()
		{
			return Api.ValueIsolate(ref _data).IsOk();
		}
		
		#endregion

		private static void ThrowTypeException(params string[] typeNames)
		{
			throw new InvalidOperationException($"{nameof(SciterValue)} is not of {nameof(Type)} `{string.Join(" | ", typeNames)}`");
		}
	}
}