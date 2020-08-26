// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleNamedExpression

namespace SciterCore
{
    public static class SciterPathExtensions
    {
        public static SciterValue ToValue(this SciterPath sciterPath)
        {
	        return sciterPath?.ToValueInternal();
        }
		
        public static bool TryToValue(this SciterPath sciterPath, out SciterValue sciterValue)
        {
	        sciterValue = default;
	        return sciterPath?.TryToValueInternal(sciterValue: out sciterValue) == true;
        }
        
        
		public static SciterPath MoveTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			sciterPath?.MoveToInternal(x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static bool TryMoveTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			return sciterPath?.TryMoveToInternal(x: x, y: y, relative: relative) == true;
		}

		public static SciterPath LineTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			sciterPath?.LineToInternal(x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static bool TryLineTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			return sciterPath?.TryLineToInternal(x: x, y: y, relative: relative) == true;
		}

		public static SciterPath ArcTo(this SciterPath sciterPath, float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative = false)
		{
			sciterPath?.ArcToInternal(x: x, y: y, angle: angle, rx: rx, ry: ry, isLargeArc: isLargeArc, clockwise: clockwise, relative: relative);
			return sciterPath;
		}

		public static bool TryArcTo(this SciterPath sciterPath, float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative = false)
		{
			return sciterPath?.TryArcToInternal(x: x, y: y, angle: angle, rx: rx, ry: ry, isLargeArc: isLargeArc, clockwise: clockwise, relative: relative) == true;
		}

		public static SciterPath QuadraticCurveTo(this SciterPath sciterPath, float xc, float yc, float x, float y, bool relative = false)
		{
			sciterPath?.QuadraticCurveToInternal(xc: xc, yc: yc, x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static bool TryQuadraticCurveTo(this SciterPath sciterPath, float xc, float yc, float x, float y, bool relative = false)
		{
			return sciterPath?.TryQuadraticCurveToInternal(xc: xc, yc: yc, x: x, y: y, relative: relative) == true;
		}

		public static SciterPath BezierCurveTo(this SciterPath sciterPath, float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			sciterPath?.BezierCurveToInternal(xc1: xc1, yc1: yc1, xc2: xc2, yc2: yc2, x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static bool TryBezierCurveTo(this SciterPath sciterPath, float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			return sciterPath?.TryBezierCurveToInternal(xc1: xc1, yc1: yc1, xc2: xc2, yc2: yc2, x: x, y: y, relative: relative) == true;
		}

		public static SciterPath ClosePath(this SciterPath sciterPath)
		{
			sciterPath?.ClosePathInternal();
			return sciterPath;
		}

		public static bool TryClosePath(this SciterPath sciterPath)
		{
			return sciterPath?.TryClosePathInternal() == true;
		}
    }
}