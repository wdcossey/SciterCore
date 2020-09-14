using System;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable UnusedMethodReturnValue.Global

namespace SciterCore
{
    public static class SciterValueExtensions
    {
	    
	    #region Json
	    

	    public static string AsJsonString(this SciterValue sciterValue, StringConversionType conversionType = StringConversionType.JsonLiteral)
	    {
		    return sciterValue?.AsJsonStringInternal(conversionType: conversionType);
	    }

	    public static bool TryAsJsonString(this SciterValue sciterValue, out string value, StringConversionType conversionType = StringConversionType.JsonLiteral)
	    {
		    value = null;
		    return sciterValue?.TryAsJsonStringInternal(value: out value, conversionType: conversionType) == true;
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
        
        public static IReadOnlyList<SciterValue> GetKeysInternal(this SciterValue sciterValue)
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