using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SciterCore.Interop
{
    internal static class DomResultExtensions
    {
        internal static bool IsOk(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_OK);
        }
        
        internal static bool IsInvalidHwnd(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_INVALID_HWND);
        }
        
        internal static bool IsOkNotHandled(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED);
        }
        
        internal static bool IsInvalidHandle(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_INVALID_HANDLE);
        }
        
        internal static bool IsPassiveHandle(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_PASSIVE_HANDLE);
        }
        
        internal static bool IsOperationFailed(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_OPERATION_FAILED);
        }
        
        internal static bool IsInvalidParameter(this SciterXDom.SCDOM_RESULT result)
        {
            return result.IsTrue(SciterXDom.SCDOM_RESULT.SCDOM_INVALID_PARAMETER);
        }
        
        internal static bool IsTrue(this SciterXDom.SCDOM_RESULT result, SciterXDom.SCDOM_RESULT value)
        {
            return result == value;
        }
        
        internal static bool IsTrue(this SciterXDom.SCDOM_RESULT result, params SciterXDom.SCDOM_RESULT[] values)
        {
            return values?.Contains(result) == true;
        }
        
        internal static bool IsFalse(this SciterXDom.SCDOM_RESULT result, SciterXDom.SCDOM_RESULT value)
        {
            return !result.IsTrue(value);
        }

        internal static bool IsFalse(this SciterXDom.SCDOM_RESULT result, params SciterXDom.SCDOM_RESULT[] values)
        {
            return !IsTrue(result, values);
        }
    }
}