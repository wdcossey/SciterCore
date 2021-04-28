using System;
using SciterCore.Interop;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable ConvertToAutoProperty

namespace SciterCore
{
    public sealed class SciterPath : IDisposable
	{
		private static readonly ISciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;

		private readonly IntPtr _pathHandle;
		
		public IntPtr Handle => _pathHandle;

		// non-user usable
		private SciterPath(IntPtr pathHandle)
		{
			if (pathHandle == IntPtr.Zero)
				throw new ArgumentOutOfRangeException(
					paramName: nameof(pathHandle),
					message: $"IntPtr.Zero received at {nameof(SciterPath)} constructor.");
			
			_pathHandle = pathHandle;
		}

		#region Create
		
		public static SciterPath Create()
		{
			TryCreatePrivate(sciterPath: out var result, ignoreResult: true);
			return result;
		}
		
		public static bool TryCreate(out SciterPath sciterPath)
		{
			return TryCreatePrivate(sciterPath: out sciterPath, ignoreResult: false);
		}

		private static bool TryCreatePrivate(out SciterPath sciterPath, bool ignoreResult)
		{
			var result= GraphicsApi.PathCreate(out var pathHandle)
				.IsOk();
			
			sciterPath = (result || ignoreResult) ? new SciterPath(pathHandle: pathHandle) : default;
			
			return result;
		}
		
		#endregion

		#region Create w/ SciterValue

		public static SciterPath Create(SciterValue sciterValue)
		{
			TryCreatePrivate(sciterPath: out var result, sciterValue: sciterValue, ignoreResult: true);
			return result;
		}
		
		public static bool TryCreate(out SciterPath sciterPath, SciterValue sciterValue)
		{
			return TryCreatePrivate(sciterPath: out sciterPath, sciterValue: sciterValue, ignoreResult: false);
		}
		
		private static bool TryCreatePrivate(out SciterPath sciterPath, SciterValue sciterValue, bool ignoreResult)
		{
			var value = sciterValue.ToVALUE();
			var result= GraphicsApi.ValueUnWrapPath(ref value, out var pathHandle)
				.IsOk();

			sciterPath = (result || ignoreResult) ? new SciterPath(pathHandle: pathHandle) : default;
			
			return result;
		}
		
		#endregion

		#region ToValue
		
		internal SciterValue ToValueInternal()
		{
			TryToValuePrivate(sciterValue: out var result, ignoreResult: true);
			return result;
		}
		
		internal bool TryToValueInternal(out SciterValue sciterValue)
		{
			return TryToValuePrivate(sciterValue: out sciterValue, ignoreResult: false);
		}
		
		private bool TryToValuePrivate(out SciterValue sciterValue, bool ignoreResult)
		{
			var result= GraphicsApi.ValueWrapPath(this.Handle, out var value)
				.IsOk();

			sciterValue =  (result || ignoreResult) ? SciterValue.Attach(value) : default;
			return result;
		}
		
		#endregion

		internal void MoveToInternal(float x, float y, bool relative = false)
		{
			TryMoveToInternal(x: x, y: y, relative: relative);
		}

		internal bool TryMoveToInternal(float x, float y, bool relative = false)
		{
			return GraphicsApi.PathMoveTo(this.Handle, x, y, relative)
				.IsOk();
		}

		internal void LineToInternal(float x, float y, bool relative = false)
		{
			TryLineToInternal(x: x, y: y, relative: relative);
		}

		internal bool TryLineToInternal(float x, float y, bool relative = false)
		{
			return GraphicsApi.PathLineTo(this.Handle, x, y, relative)
				.IsOk();
		}

		internal void ArcToInternal(float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative = false)
		{
			TryArcToInternal(x: x, y: y, angle: angle, rx: rx, ry: ry, isLargeArc: isLargeArc, clockwise: clockwise, relative: relative);
		}

		internal bool TryArcToInternal(float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative = false)
		{
			return GraphicsApi.PathArcTo(this.Handle, x, y, angle, rx, ry, isLargeArc, clockwise, relative)
				.IsOk();
		}

		internal void QuadraticCurveToInternal(float xc, float yc, float x, float y, bool relative = false)
		{
			TryQuadraticCurveToInternal(xc: xc, yc: yc, x: x, y: y, relative: relative);
		}

		internal bool TryQuadraticCurveToInternal(float xc, float yc, float x, float y, bool relative = false)
		{
			return GraphicsApi.PathQuadraticCurveTo(this.Handle, xc, yc, x, y, relative)
				.IsOk();
		}

		internal void BezierCurveToInternal(float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			TryBezierCurveToInternal(xc1: xc1, yc1: yc1, xc2: xc2, yc2: yc2, x: x, y: y, relative: relative);
		}

		internal bool TryBezierCurveToInternal(float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			return GraphicsApi.PathBezierCurveTo(this.Handle, xc1, yc1, xc2, yc2, x, y, relative)
				.IsOk();
		}

		internal void ClosePathInternal()
		{
			TryClosePathInternal();
		}

		internal bool TryClosePathInternal()
		{
			return GraphicsApi.PathClosePath(this.Handle)
				.IsOk();
		}

		#region IDisposable
		
		private bool _disposedValue = false; // To detect redundant calls

		private void Dispose(bool disposing)
		{
			if(!_disposedValue)
			{
				if(disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				GraphicsApi.PathRelease(this.Handle);
				_disposedValue = true;
			}
		}

		~SciterPath()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		#endregion
	}
}