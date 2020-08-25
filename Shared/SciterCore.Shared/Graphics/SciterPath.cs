using System;
using System.Diagnostics;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable ConvertToAutoProperty

namespace SciterCore
{
    public sealed class SciterPath : IDisposable
	{
		private static readonly Interop.SciterGraphics.SciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;

		private readonly IntPtr _pathHandle;
		
		public IntPtr Handle => _pathHandle;

		// non-user usable
		private SciterPath(IntPtr pathHandle)
		{
			if(pathHandle == IntPtr.Zero)
				throw new ArgumentException($"IntPtr.Zero received at {nameof(SciterPath)} constructor.");
			
			_pathHandle = pathHandle;
		}

		public static SciterPath Create()
		{
			var result= GraphicsApi.pathCreate(out var pathHandle)
				.IsOk();

			var st = new SciterPath(pathHandle);
			return st;
		}

		public static SciterPath FromSV(SciterValue sv)
		{
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			var result= GraphicsApi.vUnWrapPath(ref v, out var pathHandle)
				.IsOk();

			var st = new SciterPath(pathHandle);
			return st;
		}

		public SciterValue ToSV()
		{
			var result= GraphicsApi.vWrapPath(this.Handle, out var v)
				.IsOk();
			return new SciterValue(v);
		}

		public void MoveTo(float x, float y, bool relative = false)
		{
			var result= GraphicsApi.pathMoveTo(this.Handle, x, y, relative)
				.IsOk();
		}

		public void LineTo(float x, float y, bool relative = false)
		{
			var result= GraphicsApi.pathLineTo(this.Handle, x, y, relative)
				.IsOk();
		}

		public void ArcTo(float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative = false)
		{
			var result= GraphicsApi.pathArcTo(this.Handle, x, y, angle, rx, ry, isLargeArc, clockwise, relative)
				.IsOk();
		}

		public void QuadraticCurveTo(float xc, float yc, float x, float y, bool relative = false)
		{
			var result= GraphicsApi.pathQuadraticCurveTo(this.Handle, xc, yc, x, y, relative)
				.IsOk();
		}

		public void BezierCurveTo(float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			var result= GraphicsApi.pathBezierCurveTo(this.Handle, xc1, yc1, xc2, yc2, x, y, relative)
				.IsOk();
		}

		public void ClosePath()
		{
			var result= GraphicsApi.pathClosePath(this.Handle)
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

				GraphicsApi.pathRelease(this.Handle);
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