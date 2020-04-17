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
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

namespace SciterCore
{
	public struct RGBAColor
	{
		private static Interop.SciterGraphics.SciterGraphicsApi _graphicsApi = Interop.Sciter.GraphicsApi;
		private uint _value;

		public uint Value { get { return _value; } }

		public byte R { get { return (byte) (_value & 0xFF); } }
		public byte G { get { return (byte) ((_value >> 8) & 0xFF); } }
		public byte B { get { return (byte) ((_value >> 16) & 0xFF);  } }
		public byte A { get { return (byte) ((_value >> 24) & 0xFF); } }

		public RGBAColor(int r, int g, int b, double a = 1d)
			: this (r, g, b, (int)(Math.Min(Math.Max(a, 0d), 1d) * byte.MaxValue))
		{

		}

		public RGBAColor(int r, int g, int b, int a)
		{
			_value = _graphicsApi.RGBA((uint)GetMinMaxValue(r), (uint)GetMinMaxValue(g), (uint)GetMinMaxValue(b), (uint)GetMinMaxValue(a));
		}

		public RGBAColor(uint value)
		{
			_value = value;
		}

		public static RGBAColor White = new RGBAColor(255, 255, 255);
		public static RGBAColor Black = new RGBAColor(0, 0, 0);
		public static RGBAColor Invalid = new RGBAColor(-1, -1, -1);

#if WINDOWS
		public static RGBAColor FromColor(Color color)
		{
			return new RGBAColor(color.R, color.G, color.B, color.A);
		}

		private static uint ToRGBAColor(Color color)
		{
			return _graphicsApi.RGBA(color.R, color.G, color.B, color.A);
		}
#endif

		private static int GetMinMaxValue(int value)
		{
			return (int)Math.Min(Math.Max(value, -1), byte.MaxValue);
		}
	}

	public class SciterGraphics : IDisposable
	{
		private static Interop.SciterGraphics.SciterGraphicsApi _graphicsApi = Interop.Sciter.GraphicsApi;
		public readonly IntPtr _hgfx;

		private SciterGraphics() 
		{ 
			//
		}

		public SciterGraphics(IntPtr hgfx)
			: this()
		{
			Debug.Assert(hgfx != IntPtr.Zero);
			_hgfx = hgfx;
			_graphicsApi.gAddRef(hgfx);
		}

		public static SciterGraphics FromValue(SciterValue sv)
		{
			IntPtr hgfx;
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			var r = _graphicsApi.vUnWrapGfx(ref v, out hgfx);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);

			return new SciterGraphics(hgfx);
		}

		public SciterValue ToValue()
		{
			var r = _graphicsApi.vWrapGfx(_hgfx, out var value);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			return new SciterValue(value: value);
		}

		/*
		DON'T KNOW IF IT WORKS AND IF YOU MUST CALL gAddRef()
		SO NOT SAFE
		public SciterGraphics(SciterImage img)
		{
			var r = _gapi.gCreate(img._himg, out _hgfx);
			Debug.Assert(r == SciterXGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}*/

		public void BlendImage(SciterImage img, float x = 0f, float y = 0f)
		{
			//float w, h, ix, iy, iw, ih, opacity;
			var r = _graphicsApi.gDrawImage(_hgfx, img._himg, x, y, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		#region Draw Geometries
		public void Rectangle(float x1, float y1, float x2, float y2)
		{
			var r = _graphicsApi.gRectangle(_hgfx, x1, y1, x2, y2);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Line(float x1, float y1, float x2, float y2)
		{
			var r = _graphicsApi.gLine(_hgfx, x1, y1, x2, y2);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Polygon(IList<Tuple<float, float>> points_xy)
		{
			List<float> points = new List<float>();
			foreach(var item in points_xy)
			{
				points.Add(item.Item1);
				points.Add(item.Item2);
			}
			var r = _graphicsApi.gPolygon(_hgfx, points.ToArray(), (uint)points_xy.Count);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Polyline(IList<Tuple<float, float>> points_xy)
		{
			List<float> points = new List<float>();
			foreach(var item in points_xy)
			{
				points.Add(item.Item1);
				points.Add(item.Item2);
			}
			var r = _graphicsApi.gPolyline(_hgfx, points.ToArray(), (uint)points_xy.Count);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Ellipse(float x, float y, float rx, float ry)
		{
			var r = _graphicsApi.gEllipse(_hgfx, x, y, rx, ry);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
		#endregion

		#region Drawing attributes
		public float LineWidth
		{
			set
			{
				var r = _graphicsApi.gLineWidth(_hgfx, value);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			}
		}

		public Interop.SciterGraphics.SCITER_LINE_JOIN_TYPE LineJoin
		{
			set
			{
				var r = _graphicsApi.gLineJoin(_hgfx, value);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			}
		}

		public Interop.SciterGraphics.SCITER_LINE_CAP_TYPE LineCap
		{
			set
			{
				var r = _graphicsApi.gLineCap(_hgfx, value);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			}
		}
		
		public RGBAColor LineColor
		{
			set
			{
				var r = _graphicsApi.gLineColor(_hgfx, value.Value);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			}
		}

		public RGBAColor FillColor
		{
			set
			{
				var r = _graphicsApi.gFillColor(_hgfx, value.Value);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			}
		}
		#endregion

		#region Path operations
		public void DrawPath(SciterPath path, Interop.SciterGraphics.DRAW_PATH_MODE mode)
		{
			var r = _graphicsApi.gDrawPath(_hgfx, path._hpath, mode);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
		#endregion

		#region Affine tranformations
		public void Rotate(float radians, float cx, float cy)
		{
			var r = _graphicsApi.gRotate(_hgfx, radians, ref cx, ref cy);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Translate(float cx, float cy)
		{
			var r = _graphicsApi.gTranslate(_hgfx, cx, cy);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Scale(float x, float y)
		{
			var r = _graphicsApi.gScale(_hgfx, x, y);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void Skew(float dx, float dy)
		{
			var r = _graphicsApi.gSkew(_hgfx, dx, dy);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
		#endregion

		#region Text
		public void DrawText(SciterText text, float px, float py, uint position)
		{
			var r = _graphicsApi.gDrawText(_hgfx, text._htext, px, py, position);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
		#endregion

		#region Clipping
		public void PushClipBox(float x1, float y1, float x2, float y2, float opacity = 1)
		{
			var r = _graphicsApi.gPushClipBox(_hgfx, x1, y1, x2, y2, opacity);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void PushClipPath(SciterPath path, float opacity = 1)
		{
			var r = _graphicsApi.gPushClipPath(_hgfx, path._hpath, opacity);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void PopClip()
		{
			var r = _graphicsApi.gPopClip(_hgfx);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
		#endregion

		#region State save/restore
		public void StateSave()
		{
			var r = _graphicsApi.gStateSave(_hgfx);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void StateRestore()
		{
			var r = _graphicsApi.gStateRestore(_hgfx);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
		#endregion

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				_graphicsApi.gRelease(_hgfx);

				disposedValue = true;
			}
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

	public class SciterImage : IDisposable
	{
		private static Interop.SciterGraphics.SciterGraphicsApi _gapi = Interop.Sciter.GraphicsApi;
		public IntPtr _himg { get; private set; }

		private SciterImage() 
		{ 
			// non-user usable
		}

		public SciterImage(SciterValue sv)
			: this()
		{
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			IntPtr himg;
			var r = _gapi.vUnWrapImage(ref v, out himg);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_himg = himg;
		}

		public SciterValue ToSV()
		{
			Interop.SciterValue.VALUE v;
			var r = _gapi.vWrapImage(_himg, out v);
			return new SciterValue(v);
		}

		public SciterImage(uint width, uint height, bool withAlpha)
		{
			IntPtr himg;
			var r = _gapi.imageCreate(out himg, width, height, withAlpha);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_himg = himg;
		}

		/// <summary>
		/// Loads image from PNG or JPG image buffer
		/// </summary>
		public SciterImage(byte[] data)
		{
			IntPtr himg;
			var r = _gapi.imageLoad(data, (uint) data.Length, out himg);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_himg = himg;
		}

		/// <summary>
		/// Loads image from RAW BGRA pixmap data
		/// Size of pixmap data is pixmapWidth*pixmapHeight*4
		/// construct image from B[n+0],G[n+1],R[n+2],A[n+3] data
		/// </summary>
		public SciterImage(IntPtr data, uint width, uint height, bool withAlpha)
		{
			IntPtr himg;
			var r = _gapi.imageCreateFromPixmap(out himg, width, height, withAlpha, data);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_himg = himg;
		}

#if WINDOWS && !WPF
		public SciterImage(Bitmap bmp)
		{
			var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width*4 == data.Stride);

			IntPtr himg;
			var r = _gapi.imageCreateFromPixmap(out himg, (uint) bmp.Width, (uint) bmp.Height, true, data.Scan0);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_himg = himg;

			bmp.UnlockBits(data);
		}
#elif WINDOWS && WPF
		public SciterImage(BitmapSource bmp)
		{
            WriteableBitmap bitmap = new WriteableBitmap(bmp);
            bitmap.Lock();

			//var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width*4 == bitmap.BackBufferStride);
            
			IntPtr himg;
			var r = _gapi.imageCreateFromPixmap(out himg, (uint) bmp.Width, (uint) bmp.Height, true, bitmap.BackBuffer);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_himg = himg;

            bitmap.Unlock();
		}

#elif OSX && XAMARIN
		public SciterImage(CGImage img)
		{
			if(img.BitsPerPixel != 32)
				throw new Exception("Unsupported BitsPerPixel");
			if(img.BitsPerComponent != 8)
				throw new Exception("Unsupported BitsPerComponent");
			if(img.BytesPerRow != img.Width * (img.BitsPerPixel/img.BitsPerComponent))
				throw new Exception("Unsupported stride");
			
			using(var data = img.DataProvider.CopyData())
			{
				IntPtr himg;
				var r = _gapi.imageCreateFromPixmap(out himg, (uint) img.Width, (uint) img.Height, true, data.Bytes);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
				_himg = himg;
			}
		}
#endif

		/// <summary>
		/// Save this image to png/jpeg/WebP stream of bytes
		/// </summary>
		/// <param name="encoding">The output image type</param>
		/// <param name="quality">png: 0, jpeg/WebP: 10 - 100</param>
		public byte[] Save(Interop.SciterGraphics.SCITER_IMAGE_ENCODING encoding, uint quality = 0)
		{
			byte[] ret = null;
			Interop.SciterGraphics.SciterGraphicsApi.image_write_function _proc = (IntPtr prm, IntPtr data, uint data_length) =>
			{
				Debug.Assert(ret == null);
				byte[] buffer = new byte[data_length];
				Marshal.Copy(data, buffer, 0, (int) data_length);
				ret = buffer;
				return true;
			};
			_gapi.imageSave(_himg, _proc, IntPtr.Zero, encoding, quality);
			return ret;
		}

		public Interop.PInvokeUtils.SIZE Dimension
		{
			get
			{
				uint width, height;
				bool usesAlpha;
				var r = _gapi.imageGetInfo(_himg, out width, out height, out usesAlpha);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
				return new Interop.PInvokeUtils.SIZE() { cx = (int)width, cy = (int)height };
			}
		}

		public void Clear(RGBAColor color)
		{
			var r = _gapi.imageClear(_himg, color.Value);
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

				_gapi.imageRelease(_himg);
				disposedValue = true;
			}
		}

		~SciterImage()
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

	public class SciterPath : IDisposable
	{
		private static Interop.SciterGraphics.SciterGraphicsApi _gapi = Interop.Sciter.GraphicsApi;
		public IntPtr _hpath { get; private set; }

		private SciterPath() 
		{ 
			// non-user usable
		}

		public static SciterPath Create()
		{
			IntPtr hpath;
			var r = _gapi.pathCreate(out hpath);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			Debug.Assert(hpath != IntPtr.Zero);

			SciterPath st = new SciterPath();
			st._hpath = hpath;
			return st;
		}

		public static SciterPath FromSV(SciterValue sv)
		{
			IntPtr hpath;
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			var r = _gapi.vUnWrapPath(ref v, out hpath);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);

			SciterPath st = new SciterPath();
			st._hpath = hpath;

			return st;
		}

		public SciterValue ToSV()
		{
			Interop.SciterValue.VALUE v;
			var r = _gapi.vWrapPath(_hpath, out v);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			return new SciterValue(v);
		}

		public void MoveTo(float x, float y, bool relative = false)
		{
			var r = _gapi.pathMoveTo(_hpath, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void LineTo(float x, float y, bool relative = false)
		{
			var r = _gapi.pathLineTo(_hpath, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void ArcTo(float x, float y, float angle, float rx, float ry, bool is_large_arc, bool clockwise, bool relative = false)
		{
			var r = _gapi.pathArcTo(_hpath, x, y, angle, rx, ry, is_large_arc, clockwise, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void QuadraticCurveTo(float xc, float yc, float x, float y, bool relative = false)
		{
			var r = _gapi.pathQuadraticCurveTo(_hpath, xc, yc, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void BezierCurveTo(float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative = false)
		{
			var r = _gapi.pathBezierCurveTo(_hpath, xc1, yc1, xc2, yc2, x, y, relative);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}

		public void ClosePath()
		{
			var r = _gapi.pathClosePath(_hpath);
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

				_gapi.pathRelease(_hpath);
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

	public class SciterText
	{
		private static Interop.SciterGraphics.SciterGraphicsApi _gapi = Interop.Sciter.GraphicsApi;
		public IntPtr _htext { get; private set; }

		private SciterText() { }// non-user usable

		public static SciterText Create(string text, IntPtr he, string className = null)
		{
			IntPtr htext;
			var r = _gapi.textCreateForElement(out htext, text, (uint) text.Length, he, className);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			Debug.Assert(htext != IntPtr.Zero);

			SciterText st = new SciterText();
			st._htext = htext;
			return st;
		}

		/// <summary>
		/// create text layout using explicit style declaration
		/// </summary>
		/// <param name="text"></param>
		/// <param name="he"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public static SciterText CreateWithStyle(string text, IntPtr he, string style)
		{
			IntPtr htext;
			var r = _gapi.textCreateForElementAndStyle(out htext, text, (uint)text.Length, he, style, (uint) style.Length);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			Debug.Assert(htext != IntPtr.Zero);

			SciterText st = new SciterText();
			st._htext = htext;
			return st;
		}


		public static SciterText FromSV(SciterValue sv)
		{
			IntPtr htext;
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			var r = _gapi.vUnWrapText(ref v, out htext);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);

			SciterText st = new SciterText();
			st._htext = htext;

			return st;
		}

		public SciterValue ToSV()
		{
			Interop.SciterValue.VALUE v;
			var r = _gapi.vWrapText(_htext, out v);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			return new SciterValue(v);
		}

		public class TextMetrics
		{
			public float minWidth;
			public float maxWidth;
			public float height;
			public float ascent;
			public float descent;
			public uint nLines;
		}

		public TextMetrics Metrics
		{
			get
			{
				var m = new TextMetrics();
				var r = _gapi.textGetMetrics(_htext, out m.minWidth, out m.maxWidth, out m.height, out m.ascent, out m.descent, out m.nLines);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
				return m;
			}
		}

		public void SetBox(float width, float height)
		{
			var r = _gapi.textSetBox(_htext, width, height);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
		}
	}
}