
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;

#if NETCORE
using System.Drawing;
using System.Drawing.Imaging;
#elif WINDOWS && !WPF
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

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArrangeThisQualifier
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable RedundantLambdaParameterType
// ReSharper disable ConvertToAutoProperty

namespace SciterCore
{
    public sealed class SciterImage : IDisposable
	{
		private static readonly ISciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;

		private readonly IntPtr _imageHandle;

		public IntPtr Handle => _imageHandle;
		
		private SciterImage(IntPtr imageHandle) 
		{ 
			_imageHandle = imageHandle;
		}

		public static SciterImage Create(SciterValue sciterValue)
		{
			TryCreate(sciterImage: out var result, sciterValue: sciterValue);
			return result;
		}

		public static bool TryCreate(out SciterImage sciterImage, SciterValue sciterValue)
		{
			var v = sciterValue.ToVALUE();
			var result = GraphicsApi.ValueUnWrapImage(ref v, out var imageHandle)
				.IsOk();
			
			sciterImage = result ? new SciterImage(imageHandle: imageHandle) : default;
			return result;
		}

		public SciterValue ToSciterValue()
		{
			TryToSciterValue(sciterValue: out var result);
			return result;
		}

		public bool TryToSciterValue(out SciterValue sciterValue)
		{
			var result = GraphicsApi.ValueWrapImage(this.Handle, out var value)
				.IsOk();
			
			sciterValue = result ? new SciterValue(value: value) : default;
			return result;
		}

		public static SciterImage Create(int width, int height, bool withAlpha)
		{
			TryCreate(out var result, width: width, height: height, withAlpha: withAlpha);
			return result;
		}

		public static bool TryCreate(out SciterImage sciterImage, int width, int height, bool withAlpha)
		{
			var result = GraphicsApi.ImageCreate(out var imageHandle, System.Convert.ToUInt32(Math.Max(width, 0)), System.Convert.ToUInt32(Math.Max(height, 0)), withAlpha)
				.IsOk();
			
			sciterImage = result ? new SciterImage(imageHandle: imageHandle) : default;
			return result;
		}

		/// <summary>
		/// Loads image from PNG or JPG image buffer
		/// </summary>
		public static SciterImage Create(byte[] data)
		{
			TryCreate(out var result, data: data);
			return result;
		}

		/// <summary>
		/// Loads image from PNG or JPG image buffer
		/// </summary>
		public static bool TryCreate(out SciterImage sciterImage, byte[] data)
		{
			var result = GraphicsApi.ImageLoad(data, (uint) data.Length, out var imageHandle)
				.IsOk();
			
			sciterImage = result ? new SciterImage(imageHandle: imageHandle) : default;
			return result;
		}

		/// <summary>
		/// <para>Loads image from RAW BGRA pixmap data</para>
		/// <para>Size of pixmap data is pixmapWidth * pixmapHeight*4
		/// construct image from B[n+0],G[n+1],R[n+2],A[n+3] data</para>
		/// </summary>
		public static SciterImage Create(IntPtr data, uint width, uint height, bool withAlpha)
		{
			TryCreate(out var result, data: data, width: width, height: height, withAlpha: withAlpha);
			return result;
		}

		/// <summary>
		/// <para>Loads image from RAW BGRA pixmap data</para>
		/// <para>Size of pixmap data is pixmapWidth * pixmapHeight*4
		/// construct image from B[n+0],G[n+1],R[n+2],A[n+3] data</para>
		/// </summary>
		public static bool TryCreate(out SciterImage sciterImage, IntPtr data, uint width, uint height, bool withAlpha)
		{
			var result = GraphicsApi.ImageCreateFromPixmap(out var imageHandle, width, height, withAlpha, data)
				.IsOk();

			sciterImage = result ? new SciterImage(imageHandle: imageHandle) : default;
			return result;
		}

#if NETCORE
		public static SciterImage Create(Bitmap bmp)
		{
			var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width * 4 == data.Stride);

			var result = GraphicsApi.ImageCreateFromPixmap(out var imageHandle, (uint) bmp.Width, (uint) bmp.Height, true, data.Scan0)
				.IsOk();

			bmp.UnlockBits(data);

			return result ? new SciterImage(imageHandle: imageHandle) : default;
		}
#elif WINDOWS && !WPF
		public static SciterImage Create(Bitmap bmp)
		{
			var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width * 4 == data.Stride);

			var result = GraphicsApi.ImageCreateFromPixmap(out var imageHandle, (uint) bmp.Width, (uint) bmp.Height, true, data.Scan0)
				.IsOk();

			bmp.UnlockBits(data);

			return result ? new SciterImage(imageHandle: imageHandle) : default;
		}
#elif WINDOWS && WPF
		public static SciterImage Create(BitmapSource bmp)
		{
            WriteableBitmap bitmap = new WriteableBitmap(bmp);
            bitmap.Lock();

			//var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
			Debug.Assert(bmp.Width*4 == bitmap.BackBufferStride);

			var result = GraphicsApi.ImageCreateFromPixmap(out var imageHandle, (uint) bmp.Width, (uint) bmp.Height, true, bitmap.BackBuffer)
				.IsOk();

            bitmap.Unlock();

			return result ? new SciterImage(imageHandle: imageHandle) : default;
		}

#elif OSX && XAMARIN
		public static SciterImage Create(CGImage img)
		{
			if(img.BitsPerPixel != 32)
				throw new Exception("Unsupported BitsPerPixel");
			if(img.BitsPerComponent != 8)
				throw new Exception("Unsupported BitsPerComponent");
			if(img.BytesPerRow != img.Width * (img.BitsPerPixel/img.BitsPerComponent))
				throw new Exception("Unsupported stride");
			
			using(var data = img.DataProvider.CopyData())
			{
				var result = GraphicsApi.ImageCreateFromPixmap(out var imageHandle, (uint) img.Width, (uint) img.Height, true, data.Bytes)
					.IsOk();

				return result ? new SciterImage(imageHandle: imageHandle) : default;
			}
		}
#endif

		/// <summary>
		/// Save this image to raw/png/jpeg/WebP stream of bytes
		/// </summary>
		/// <param name="encoding">The output image type</param>
		/// <param name="quality">png: 0, jpeg/WebP: 10 - 100</param>
		internal byte[] SaveInternal(ImageEncoding encoding, uint quality = 0)
		{
			TrySaveInternal(buffer: out var result, encoding: encoding, quality: quality);
			return result;
		}
		
		/// <summary>
		/// Save this image to png/jpeg/WebP stream of bytes
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="encoding">The output image type</param>
		/// <param name="quality">png: 0, jpeg/WebP: 10 - 100</param>
		internal bool TrySaveInternal(out byte[] buffer, ImageEncoding encoding, uint quality = 0)
		{
			byte[] outBuffer = null;
			var result = GraphicsApi.ImageSave(
				himg: this.Handle, 
				pfn: (IntPtr prm, IntPtr data, uint dataLength) =>
					{
						Debug.Assert(outBuffer == null);
						var tmpBuffer = new byte[dataLength];
						Marshal.Copy(data, tmpBuffer, 0, System.Convert.ToInt32(dataLength));
						outBuffer = tmpBuffer;
						return true;
					},
				prm: IntPtr.Zero, 
				bpp: (Interop.SciterGraphics.SCITER_IMAGE_ENCODING)(int)encoding, 
				quality: quality)
				.IsOk();

			buffer = result ? outBuffer : null;
			return result;
		}

		public SciterSize Dimensions => GetDimensionsInternal();

		internal SciterSize GetDimensionsInternal()
		{
			TryGetDimensionsInternal(size: out var result);
			return result;
		}

		internal bool TryGetDimensionsInternal(out SciterSize size)
		{
			var result = GraphicsApi.ImageGetInfo(himg: this.Handle, width: out var width, height: out var height, usesAlpha: out _)
					.IsOk();
			
			size = result ? new SciterSize(width: System.Convert.ToInt32(width), height: System.Convert.ToInt32(height)) : default;
			return result;
		}

		internal void ClearInternal(SciterColor color)
		{
			TryClearInternal(color: color);
		}

		internal bool TryClearInternal(SciterColor color)
		{
			return GraphicsApi.ImageClear(this.Handle, color.Value)
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

				GraphicsApi.ImageRelease(this.Handle);
				_disposedValue = true;
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