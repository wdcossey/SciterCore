using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore.PlatformWrappers
{
	public static partial class SciterWindowWrapper
	{
		internal class WindowsWrapper : ISciterWindowWrapper
		{
			private static readonly ISciterApi SciterApi = Sciter.SciterApi;
			
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

			private const SciterXDef.SCITER_CREATE_WINDOW_FLAGS DefaultWindowsCreateFlags =
				DefaultCreateFlags | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY;

			public IntPtr GetWindowHandle(IntPtr handle) => handle;

			/// <summary>
			/// Creates the Sciter window and returns the native handle
			/// </summary>
			/// <param name="frame">Rectangle of the window</param>
			/// <param name="creationFlags">Flags for the window creation, defaults to SW_MAIN | SW_TITLEBAR | SW_RESIZEABLE | SW_CONTROLS</param>
			/// <param name="parent"></param>
			public void CreateWindow(SciterRectangle frame = new SciterRectangle(),
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultWindowsCreateFlags, IntPtr? parent = null)
			{
#if DEBUG
				// Force Sciter SW_ENABLE_DEBUG in Debug build.
				creationFlags |= SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG;
#endif
				var result = SciterApi.SciterCreateWindow(
					creationFlags,
					frame,
					/*WindowDelegateRegistry.Get(this)*/null,
					IntPtr.Zero,
					parent ?? IntPtr.Zero
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

/*
#if WINDOWS || NETCORE
			public SciterWindow CreateChildWindow(IntPtr hwndParent,
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS flags = SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CHILD)
			{
				if (PInvokeWindows.IsWindow(hwndParent) == false)
					throw new ArgumentException("Invalid parent window");

				PInvokeWindows.GetClientRect(hwndParent, out var frame);

#if DEBUG
				Api.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, new IntPtr(1));
#endif

#if true
				string wndclass = Api.SciterClassName();

				Handle = PInvokeWindows.CreateWindowEx(
					(int) (PInvokeWindows.WindowStyles.WS_EX_TRANSPARENT),
					wndclass,
					null,
					(int) PInvokeWindows.WindowStyles.WS_CHILD,
					0,
					0,
					frame.Right,
					frame.Bottom,
					hwndParent,
					IntPtr.Zero,
					IntPtr.Zero,
					IntPtr.Zero);

				//Hwnd = PInvokeWindows.CreateWindowEx(0, wndclass, null, (int)PInvokeWindows.WindowStyles.WS_CHILD, 0, 0, frame.Right, frame.Bottom, hwnd_parent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
				//SetSciterOption(SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, new IntPtr(1));// NO, user should opt for it
#else
				Hwnd = _api.SciterCreateWindow(flags, ref frame, _proc, IntPtr.Zero, hwnd_parent);
#endif

				if (Handle == IntPtr.Zero)
					throw new Exception("CreateChildWindow() failed");

				return this;
			}
#endif*/

			public void Destroy(IntPtr handle)
			{
				PInvokeWindows.DestroyWindow(handle);
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
			public void CenterWindow(IntPtr handle)
			{
				PInvokeWindows.GetWindowRect(handle, out var rectWindow);

				var rectWorkArea = new PInvokeUtils.RECT();
				PInvokeWindows.SystemParametersInfo(PInvokeWindows.SPI_GETWORKAREA, 0, ref rectWorkArea, 0);

				var newX = (rectWorkArea.Width - rectWindow.Width) / 2 + rectWorkArea.Left;
				var newY = (rectWorkArea.Height - rectWindow.Height) / 2 + rectWorkArea.Top;

				PInvokeWindows.MoveWindow(handle, newX, newY, rectWindow.Width, rectWindow.Height, false);
			}

			/// <summary>
			/// Cross-platform handy method to get the size of the screen
			/// </summary>
			/// <returns>SIZE measures of the screen of primary monitor</returns>
			public SciterSize GetPrimaryScreenSize()
			{
				var nScreenWidth = PInvokeWindows.GetSystemMetrics(PInvokeWindows.SystemMetric.SM_CXSCREEN);
				var nScreenHeight = PInvokeWindows.GetSystemMetrics(PInvokeWindows.SystemMetric.SM_CYSCREEN);
				return new SciterSize(nScreenWidth, nScreenHeight);
			}

			public SciterSize GetScreenSize(IntPtr handle)
			{
				var hmonitor = PInvokeWindows.MonitorFromWindow(handle, PInvokeWindows.MONITOR_DEFAULTTONEAREST);
				var mi = new PInvokeWindows.MONITORINFO()
				{
					cbSize = Marshal.SizeOf(typeof(PInvokeWindows.MONITORINFO))
				};
				PInvokeWindows.GetMonitorInfo(hmonitor, ref mi);
				return new SciterSize(mi.rcMonitor.Width, mi.rcMonitor.Height);
			}

			public SciterSize Size(IntPtr handle)
			{
				PInvokeWindows.GetWindowRect(handle, out var rectWindow);
				return new SciterSize(rectWindow.Width, rectWindow.Height);
			}

			public SciterPoint GetPosition(IntPtr handle)
			{
				PInvokeWindows.GetWindowRect(handle, out var rectWindow);
					return new SciterPoint(rectWindow.Left, rectWindow.Top);
			}

			public void SetPosition(IntPtr handle, SciterPoint point)
			{
				var size = Size(handle);
				PInvokeWindows.MoveWindow(handle, point.X, point.Y, size.Width, size.Height, false);
			}

			public void Show(IntPtr handle, bool show = true) =>
				PInvokeWindows.ShowWindow(handle,
					show ? PInvokeWindows.ShowWindowCommands.Show : PInvokeWindows.ShowWindowCommands.Hide);
			
			public void ShowModal(IntPtr handle)
			{
				Show(handle);
				SciterPlatform.RunMessageLoop(handle);
			}

			/// <summary>
			/// Close the window. Posts WM_CLOSE message on Windows.
			/// </summary>
			public void Close(IntPtr handle) =>
				PInvokeWindows.PostMessage(handle, PInvokeWindows.Win32Msg.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

			public bool GetIsVisible(IntPtr handle) => PInvokeWindows.IsWindowVisible(handle);

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

			public string GetTitle(IntPtr handle)
			{
				var unmanagedPointer = Marshal.AllocHGlobal(2048);
				var lengthPtr = PInvokeWindows.SendMessageW(handle, PInvokeWindows.Win32Msg.WM_GETTEXT,
					new IntPtr(2048), unmanagedPointer);
				var title = Marshal.PtrToStringUni(unmanagedPointer, lengthPtr.ToInt32());
				Marshal.FreeHGlobal(unmanagedPointer);
				return title;
			}
			
			public void SetTitle(IntPtr handle, string title)
			{
				var strPtr = Marshal.StringToHGlobalUni(title);
				PInvokeWindows.SendMessageW(handle, PInvokeWindows.Win32Msg.WM_SETTEXT, IntPtr.Zero, strPtr);
				Marshal.FreeHGlobal(strPtr);
			}

			/*public string Title
			{
				get => GetTitleInternal();
				private set => SetTitleInternal(value);
			}*/

			#endregion
		}
	}
}