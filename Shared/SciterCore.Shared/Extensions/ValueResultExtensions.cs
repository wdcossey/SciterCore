using System.Linq;
using SciterCore.Interop;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SciterTest.CoreForms.Extensions
{
    internal static class ValueResultExtensions
    {
        internal static bool IsOk(this SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterValue.VALUE_RESULT.HV_OK);
        }
        
        internal static bool IsOkTrue(this SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterValue.VALUE_RESULT.HV_OK_TRUE);
        }
        
        internal static bool IsBadParameter(this SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterValue.VALUE_RESULT.HV_BAD_PARAMETER);
        }
        
        internal static bool IsIncompatibleType(this SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterValue.VALUE_RESULT.HV_INCOMPATIBLE_TYPE);
        }
        
        internal static bool IsTrue(this SciterValue.VALUE_RESULT result, SciterValue.VALUE_RESULT value)
        {
            return result == value;
        }
        
        internal static bool IsTrue(this SciterValue.VALUE_RESULT result, params SciterValue.VALUE_RESULT[] values)
        {
            return values?.Contains(result) == true;
        }
        
        internal static bool IsFalse(this SciterValue.VALUE_RESULT result, SciterValue.VALUE_RESULT value)
        {
            return !result.IsTrue(value);
        }

        internal static bool IsFalse(this SciterValue.VALUE_RESULT result, params SciterValue.VALUE_RESULT[] values)
        {
            return !IsTrue(result, values);
        }
    }
}