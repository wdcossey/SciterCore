using System;
using System.Collections.Generic;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression
// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable UnusedMethodReturnValue.Global

namespace SciterCore
{
    public static class SciterValueExtensions
    {
	    #region As

	    /// <summary>
	    /// Reads the <see cref="SciterValue"/> as a <see cref="Boolean"/>
	    /// </summary>
	    /// <param name="sciterValue"></param>
	    /// <param name="default">Default value to return on error</param>
	    /// <returns></returns>
	    public static bool AsBoolean(this SciterValue sciterValue, bool @default = default(bool))
		{
			return sciterValue.AsBooleanInternal(@default: @default);
		}

	    /// <summary>
	    /// Reads the <see cref="SciterValue"/> as a <see cref="Boolean"/>
	    /// </summary>
	    /// <param name="sciterValue"></param>
	    /// <param name="value">The output value</param>
	    /// <param name="default">Default value to return on error</param>
	    /// <returns></returns>
	    public static bool TryAsBoolean(this SciterValue sciterValue, out bool value, bool @default = default(bool))
		{
			return sciterValue.TryAsBooleanInternal(out value, @default: @default);
		}

	    /// <summary>
	    /// Reads the <see cref="SciterValue"/> as a <see cref="Int32"/>
	    /// </summary>
	    /// <param name="sciterValue"></param>
	    /// <param name="default">Default value to return on error</param>
	    /// <returns></returns>
	    public static int AsInt32(this SciterValue sciterValue, int @default = default(int))
        {
	        return sciterValue.AsInt32Internal(@default: @default);
        }

	    /// <summary>
	    /// Reads the <see cref="SciterValue"/> as a <see cref="Int32"/>
	    /// </summary>
	    /// <param name="sciterValue"></param>
	    /// <param name="value">The output value</param>
	    /// <param name="default">Default value to return on error</param>
	    /// <returns></returns>
	    public static bool TryAsInt32(this SciterValue sciterValue, out int value, int @default = default(int))
		{
			return sciterValue.TryAsInt32Internal(out value, @default: @default);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="UInt32"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static uint AsUInt32(this SciterValue sciterValue, uint @default = default(uint))
        {
	        return sciterValue.AsUInt32Internal(@default: @default);
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="UInt32"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static bool TryAsUInt32(this SciterValue sciterValue, out uint value, uint @default = default(uint))
		{
			return sciterValue.TryAsUInt32Internal(out value, @default: @default);
		}
		
		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Int64"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static long AsInt64(this SciterValue sciterValue, long @default = default(long))
		{
			return sciterValue.AsInt64Internal(@default: @default);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Int64"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static bool TryAsInt64(this SciterValue sciterValue, out long value, long @default = default(long))
		{
			return sciterValue.TryAsInt64Internal(out value, @default: @default);
		}
		
		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Double"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static double AsDouble(this SciterValue sciterValue, double @default = default(double))
        {
	        return sciterValue.AsDoubleInternal(@default: @default);
        }
		
		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="Double"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static bool TryAsDouble(this SciterValue sciterValue, out double value, double @default = default(double))
        {
	        return sciterValue.TryAsDoubleInternal(out value, @default: @default);
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="String"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static string AsString(this SciterValue sciterValue, string @default = default(string))
		{
			return sciterValue.AsStringInternal(@default: @default);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="String"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <param name="default">Default value to return on error</param>
		/// <returns></returns>
		public static bool TryAsString(this SciterValue sciterValue, out string value, string @default = default(string))
        {
	        return sciterValue.TryAsStringInternal(out value, @default: @default);
        }

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="byte"/> <see cref="Array"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <returns></returns>
		public static byte[] AsBytes(this SciterValue sciterValue)
		{
			return sciterValue.AsBytesInternal();
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="byte"/> <see cref="Array"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		public static bool TryAsBytes(this SciterValue sciterValue, out byte[] value)
		{
			return sciterValue.TryAsBytesInternal(out value);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="SciterColor"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <returns></returns>
		public static SciterColor AsColor(this SciterValue sciterValue)
		{
			return sciterValue.AsColorInternal();
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="SciterColor"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		public static bool TryAsColor(this SciterValue sciterValue, out SciterColor value)
		{
			return sciterValue.TryAsColorInternal(out value);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a Angle (<see cref="Double"/>)
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <returns></returns>
		public static double AsAngle(this SciterValue sciterValue)
		{
			return sciterValue.AsAngleInternal();
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a Angle (<see cref="Double"/>)
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		public static bool TryAsAngle(this SciterValue sciterValue, out double value)
		{
			return sciterValue.TryAsAngleInternal(out value);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a Duration (<see cref="SciterColor"/>)
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <returns></returns>
		public static double AsDuration(this SciterValue sciterValue)
		{
			return sciterValue.AsDurationInternal();
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a Duration (<see cref="SciterColor"/>)
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		public static bool TryAsDuration(this SciterValue sciterValue, out double value)
		{
			return sciterValue.TryAsDurationInternal(out value);
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a Currency (<see cref="SciterColor"/>)
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <returns></returns>
		public static long AsCurrency(this SciterValue sciterValue)
		{
			return sciterValue.AsCurrencyInternal();
		}

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a Currency (<see cref="SciterColor"/>)
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="value">The output value</param>
		/// <returns></returns>
		public static bool TryAsCurrency(this SciterValue sciterValue, out long value)
		{
			return sciterValue.TryAsCurrencyInternal(out value);
		}

//#if WINDOWS || OSX || NETCORE

		/// <summary>
		/// Reads the <see cref="SciterValue"/> as a <see cref="DateTime"/>
		/// </summary>
		/// <param name="sciterValue"></param>
		/// <param name="universalTime"></param>
		/// <returns></returns>
		public static DateTime AsDateTime(this SciterValue sciterValue, bool universalTime = true)
		{
			return sciterValue.AsDateTimeInternal(universalTime: universalTime);
		}

	    /// <summary>
	    /// Reads the <see cref="SciterValue"/> as a <see cref="DateTime"/>
	    /// </summary>
	    /// <param name="sciterValue"></param>
	    /// <param name="value">The output value</param>
	    /// <param name="universalTime"></param>
	    /// <returns></returns>
	    public static bool TryAsDateTime(this SciterValue sciterValue, out DateTime value, bool universalTime = true)
		{
			value = DateTime.MinValue;
			return sciterValue.TryAsDateTimeInternal(out value, universalTime: universalTime);
		}
		
//#endif
		
	    public static IEnumerable<SciterValue> AsEnumerable(this SciterValue sciterValue)
		{
			return sciterValue.AsEnumerableInternal();
		}

	    public static IDictionary<SciterValue, SciterValue> AsValueDictionary(this SciterValue sciterValue)
		{
			return sciterValue.AsValueDictionaryInternal();
		}
		
	    public static IDictionary<string, IConvertible> AsDictionary(this SciterValue sciterValue)
		{
			return sciterValue.AsDictionaryInternal();
		}
		
	    public static SciterElement AsElement(this SciterValue sciterValue)
		{
			return sciterValue.AsElementInternal();
		}
	    
	    public static string AsJsonString(this SciterValue sciterValue, StringConversionType conversionType = StringConversionType.JsonLiteral)
	    {
		    return sciterValue?.AsJsonStringInternal(conversionType: conversionType);
	    }

	    public static bool TryAsJsonString(this SciterValue sciterValue, out string value, StringConversionType conversionType = StringConversionType.JsonLiteral)
	    {
		    value = null;
		    return sciterValue?.TryAsJsonStringInternal(value: out value, conversionType: conversionType) == true;
	    }
	    
	    
	    //TODO: Work in progress
	    public static T MapTo<T>(this SciterValue sciterValue)
			where T: class, new()
	    {
		    
		    if (sciterValue.IsObjectObject)
			    sciterValue = sciterValue.Isolate();
		    
		    if (!sciterValue.IsMap)
		    {
			    throw new ArgumentOutOfRangeException(nameof(sciterValue), $"{nameof(sciterValue)} cannot be converted to Map");
		    }
		    
		    var result = new T();

		    foreach (var property in typeof(T).GetProperties())
		    {
			    sciterValue.TryGetItemInternal(out var value, property.Name);
			    property.SetValue(result, value.ToObject());
		    }

		    return result;
	    }

	    //TODO: Work in progress
	    public static bool TryMapTo<T>(this SciterValue sciterValue, out T value)
		    where T: class, new()
	    {
		    value = default;
		    return false;
	    }

	    #endregion
	    
	    #region Clear
		
	    public static SciterValue Clear(this SciterValue sciterValue)
	    {
		    sciterValue?.ClearInternal();
		    return sciterValue;
	    }
		
	    public static bool TryClear(this SciterValue sciterValue)
	    {
		    return sciterValue?.TryClearInternal() == true;
	    }
		
	    #endregion
	    
        #region Item Operations

        public static SciterValue SetItem(this SciterValue sciterValue, int index, SciterValue value)
		{
			sciterValue?.SetItemInternal(index: index, value: value);
			return sciterValue;
		}
		
        public static bool TrySetItem(this SciterValue sciterValue, int index, SciterValue value)
		{
			return sciterValue?.TrySetItemInternal(index: index, value: value) == true;
		}

		public static SciterValue SetItem(this SciterValue sciterValue, SciterValue key, SciterValue value)
		{
			sciterValue?.SetItemInternal(key: key, value: value);
			return sciterValue;
		}

		public static bool TrySetItem(this SciterValue sciterValue, SciterValue key, SciterValue value)
		{
			return sciterValue?.TrySetItemInternal(key: key, value: value) == true;
		}

		public static SciterValue SetItem(this SciterValue sciterValue, string key, SciterValue value)
		{
			sciterValue?.SetItemInternal(key: key, value: value);
			return sciterValue;
		}

		public static bool TrySetItem(this SciterValue sciterValue, string key, SciterValue value)
		{
			return sciterValue?.TrySetItemInternal(key: key, value: value) == true;
		}

		public static SciterValue Append(this SciterValue sciterValue, SciterValue value)
		{
			sciterValue?.AppendInternal(value: value);
			return sciterValue;
		}

		public static bool TryAppend(this SciterValue sciterValue, SciterValue value)
		{
			return sciterValue?.TryAppendInternal(value: value) == true;
		}

		public static SciterValue GetItem(this SciterValue sciterValue, int index)
		{
			return sciterValue?.GetItemInternal(index: index);
		}

		public static bool TryGetItem(this SciterValue sciterValue, out SciterValue value, int index)
		{
			value = SciterValue.Null;
			return sciterValue?.TryGetItemInternal(value: out value, index: index) == true;
		}

		public static SciterValue GetItem(this SciterValue sciterValue, SciterValue key)
		{
			return sciterValue?.GetItemInternal(key: key);
		}

		public static bool TryGetItem(this SciterValue sciterValue, out SciterValue value, SciterValue key)
		{
			value = SciterValue.Null;
			return sciterValue?.TryGetItemInternal(value: out value, key: key) == true;
		}

		public static SciterValue GetItem(this SciterValue sciterValue, string key)
		{
			return sciterValue?.GetItemInternal(key: key);
		}

		public static bool TryGetItem(this SciterValue sciterValue, out SciterValue value, string key)
		{
			value = SciterValue.Null;
			return sciterValue?.TryGetItemInternal(value: out value, key: key) == true;
		}

        #endregion
        
        #region Keys
        
        public static SciterValue GetKey(this SciterValue sciterValue, int index)
        {
            return sciterValue?.GetKeyInternal(index);
        }
		
        public static bool TryGetKey(this SciterValue sciterValue, out SciterValue value, int index)
        {
            value = SciterValue.Null;
            return sciterValue?.TryGetKeyInternal(out value, index) == true;
        }
        
        public static IReadOnlyList<SciterValue> GetKeys(this SciterValue sciterValue)
        {
            return sciterValue?.GetKeysInternal();
        }
        
        #endregion
        
        #region GetObjectData
		
        public static IntPtr GetObjectData(this SciterValue sciterValue)
        {
            return sciterValue?.GetObjectDataInternal() ?? IntPtr.Zero;
        }
		
        public static bool TryGetObjectData(this SciterValue sciterValue, out IntPtr value)
        {
            value = IntPtr.Zero;
            return sciterValue?.TryGetObjectDataInternal(out value) == true;
        }
		
        #endregion
        
        #region Invoke

        public static SciterValue Invoke(this SciterValue sciterValue, IList<SciterValue> args, SciterValue self = null, string urlOrScriptName = null)
        {
            return sciterValue?.InvokeInternal(args: args, self: self, urlOrScriptName: urlOrScriptName);
        }

        public static bool TryInvoke(this SciterValue sciterValue, out SciterValue value, IList<SciterValue> args, SciterValue self = null, string urlOrScriptName = null)
        {
            value = new SciterValue();
            return sciterValue?.TryInvokeInternal(out value, args: args, self: self, urlOrScriptName: urlOrScriptName) == true;
        }

        public static SciterValue Invoke(this SciterValue sciterValue, params SciterValue[] args)
        {
            return sciterValue?.InvokeInternal(args: args);
        }

        public static bool TryInvoke(this SciterValue sciterValue, out SciterValue value, params SciterValue[] args)
        {
            value = new SciterValue();
            return sciterValue?.TryInvokeInternal(out value, args: args) == true;
        }

        #endregion

        #region Isolate

        public static SciterValue Isolate(this SciterValue sciterValue)
        {
            sciterValue?.TryIsolateInternal();
            return sciterValue;
        }
        
        public static bool TryIsolate(this SciterValue sciterValue)
        {
            return sciterValue?.TryIsolateInternal() == true;
        }

        #endregion

    }
}