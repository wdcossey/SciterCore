using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SciterCore.Interop
{
	public static class PInvokeUtils
	{
		// PInvoke marshaling utils ===============================================================
		public static IntPtr NativeUtf16FromString(string managedString, int minlen)
		{
			// Marshal.StringToHGlobalUni() -- does not gives the buffer size
			byte[] strbuffer = Encoding.Unicode.GetBytes(managedString);

			minlen = Math.Max(strbuffer.Length, minlen);
			byte[] zerobuffer = new byte[minlen];
			Buffer.BlockCopy(strbuffer, 0, zerobuffer, 0, strbuffer.Length);

			IntPtr nativeUtf16 = Marshal.AllocHGlobal(minlen);
			Marshal.Copy(zerobuffer, 0, nativeUtf16, minlen);
			return nativeUtf16;
		}
		public static void NativeUtf16FromString_FreeBuffer(IntPtr buffer)
		{
			Marshal.FreeHGlobal(buffer);
		}

		public static string StringFromNativeUtf16(IntPtr nativeUtf16)
		{
			return Marshal.PtrToStringUni(nativeUtf16);
		}

		// PInvoke structs ===============================================================
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
                : this(right: right, bottom: bottom)
            {
                Left = left;
                Top = top;
            }

            public RECT(int right, int bottom)
            {
                Left = 0;
                Top = 0;
                Right = right;
                Bottom = bottom;
            }

            public int Left, Top, Right, Bottom;

            public int Width => Right - Left;

            public int Height => Bottom - Top;
        }

        [StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				X = x;
				Y = y;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SIZE
		{
			public int cx;
			public int cy;

			public SIZE(int x, int y)
			{
				cx = x;
				cy = y;
			}
		}
	}
}