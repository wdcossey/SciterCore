using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SciterCore
{
    internal static class GraphinResultExtensions
    {
        internal static bool IsOk(this Interop.SciterGraphics.GRAPHIN_RESULT result)
        {
            return result.IsTrue(Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
        }
        
        internal static bool IsPanic(this Interop.SciterGraphics.GRAPHIN_RESULT result)
        {
            return result.IsTrue(Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_PANIC);
        }
        
        internal static bool IsFailure(this Interop.SciterGraphics.GRAPHIN_RESULT result)
        {
            return result.IsTrue(Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_FAILURE);
        }
        
        internal static bool IsBadParameter(this Interop.SciterGraphics.GRAPHIN_RESULT result)
        {
            return result.IsTrue(Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_BAD_PARAM);
        }
        
        internal static bool IsNotSupported(this Interop.SciterGraphics.GRAPHIN_RESULT result)
        {
            return result.IsTrue(Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_NOTSUPPORTED);
        }
        
        internal static bool IsTrue(this Interop.SciterGraphics.GRAPHIN_RESULT result, Interop.SciterGraphics.GRAPHIN_RESULT value)
        {
            return result == value;
        }
        
        internal static bool IsTrue(this Interop.SciterGraphics.GRAPHIN_RESULT result, params Interop.SciterGraphics.GRAPHIN_RESULT[] values)
        {
            return values?.Contains(result) == true;
        }
        
        internal static bool IsFalse(this Interop.SciterGraphics.GRAPHIN_RESULT result,Interop.SciterGraphics.GRAPHIN_RESULT value)
        {
            return !result.IsTrue(value);
        }

        internal static bool IsFalse(this Interop.SciterGraphics.GRAPHIN_RESULT result, params Interop.SciterGraphics.GRAPHIN_RESULT[] values)
        {
            return !IsTrue(result, values);
        }
    }
}