using System;
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
	public static class PInvokeGtk
	{
		private const string LibGtkLibrary = "libgtk-3.so.0";
			
		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_init(IntPtr argc, IntPtr argv);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_main();

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr gtk_widget_get_toplevel(IntPtr widget);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_window_set_title(IntPtr window, [MarshalAs(UnmanagedType.LPStr)]string title);

        [DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr gtk_window_get_title(IntPtr window);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_window_present(IntPtr window);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_widget_hide(IntPtr window);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_window_close(IntPtr window);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_window_get_size(IntPtr window, out int width, out int height);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gdk_screen_width();

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gdk_screen_height();

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gtk_window_move(IntPtr window, int x, int y);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern void gtk_widget_destroy(IntPtr widget);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gtk_widget_get_visible(IntPtr widget);

		[DllImport(LibGtkLibrary, CallingConvention = CallingConvention.Cdecl)]
		public static extern int gtk_window_set_icon_from_file(IntPtr window, [MarshalAs(UnmanagedType.LPStr)]string title, IntPtr err);
	}
}