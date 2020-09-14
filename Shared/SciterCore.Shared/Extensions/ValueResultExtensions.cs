using System.Diagnostics.CodeAnalysis;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SciterCore
{
    [ExcludeFromCodeCoverage]
    internal static class ValueResultExtensions
    {
        internal static bool IsOk(this SciterCore.Interop.SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterCore.Interop.SciterValue.VALUE_RESULT.HV_OK);
        }
        
        internal static bool IsOkTrue(this SciterCore.Interop.SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterCore.Interop.SciterValue.VALUE_RESULT.HV_OK_TRUE);
        }
        
        internal static bool IsBadParameter(this SciterCore.Interop.SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterCore.Interop.SciterValue.VALUE_RESULT.HV_BAD_PARAMETER);
        }
        
        internal static bool IsIncompatibleType(this SciterCore.Interop.SciterValue.VALUE_RESULT result)
        {
            return result.IsTrue(SciterCore.Interop.SciterValue.VALUE_RESULT.HV_INCOMPATIBLE_TYPE);
        }
        
        internal static bool IsTrue(this SciterCore.Interop.SciterValue.VALUE_RESULT result, SciterCore.Interop.SciterValue.VALUE_RESULT value)
        {
            return result == value;
        }
        
        internal static bool IsTrue(this SciterCore.Interop.SciterValue.VALUE_RESULT result, params SciterCore.Interop.SciterValue.VALUE_RESULT[] values)
        {
            return values?.Contains(result) == true;
        }
        
        internal static bool IsFalse(this SciterCore.Interop.SciterValue.VALUE_RESULT result, SciterCore.Interop.SciterValue.VALUE_RESULT value)
        {
            return !result.IsTrue(value);
        }

        internal static bool IsFalse(this SciterCore.Interop.SciterValue.VALUE_RESULT result, params SciterCore.Interop.SciterValue.VALUE_RESULT[] values)
        {
            return !IsTrue(result, values);
        }
    }
}