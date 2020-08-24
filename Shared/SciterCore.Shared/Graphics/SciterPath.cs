using System;
using System.Diagnostics;

namespace SciterCore
{
    public class SciterPath : IDisposable
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
			var r = GraphicsApi.pathCreate(out var hpath);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			Debug.Assert(hpath != IntPtr.Zero);

			SciterPath st = new SciterPath(hpath);
			return st;
		}

		public static SciterPath FromSV(SciterValue sv)
		{
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			var r = GraphicsApi.vUnWrapPath(ref v, out var hpath);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);

			SciterPath st = new SciterPath(hpath);
			return st;
		}

		public SciterValue ToSV()
		{
			var r = GraphicsApi.vWrapPath(this.Handle, out var v);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			return new SciterValue(v);
		}

		public void MoveTo(float x, float y, bool relative = false)
		{
			var r = GraphicsApi.pathMoveTo(this.Handle, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void LineTo(float x, float y, bool relative = false)
		{
			var r = GraphicsApi.pathLineTo(this.Handle, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void ArcTo(float x, float y, float angle, float rx, float ry, bool is_large_arc, bool clockwise, bool relative = false)
		{
			var r = GraphicsApi.pathArcTo(this.Handle, x, y, angle, rx, ry, is_large_arc, clockwise, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void QuadraticCurveTo(float xc, float yc, float x, float y, bool relative = false)
		{
			var r = GraphicsApi.pathQuadraticCurveTo(this.Handle, xc, yc, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void BezierCurveTo(float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			var r = GraphicsApi.pathBezierCurveTo(this.Handle, xc1, yc1, xc2, yc2, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void ClosePath()
		{
			var r = GraphicsApi.pathClosePath(this.Handle);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				if(disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				GraphicsApi.pathRelease(this.Handle);
				disposedValue = true;
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