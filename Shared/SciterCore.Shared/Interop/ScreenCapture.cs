#if WINDOWS
using System;
using System.Runtime.InteropServices;
#if !WPF
using System.Drawing;
using System.Drawing.Imaging;
#else
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace SciterCore.Interop
{
	/// <summary>
	/// Provides functions to capture the entire screen, or a particular window, and save it to a file.
	/// </summary>
	public class ScreenCapture
	{
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
#if !WPF 
        public Image CaptureScreen() 
#else
        public BitmapSource CaptureScreen() 
#endif
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
#if !WPF 
        public Image CaptureWindow(IntPtr handle) 
#else
        public BitmapSource CaptureWindow(IntPtr handle)
#endif
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle,ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,width,height); 
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest,0,0,width,height,hdcSrc,0,0,GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest,hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle,hdcSrc);

            // get a .NET image object for it
#if !WPF 
            Image result = Image.FromHbitmap(hBitmap);
#else
            BitmapSource result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
#endif

            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            return result;
        }

#if !WPF 
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, string filePath, ImageFormat format) 
        {
            Image img = CaptureWindow(handle);
            img.Save(filePath, format);
        }
#else
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filePath"></param>
        public void CaptureWindowToFile(IntPtr handle, string filePath)
        {
            BitmapSource source = CaptureWindow(handle);
            SaveToFile(filePath: filePath, source: source);
        }
#endif

#if !WPF
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filePath, ImageFormat format) 
        {
            Image img = CaptureScreen();
            img.Save(filePath,format);
        }
#else
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filePath"></param>
        public void CaptureScreenToFile(string filePath) 
        {
            BitmapSource source = CaptureScreen();
            SaveToFile(filePath: filePath, source: source);
        }
#endif

#if WPF
        private void SaveToFile(string filePath, BitmapSource source) 
        {
            BitmapSource img = CaptureScreen();
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source: source));
                encoder.Save(stream: fileStream);
            }
        }
#endif

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject,int nXDest,int nYDest,
                int nWidth,int nHeight,IntPtr hObjectSource,
                int nXSrc,int nYSrc,int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC,int nWidth, 
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC,IntPtr hObject);
        }
 
        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd,IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd,ref RECT rect);

        }
	}
}
#endif