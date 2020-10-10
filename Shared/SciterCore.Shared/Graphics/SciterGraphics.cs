// Copyright 2016 Ramon F. Mendes
//
// This file is part of SciterSharp.
// 
// SciterSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SciterSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SciterSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
#if WINDOWS && !WPF
using System.Drawing;
using System.Drawing.Imaging;
#elif WINDOWS && WPF
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
#elif OSX && XAMARIN
using Foundation;
using CoreGraphics;
#endif

// ReSharper disable ArrangeThisQualifier
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace SciterCore
{
	
	public sealed class SciterGraphics : IDisposable
	{
		private static readonly Interop.SciterGraphics.SciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;
		private readonly IntPtr _graphicsHandle;

		// ReSharper disable once ConvertToAutoProperty
		public IntPtr Handle => _graphicsHandle;

		internal SciterGraphics(IntPtr graphicsHandle)
		{
			if(graphicsHandle == IntPtr.Zero)
				throw new ArgumentException($"IntPtr.Zero received at {nameof(SciterGraphics)} constructor.");
			
			_graphicsHandle = graphicsHandle;
			GraphicsApi.gAddRef(graphicsHandle);
		}

		public static SciterGraphics Create(IntPtr graphicsHandle)
		{
			return new SciterGraphics(graphicsHandle: graphicsHandle);
		}

		public static SciterGraphics Create(SciterValue sciterValue)
		{
			TryCreate(sciterValue: sciterValue, sciterGraphics: out var result);
			return result;
		}

		public static bool TryCreate(SciterValue sciterValue, out SciterGraphics sciterGraphics)
		{
			var value = sciterValue.ToVALUE();
			var result = GraphicsApi.vUnWrapGfx(ref value, out var graphicsHandle)
				.IsOk();

			sciterGraphics = result ? new SciterGraphics(graphicsHandle: graphicsHandle) : default;
			
			return result;
		}

		internal SciterValue ToValueInternal()
		{
			TryToValueInternal(out var result);
			return result;
		}

		internal bool TryToValueInternal(out SciterValue sciterValue)
		{
			var result = GraphicsApi.vWrapGfx(this.Handle, out var value)
				.IsOk();
			
			sciterValue = result ? new SciterValue(value: value) : default;
			
			return result;
		}

		
		/*DON'T KNOW IF IT WORKS AND IF YOU MUST CALL gAddRef()
		SO NOT SAFE
		public SciterGraphics(SciterImage img)
		{
			var r = GraphicsApi.gCreate(img.Handle, out var gfx);
			Debug.Assert(r.IsOk());
		}
		*/
		
		internal void BlendImageInternal(SciterImage img, float x = 0f, float y = 0f)
		{
			TryBlendImageInternal(img: img, x: x, y: y);
		}
		
		internal bool TryBlendImageInternal(SciterImage img, float x = 0f, float y = 0f)
		{
			//float w, h, ix, iy, iw, ih, opacity;
			return GraphicsApi.gDrawImage(this.Handle, img.Handle, x, y, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero)
				.IsOk();
		}

		#region Draw Geometries
		
		internal void DrawRectangleInternal(float x1, float y1, float x2, float y2)
		{
			TryDrawRectangleInternal(x1: x1,  y1: y1,  x2: x2,  y2: y2);
		}
		
		internal bool TryDrawRectangleInternal(float x1, float y1, float x2, float y2)
		{
			return GraphicsApi.gRectangle(this.Handle, x1, y1, x2, y2)
				.IsOk();
		}

		internal void DrawLineInternal(float x1, float y1, float x2, float y2)
		{
			TryDrawLineInternal(x1: x1,  y1: y1,  x2: x2,  y2: y2);
		}

		internal bool TryDrawLineInternal(float x1, float y1, float x2, float y2)
		{
			return GraphicsApi.gLine(this.Handle, x1, y1, x2, y2)
				.IsOk();
		}

		internal void DrawPolygonInternal(IList<PolygonPoint> points)
		{
			TryDrawPolygonInternal(points: points);
		}

		internal bool TryDrawPolygonInternal(IList<PolygonPoint> points)
		{
			var pointList = new List<float>();
			
			foreach(var point in points)
				pointList.AddRange(point.Value);

			return GraphicsApi.gPolygon(this.Handle, pointList.ToArray(), Convert.ToUInt32(points.Count()))
				.IsOk();
		}

		internal void DrawPolylineInternal(IList<PolylinePoint> points)
		{
			TryDrawPolylineInternal(points: points);
		}

		internal bool TryDrawPolylineInternal(IList<PolylinePoint> points)
		{
			var pointList = new List<float>();
			
			foreach(var point in points)
				pointList.AddRange(point.Value);
			
			return GraphicsApi.gPolyline(this.Handle, pointList.ToArray(), Convert.ToUInt32(points.Count))
				.IsOk();
		}

		internal void DrawEllipseInternal(float x, float y, float rx, float ry)
		{
			TryDrawEllipseInternal(x: x, y: y, rx: rx, ry: ry);
		}

		internal bool TryDrawEllipseInternal(float x, float y, float rx, float ry)
		{
			return GraphicsApi.gEllipse(this.Handle, x, y, rx, ry)
				.IsOk();
		}
		
		#endregion

		#region Drawing Attributes
		
		public float LineWidth
		{
			set => SetLineWidthInternal(value);
		}
		
		internal void SetLineWidthInternal(float lineWidth)
		{
			TrySetLineWidthInternal(lineWidth: lineWidth);
		}
		
		internal bool TrySetLineWidthInternal(float lineWidth)
		{
			return GraphicsApi.gLineWidth(this.Handle, lineWidth)
				.IsOk();
		}

		public LineJoinType LineJoin
		{
			set => SetLineJoinInternal(value);
		}
		
		internal void SetLineJoinInternal(LineJoinType joinType)
		{
			TrySetLineJoinInternal(joinType: joinType);
		}
		
		internal bool TrySetLineJoinInternal(LineJoinType joinType)
		{
			return GraphicsApi.gLineJoin(this.Handle, (Interop.SciterGraphics.SCITER_LINE_JOIN_TYPE)(int)joinType)
				.IsOk();
		}

		public LineCapType LineCap
		{
			set => SetLineCapInternal(value);
		}

		internal void SetLineCapInternal(LineCapType capType)
		{
			TrySetLineCapInternal(capType: capType);
		}
		
		internal bool TrySetLineCapInternal(LineCapType capType)
		{
			return GraphicsApi.gLineCap(this.Handle, (Interop.SciterGraphics.SCITER_LINE_CAP_TYPE)(int)capType)
				.IsOk();
		}
		
		public SciterColor LineColor
		{
			set => SetLineColorInternal(value);
		}

		internal void SetLineColorInternal(SciterColor lineColor)
		{
			TrySetLineColorInternal(lineColor: lineColor);
		}
		
		internal bool TrySetLineColorInternal(SciterColor lineColor)
		{
			return GraphicsApi.gLineColor(this.Handle, lineColor.Value)
				.IsOk();
		}
		
		public SciterColor FillColor
		{
			set => SetFillColorInternal(value);
		}
		
		internal void SetFillColorInternal(SciterColor fillColor)
		{
			TrySetFillColorInternal(fillColor: fillColor);
		}
		
		internal bool TrySetFillColorInternal(SciterColor fillColor)
		{
			return GraphicsApi.gFillColor(this.Handle, fillColor.Value)
				.IsOk();
		}
		
		#endregion

		#region Path Operations
		
		internal void DrawPathInternal(SciterPath path, DrawPathMode pathMode)
		{
			TryDrawPathInternal(path: path, pathMode: pathMode);
		}
		
		internal bool TryDrawPathInternal(SciterPath path, DrawPathMode pathMode)
		{
			return GraphicsApi.gDrawPath(this.Handle, path.Handle, (Interop.SciterGraphics.DRAW_PATH_MODE)(int)pathMode)
				.IsOk();
		}
		
		#endregion

		#region Affine tranformations
		
		internal void RotateInternal(float radians, float cx, float cy)
		{
			TryRotateInternal(radians: radians, cx: cx, cy: cy);
		}
		
		internal bool TryRotateInternal(float radians, float cx, float cy)
		{
			return GraphicsApi.gRotate(this.Handle, radians, ref cx, ref cy)
				.IsOk();
		}

		internal void TranslateInternal(float cx, float cy)
		{
			TryTranslateInternal(cx: cx, cy: cy);
		}

		internal bool TryTranslateInternal(float cx, float cy)
		{
			return GraphicsApi.gTranslate(this.Handle, cx, cy)
				.IsOk();
		}

		internal void ScaleInternal(float x, float y)
		{
			TryScaleInternal(x: x, y: y);
		}

		internal bool TryScaleInternal(float x, float y)
		{
			return GraphicsApi.gScale(this.Handle, x, y)
				.IsOk();
		}

		internal void SkewInternal(float dx, float dy)
		{
			TrySkewInternal(dx: dx, dy: dy);
		}

		internal bool TrySkewInternal(float dx, float dy)
		{
			return GraphicsApi.gSkew(this.Handle, dx, dy)
				.IsOk();
		}
		
		#endregion

		#region Text
		
		internal void DrawTextInternal(SciterText text, float px, float py, uint position)
		{
			TryDrawTextInternal(text: text, px: px, py: py, position: position);
		}
		
		internal bool TryDrawTextInternal(SciterText text, float px, float py, uint position)
		{
			return GraphicsApi.gDrawText(this.Handle, text.Handle, px, py, position)
				.IsOk();
		}
		
		#endregion

		#region Clipping
		
		internal void PushClipBoxInternal(float x1, float y1, float x2, float y2, float opacity = 1)
		{
			TryPushClipBoxInternal(x1: x1, y1: y1, x2: x2, y2: y2, opacity: opacity);
		}
		
		internal bool TryPushClipBoxInternal(float x1, float y1, float x2, float y2, float opacity = 1)
		{
			return GraphicsApi.gPushClipBox(this.Handle, x1, y1, x2, y2, opacity)
				.IsOk();
		}

		internal void PushClipPathInternal(SciterPath path, float opacity = 1)
		{
			TryPushClipPathInternal(path: path, opacity: opacity);
		}

		internal bool TryPushClipPathInternal(SciterPath path, float opacity = 1)
		{
			return GraphicsApi.gPushClipPath(this.Handle, path.Handle, opacity)
				.IsOk();
		}

		internal void PopClipInternal()
		{
			TryPopClipInternal();
		}

		internal bool TryPopClipInternal()
		{
			return GraphicsApi.gPopClip(this.Handle)
				.IsOk();
		}
		
		#endregion

		#region State
		
		internal void SaveStateInternal()
		{
			TrySaveStateInternal();
		}
		
		internal bool TrySaveStateInternal()
		{
			return GraphicsApi.gStateSave(this.Handle)
				.IsOk();
		}

		internal void RestoreStateInternal()
		{
			TryRestoreStateInternal();
		}

		internal bool TryRestoreStateInternal()
		{
			return GraphicsApi.gStateRestore(this.Handle)
				.IsOk();
		}
		
		#endregion

		#region IDisposable
		
		private bool _disposedValue = false;

		private void Dispose(bool disposing)
		{
			if (_disposedValue) 
				return;
			
			GraphicsApi.gRelease(this.Handle);
			_disposedValue = true;
		}

		~SciterGraphics()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		
		#endregion
	}
}