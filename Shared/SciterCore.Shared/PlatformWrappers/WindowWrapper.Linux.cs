using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore.PlatformWrappers
{
	public static partial class SciterWindowWrapper
	{
		internal class LinuxWrapper : ISciterWindowWrapper
		{
			private static readonly ISciterApi SciterApi = Sciter.Api;
			
			//private IntPtr _handle;
			
			//public IntPtr Handle
			//{
			//	get => _handle;
			//	protected set => _handle = value;
			//}
			
			/*public bool SetSciterOption(SciterXDef.SCITER_RT_OPTIONS option, IntPtr value)
			{
				Debug.Assert(Handle != IntPtr.Zero);
				return Api.SciterSetOption(Handle, option, value);
			}*/

			/*public SciterWindow()
			{
	
				var allow = SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_EVAL |
							SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_FILE_IO |
							SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SOCKET_IO |
							SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SYSINFO;
	
				Api.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_SCRIPT_RUNTIME_FEATURES, new IntPtr((int)allow));
	
	#if WINDOWS || NETCORE
				WindowDelegateRegistry.Set(this, InternalProcessSciterWindowMessage);
	#endif
			}*/

			/*public SciterWindow(IntPtr hwnd, bool weakReference = false)
			{
				Handle = hwnd;
	
				if (!weakReference)
				{
	
	#if WINDOWS || NETCORE
					WindowDelegateRegistry.Set(this, InternalProcessSciterWindowMessage);
	#endif
	
	#if GTKMONO
					_gtkwindow = PInvokeGtk.gtk_widget_get_toplevel(Handle);
					Debug.Assert(_gtkwindow != IntPtr.Zero);
	#elif OSX && XAMARIN
					_nsview = new OSXView(Handle);
	#endif
				}
			}*/

			private const SciterXDef.SCITER_CREATE_WINDOW_FLAGS DefaultCreateFlags =
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS;

			/// <summary>
			/// Creates the Sciter window and returns the native handle
			/// </summary>
			/// <param name="frame">Rectangle of the window</param>
			/// <param name="creationFlags">Flags for the window creation, defaults to SW_MAIN | SW_TITLEBAR | SW_RESIZEABLE | SW_CONTROLS</param>
			/// <param name="parent"></param>
			public void CreateWindow(PInvokeUtils.RECT frame = new PInvokeUtils.RECT(),
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags, IntPtr parent = new IntPtr())
			{
#if DEBUG
				// Force Sciter SW_ENABLE_DEBUG in Debug build.
				creationFlags |= SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG;
#endif
				var result = SciterApi.SciterCreateWindow(
					creationFlags,
					ref frame,
					null,
					IntPtr.Zero,
					parent
				);
				
				Debug.Assert(result != IntPtr.Zero);

				if (result == IntPtr.Zero)
					throw new Exception("CreateWindow() failed");
			}

			/*public SciterWindow CreateMainWindow(int width, int height,
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
			{
				var frame = new PInvokeUtils.RECT(width, height);
				return CreateWindow(frame, creationFlags);
			}

			public SciterWindow CreateOwnedWindow(IntPtr owner, int width, int height,
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
			{
				var frame = new PInvokeUtils.RECT(width, height);
				return CreateWindow(frame, creationFlags, owner);
			}*/

			/*
			/// <summary>
			/// Create an owned top-level Sciter window
			/// </summary>
			/// <param name="width"></param>
			/// <param name="height"></param>
			/// <param name="owner_hwnd"></param>
			public void CreatePopupAlphaWindow(int width, int height, IntPtr owner_hwnd)
			{
				PInvokeUtils.RECT frame = new PInvokeUtils.RECT();
				frame.right = width;
				frame.bottom = height;
				CreateWindow(frame, SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TOOL, owner_hwnd);
				// Sciter BUG: window comes with WM_EX_APPWINDOW style
			}*/
			

			public void Destroy(IntPtr window)
			{
				PInvokeGtk.gtk_widget_destroy(window);
			}

			/*
#if WINDOWS || NETCORE
			public bool ModifyStyle(PInvokeWindows.WindowStyles dwRemove, PInvokeWindows.WindowStyles dwAdd)
			{
				int GWL_EXSTYLE = -20;

				PInvokeWindows.WindowStyles dwStyle =
					(PInvokeWindows.WindowStyles) PInvokeWindows.GetWindowLongPtr(Handle, GWL_EXSTYLE);
				PInvokeWindows.WindowStyles dwNewStyle = (dwStyle & ~dwRemove) | dwAdd;

				if (dwStyle == dwNewStyle)
					return false;

				PInvokeWindows.SetWindowLongPtr(Handle, GWL_EXSTYLE, (IntPtr) dwNewStyle);
				return true;
			}

			public bool ModifyStyleEx(PInvokeWindows.WindowStyles dwRemove, PInvokeWindows.WindowStyles dwAdd)
			{
				int GWL_STYLE = -16;

				PInvokeWindows.WindowStyles dwStyle =
					(PInvokeWindows.WindowStyles) PInvokeWindows.GetWindowLongPtr(Handle, GWL_STYLE);
				PInvokeWindows.WindowStyles dwNewStyle = (dwStyle & ~dwRemove) | dwAdd;
				if (dwStyle == dwNewStyle)
					return false;

				PInvokeWindows.SetWindowLongPtr(Handle, GWL_STYLE, (IntPtr) dwNewStyle);
				return true;
			}
#endif*/

			/// <summary>
			/// Centers the window in the screen. You must call it after the window is created, but before it is shown to avoid flickering
			/// </summary>
			public void CenterWindow(IntPtr window)
			{
				var screenWidth = PInvokeGtk.gdk_screen_width();
				var screenHeight = PInvokeGtk.gdk_screen_height();

				PInvokeGtk.gtk_window_get_size(window, out var windowWidth, out var windowHeight);

				var newX = (screenWidth - windowWidth) / 2;
				var newY = (screenHeight - windowHeight) / 2;

				PInvokeGtk.gtk_window_move(window, newX, newY);
			}

			/// <summary>
			/// Cross-platform handy method to get the size of the screen
			/// </summary>
			/// <returns>SIZE measures of the screen of primary monitor</returns>
			public SciterSize GetPrimaryScreenSize()
			{
				var screenWidth = PInvokeGtk.gdk_screen_width();
				var screenHeight = PInvokeGtk.gdk_screen_height();
				return new SciterSize(screenWidth, screenHeight);
			}

			// TODO: Does this exist in GTK?
			public SciterSize GetScreenSize(IntPtr window) =>
				SciterSize.Empty;
			
			public SciterSize Size(IntPtr window)
			{
				PInvokeGtk.gtk_window_get_size(window, out var windowWidth, out var windowHeight);
				return new SciterSize(windowWidth, windowHeight);
			}

			// TODO: Does this exist in GTK?
			public SciterPoint GetPosition(IntPtr window) =>
				SciterPoint.Empty;
			

			public void SetPosition(IntPtr window, SciterPoint point) =>
				PInvokeGtk.gtk_window_move(window, point.X, point.Y);

			public void Show(IntPtr window, bool show = true)
			{
				if(show)
					PInvokeGtk.gtk_window_present(window);
				else
					PInvokeGtk.gtk_widget_hide(window);
			}
			
			public void ShowModal(IntPtr window)
			{
				Show(window);
				SciterPlatform.RunMessageLoop(window);
			}

			/// <summary>
			/// Close the window. Posts WM_CLOSE message on Windows.
			/// </summary>
			public void Close(IntPtr window) =>
				PInvokeGtk.gtk_window_close(window);

			public bool GetIsVisible(IntPtr window) =>
				PInvokeGtk.gtk_widget_get_visible(window) != 0;

/*#if WINDOWS && !WPF
			public SciterWindow SetIcon(Icon icon)
			{
				// instead of using this property, you can use View.windowIcon on all platforms
				// larger icon
				PInvokeWindows.SendMessageW(Handle, PInvokeWindows.Win32Msg.WM_SETICON, new IntPtr(1), icon.Handle);
				// small icon
				PInvokeWindows.SendMessageW(Handle, PInvokeWindows.Win32Msg.WM_SETICON, IntPtr.Zero,
					new Icon(icon, 16, 16).Handle);
				return this;
			}
#endif*/

			#region Title

			public string GetTitle(IntPtr window)
			{
				var titlePtr = PInvokeGtk.gtk_window_get_title(window);
				return Marshal.PtrToStringAnsi(titlePtr);
			}
			
			public void SetTitle(IntPtr window, string title) =>
				PInvokeGtk.gtk_window_set_title(window, title);
			

			#endregion
		}
	}
}