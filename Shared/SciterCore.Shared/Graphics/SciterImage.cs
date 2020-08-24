using System;
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
    public class SciterImage : IDisposable
	{
		private static readonly Interop.SciterGraphics.SciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;

		private readonly IntPtr _imageHandle;

		public IntPtr Handle => _imageHandle;
		
		private SciterImage() 
		{ 
			// non-user usable
		}

		public SciterImage(SciterValue sv)
			: this()
		{
			Interop.SciterValue.VALUE v = sv.ToVALUE();
			var r = GraphicsApi.vUnWrapImage(ref v, out var imageHandle);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_imageHandle = imageHandle;
		}

		public SciterValue ToSV()
		{
			Interop.SciterValue.VALUE v;
			var r = GraphicsApi.vWrapImage(this.Handle, out v);
			return new SciterValue(v);
		}

		public SciterImage(uint width, uint height, bool withAlpha)
		{
			var r = GraphicsApi.imageCreate(out var himg, width, height, withAlpha);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_imageHandle = himg;
		}

		/// <summary>
		/// Loads image from PNG or JPG image buffer
		/// </summary>
		public SciterImage(byte[] data)
		{
			var r = GraphicsApi.imageLoad(data, (uint) data.Length, out var imageHandle);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_imageHandle = imageHandle;
		}

		/// <summary>
		/// Loads image from RAW BGRA pixmap data
		/// Size of pixmap data is pixmapWidth*pixmapHeight*4
		/// construct image from B[n+0],G[n+1],R[n+2],A[n+3] data
		/// </summary>
		public SciterImage(IntPtr data, uint width, uint height, bool withAlpha)
		{
			var r = GraphicsApi.imageCreateFromPixmap(out var imageHandle, width, height, withAlpha, data);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_imageHandle = imageHandle;
		}

#if WINDOWS && !WPF
		public SciterImage(Bitmap bmp)
		{
			var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width*4 == data.Stride);

			var r = GraphicsApi.imageCreateFromPixmap(out var imageHandle, (uint) bmp.Width, (uint) bmp.Height, true, data.Scan0);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_imageHandle = imageHandle;

			bmp.UnlockBits(data);
		}
#elif WINDOWS && WPF
		public SciterImage(BitmapSource bmp)
		{
            WriteableBitmap bitmap = new WriteableBitmap(bmp);
            bitmap.Lock();

			//var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width*4 == bitmap.BackBufferStride);

			var r = GraphicsApi.imageCreateFromPixmap(out var imageHandle, (uint) bmp.Width, (uint) bmp.Height, true, bitmap.BackBuffer);
			Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
			_imageHandle = imageHandle;

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
				var r = GraphicsApi.imageCreateFromPixmap(out var imageHandle, (uint) img.Width, (uint) img.Height, true, data.Bytes);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
				_imageHandle = imageHandle;
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
			GraphicsApi.imageSave(this.Handle, _proc, IntPtr.Zero, encoding, quality);
			return ret;
		}

		public Interop.PInvokeUtils.SIZE Dimension
		{
			get
			{
				uint width, height;
				bool usesAlpha;
				var r = GraphicsApi.imageGetInfo(this.Handle, out width, out height, out usesAlpha);
				Debug.Assert(r == Interop.SciterGraphics.GRAPHIN_RESULT.GRAPHIN_OK);
				return new Interop.PInvokeUtils.SIZE() { cx = (int)width, cy = (int)height };
			}
		}

		public void Clear(RGBAColor color)
		{
			var r = GraphicsApi.imageClear(this.Handle, color.Value);
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

				GraphicsApi.imageRelease(this.Handle);
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
}