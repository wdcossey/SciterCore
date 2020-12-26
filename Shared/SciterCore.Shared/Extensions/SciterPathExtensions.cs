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

		public static SciterPath MoveTo(this SciterPath sciterPath, double x, double y, bool relative = false)
		{
			return sciterPath?.MoveTo(x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y),
				relative: relative);
		}

		public static bool TryMoveTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			return sciterPath?.TryMoveToInternal(x: x, y: y, relative: relative) == true;
		}

		public static bool TryMoveTo(this SciterPath sciterPath, double x, double y, bool relative = false)
		{
			return sciterPath?.TryMoveTo(x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y),
				relative: relative) == true;
		}

		public static SciterPath LineTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			sciterPath?.LineToInternal(x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static SciterPath LineTo(this SciterPath sciterPath, double x, double y, bool relative = false)
		{
			sciterPath?.LineTo(x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y), relative: relative);
			return sciterPath;
		}

		public static bool TryLineTo(this SciterPath sciterPath, float x, float y, bool relative = false)
		{
			return sciterPath?.TryLineToInternal(x: x, y: y, relative: relative) == true;
		}

		public static bool TryLineTo(this SciterPath sciterPath, double x, double y, bool relative = false)
		{
			return sciterPath?.TryLineTo(x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y),
				relative: relative) == true;
		}

		public static SciterPath ArcTo(this SciterPath sciterPath, float x, float y, float angle, float rx, float ry,
			bool isLargeArc, bool clockwise, bool relative = false)
		{
			sciterPath?.ArcToInternal(x: x, y: y, angle: angle, rx: rx, ry: ry, isLargeArc: isLargeArc,
				clockwise: clockwise, relative: relative);
			return sciterPath;
		}

		public static SciterPath ArcTo(this SciterPath sciterPath, double x, double y, double angle, double rx, double ry,
			bool isLargeArc, bool clockwise, bool relative = false)
		{
			return sciterPath?.ArcTo(x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y),
				angle: System.Convert.ToSingle(angle), rx: System.Convert.ToSingle(rx), ry: System.Convert.ToSingle(ry),
				isLargeArc: isLargeArc,
				clockwise: clockwise, relative: relative);
		}

		public static bool TryArcTo(this SciterPath sciterPath, float x, float y, float angle, float rx, float ry,
			bool isLargeArc, bool clockwise, bool relative = false)
		{
			return sciterPath?.TryArcToInternal(x: x, y: y, angle: angle, rx: rx, ry: ry, isLargeArc: isLargeArc,
				clockwise: clockwise, relative: relative) == true;
		}

		public static bool TryArcTo(this SciterPath sciterPath, double x, double y, double angle, double rx, double ry,
			bool isLargeArc, bool clockwise, bool relative = false)
		{
			return sciterPath?.TryArcTo(x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y),
				angle: System.Convert.ToSingle(angle), rx: System.Convert.ToSingle(rx), ry: System.Convert.ToSingle(ry),
				isLargeArc: isLargeArc,
				clockwise: clockwise, relative: relative) == true;
		}

		public static SciterPath QuadraticCurveTo(this SciterPath sciterPath, float xc, float yc, float x, float y,
			bool relative = false)
		{
			sciterPath?.QuadraticCurveToInternal(xc: xc, yc: yc, x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static SciterPath QuadraticCurveTo(this SciterPath sciterPath, double xc, double yc, double x, double y,
			bool relative = false)
		{
			return sciterPath?.QuadraticCurveTo(xc: System.Convert.ToSingle(xc), yc: System.Convert.ToSingle(yc),
				x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y), relative: relative);
		}

		public static bool TryQuadraticCurveTo(this SciterPath sciterPath, float xc, float yc, float x, float y,
			bool relative = false)
		{
			return sciterPath?.TryQuadraticCurveToInternal(xc: xc, yc: yc, x: x, y: y, relative: relative) == true;
		}

		public static bool TryQuadraticCurveTo(this SciterPath sciterPath, double xc, double yc, double x, double y,
			bool relative = false)
		{
			return sciterPath?.TryQuadraticCurveTo(xc: System.Convert.ToSingle(xc), yc: System.Convert.ToSingle(yc),
				x: System.Convert.ToSingle(x), y: System.Convert.ToSingle(y), relative: relative) == true;
		}

		public static SciterPath BezierCurveTo(this SciterPath sciterPath, float xc1, float yc1, float xc2, float yc2,
			float x, float y, bool relative = false)
		{
			sciterPath?.BezierCurveToInternal(xc1: xc1, yc1: yc1, xc2: xc2, yc2: yc2, x: x, y: y, relative: relative);
			return sciterPath;
		}

		public static SciterPath BezierCurveTo(this SciterPath sciterPath, double xc1, double yc1, double xc2,
			double yc2, double x, double y, bool relative = false)
		{
			return sciterPath?.BezierCurveTo(xc1: System.Convert.ToSingle(xc1), yc1: System.Convert.ToSingle(yc1),
				xc2: System.Convert.ToSingle(xc2), yc2: System.Convert.ToSingle(yc2), x: System.Convert.ToSingle(x),
				y: System.Convert.ToSingle(y), relative: relative);
		}

		public static bool TryBezierCurveTo(this SciterPath sciterPath, float xc1, float yc1, float xc2, float yc2,
			float x, float y, bool relative = false)
		{
			return sciterPath?.TryBezierCurveToInternal(xc1: xc1, yc1: yc1, xc2: xc2, yc2: yc2, x: x, y: y,
				relative: relative) == true;
		}

		public static bool TryBezierCurveTo(this SciterPath sciterPath, double xc1, double yc1, double xc2, double yc2,
			double x, double y, bool relative = false)
		{
			return sciterPath?.TryBezierCurveTo(xc1: System.Convert.ToSingle(xc1), yc1: System.Convert.ToSingle(yc1),
				xc2: System.Convert.ToSingle(xc2), yc2: System.Convert.ToSingle(yc2), x: System.Convert.ToSingle(x),
				y: System.Convert.ToSingle(y), relative: relative) == true;
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