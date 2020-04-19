﻿// Copyright 2016 Ramon F. Mendes
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;
#if OSX && XAMARIN
using AppKit;
using Foundation;
#else
using System.Drawing;
#endif

namespace SciterCore
{
#if OSX && XAMARIN
	public class OSXView : NSView
	{
		public OSXView(IntPtr handle)
			: base(handle)
		{
		}
	}
#endif

	public class SciterWindow
#if WINDOWS && !WPF
		: System.Windows.Forms.IWin32Window
#endif
	{
		protected static Sciter.SciterApi _api = Sciter.Api;

		private IntPtr _handle;

		public IntPtr Handle
		{
			get => _handle;
			protected set => _handle = value;
		}

		private SciterXDef.SCITER_WINDOW_DELEGATE _proc;
#if GTKMONO || NETCORE
		public IntPtr _gtkwindow { get; private set; }
#elif OSX && XAMARIN
		public NSView _nsview { get; private set; }
#endif

		public static implicit operator IntPtr(SciterWindow wnd)
		{
			return wnd.Handle;
		}

		public bool SetSciterOption(SciterXDef.SCITER_RT_OPTIONS option, IntPtr value)
		{
			Debug.Assert(Handle != IntPtr.Zero);
			return _api.SciterSetOption(Handle, option, value);
		}

		public SciterWindow()
		{

			var allow = SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_EVAL |
						SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_FILE_IO |
						SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SOCKET_IO |
						SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SYSINFO;

			_api.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_SCRIPT_RUNTIME_FEATURES, new IntPtr((int)allow));

#if WINDOWS || NETCORE
			_proc = InternalProcessSciterWindowMessage;
#else
			_proc = null;
#endif
		}

		public SciterWindow(IntPtr hwnd)
		{
			Handle = hwnd;

#if WINDOWS || NETCORE
			_proc = InternalProcessSciterWindowMessage;
#else
			_proc = null;
#endif

#if GTKMONO
			_gtkwindow = PInvokeGTK.gtk_widget_get_toplevel(Handle);
			Debug.Assert(_gtkwindow != IntPtr.Zero);
#elif OSX && XAMARIN
			_nsview = new OSXView(Handle);
#endif
		}

		public const SciterXDef.SCITER_CREATE_WINDOW_FLAGS DefaultCreateFlags =
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY;

		/// <summary>
		/// Creates the Sciter window and returns the native handle
		/// </summary>
		/// <param name="frame">Rectangle of the window</param>
		/// <param name="creationFlags">Flags for the window creation, defaults to SW_MAIN | SW_TITLEBAR | SW_RESIZEABLE | SW_CONTROLS</param>
		public SciterWindow CreateWindow(PInvokeUtils.RECT frame = new PInvokeUtils.RECT(), SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags, IntPtr parent = new IntPtr())
		{

#if DEBUG
			// Force Sciter SW_ENABLE_DEBUG in Debug build.
			creationFlags |= SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG;
#endif
			Debug.Assert(Handle == IntPtr.Zero);
			Handle = _api.SciterCreateWindow(
				creationFlags,
				ref frame,
				_proc,
				IntPtr.Zero,
				parent
			);
			Debug.Assert(Handle != IntPtr.Zero);

			if(Handle == IntPtr.Zero)
				throw new Exception("CreateWindow() failed");

#if GTKMONO
			_gtkwindow = PInvokeGTK.gtk_widget_get_toplevel(Handle);
			Debug.Assert(_gtkwindow != IntPtr.Zero);
#elif OSX && XAMARIN
			_nsview = new OSXView(Handle);
#endif
			return this;
		}

		public SciterWindow CreateMainWindow(int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
		{
			PInvokeUtils.RECT frame = new PInvokeUtils.RECT(width, height);
			return CreateWindow(frame, creationFlags);
		}

		public SciterWindow CreateOwnedWindow(IntPtr owner, int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
		{
			PInvokeUtils.RECT frame = new PInvokeUtils.RECT(width, height);
            return CreateWindow(frame, creationFlags, owner);
		}

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

#if WINDOWS || NETCORE
		public SciterWindow CreateChildWindow(IntPtr hwnd_parent, SciterXDef.SCITER_CREATE_WINDOW_FLAGS flags = SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CHILD)
		{
			if(PInvokeWindows.IsWindow(hwnd_parent) == false)
				throw new ArgumentException("Invalid parent window");

			PInvokeUtils.RECT frame;
			PInvokeWindows.GetClientRect(hwnd_parent, out frame);

#if true
            string wndclass = Marshal.PtrToStringUni(_api.SciterClassName());

            Handle = PInvokeWindows.CreateWindowEx(
				(int)(PInvokeWindows.WindowStyles.WS_EX_TRANSPARENT),
                wndclass,
				null,
                (int)PInvokeWindows.WindowStyles.WS_CHILD,
                0, 
                0, 
                frame.Right, 
                frame.Bottom,
                hwnd_parent,
				IntPtr.Zero,
				IntPtr.Zero,
				IntPtr.Zero);

			//Hwnd = PInvokeWindows.CreateWindowEx(0, wndclass, null, (int)PInvokeWindows.WindowStyles.WS_CHILD, 0, 0, frame.Right, frame.Bottom, hwnd_parent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			//SetSciterOption(SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, new IntPtr(1));// NO, user should opt for it
#else
			Hwnd = _api.SciterCreateWindow(flags, ref frame, _proc, IntPtr.Zero, hwnd_parent);
#endif

			if(Handle == IntPtr.Zero)
				throw new Exception("CreateChildWindow() failed");

            return this;
		}
#endif

		public void Destroy()
		{
#if WINDOWS || NETCORE
			PInvokeWindows.DestroyWindow(Handle);
#elif GTKMONO
			PInvokeGTK.gtk_widget_destroy(_gtkwindow);
#endif
		}

#if WINDOWS || NETCORE
		public bool ModifyStyle(PInvokeWindows.WindowStyles dwRemove, PInvokeWindows.WindowStyles dwAdd)
		{
			int GWL_EXSTYLE = -20;

			PInvokeWindows.WindowStyles dwStyle = (PInvokeWindows.WindowStyles)PInvokeWindows.GetWindowLongPtr(Handle, GWL_EXSTYLE);
			PInvokeWindows.WindowStyles dwNewStyle = (dwStyle & ~dwRemove) | dwAdd;

			if(dwStyle == dwNewStyle)
				return false;

			PInvokeWindows.SetWindowLongPtr(Handle, GWL_EXSTYLE, (IntPtr)dwNewStyle);
			return true;
		}

		public bool ModifyStyleEx(PInvokeWindows.WindowStyles dwRemove, PInvokeWindows.WindowStyles dwAdd)
		{
			int GWL_STYLE = -16;

			PInvokeWindows.WindowStyles dwStyle = (PInvokeWindows.WindowStyles)PInvokeWindows.GetWindowLongPtr(Handle, GWL_STYLE);
			PInvokeWindows.WindowStyles dwNewStyle = (dwStyle & ~dwRemove) | dwAdd;
			if(dwStyle == dwNewStyle)
				return false;

			PInvokeWindows.SetWindowLongPtr(Handle, GWL_STYLE, (IntPtr)dwNewStyle);
			return true;
		}
#endif

		/// <summary>
		/// Centers the window in the screen. You must call it after the window is created, but before it is shown to avoid flickering
		/// </summary>
		public SciterWindow CenterTopLevelWindow()
		{
#if WINDOWS || NETCORE
			PInvokeUtils.RECT rectWindow;
			PInvokeWindows.GetWindowRect(Handle, out rectWindow);

			PInvokeUtils.RECT rectWorkArea = new PInvokeUtils.RECT();
			PInvokeWindows.SystemParametersInfo(PInvokeWindows.SPI_GETWORKAREA, 0, ref rectWorkArea, 0);
			
			int nX = (rectWorkArea.Width - rectWindow.Width) / 2 + rectWorkArea.Left;
			int nY = (rectWorkArea.Height - rectWindow.Height) / 2 + rectWorkArea.Top;
			
			PInvokeWindows.MoveWindow(Handle, nX, nY, rectWindow.Width, rectWindow.Height, false);
#elif GTKMONO
			int screen_width = PInvokeGTK.gdk_screen_width();
			int screen_height = PInvokeGTK.gdk_screen_height();

			int window_width, window_height;
			PInvokeGTK.gtk_window_get_size(_gtkwindow, out window_width, out window_height);

			int nX = (screen_width - window_width) / 2;
			int nY = (screen_height - window_height) / 2;

			PInvokeGTK.gtk_window_move(_gtkwindow, nX, nY);
#elif OSX && XAMARIN
			_nsview.Window.Center();
#endif
			return this;
		}

		/// <summary>
		/// Cross-platform handy method to get the size of the screen
		/// </summary>
		/// <returns>SIZE measures of the screen of primary monitor</returns>
		public static PInvokeUtils.SIZE GetPrimaryMonitorScreenSize()
		{
#if WINDOWS || NETCORE
			int nScreenWidth = PInvokeWindows.GetSystemMetrics(PInvokeWindows.SystemMetric.SM_CXSCREEN);
			int nScreenHeight = PInvokeWindows.GetSystemMetrics(PInvokeWindows.SystemMetric.SM_CYSCREEN);
			return new PInvokeUtils.SIZE() { cx = nScreenWidth, cy = nScreenHeight };
#elif GTKMONO
			int screen_width = PInvokeGTK.gdk_screen_width();
			int screen_height = PInvokeGTK.gdk_screen_height();
			return new PInvokeUtils.SIZE() { cx = screen_width, cy = screen_height };
#elif OSX && XAMARIN
			var sz = NSScreen.MainScreen.Frame.Size;
			return new PInvokeUtils.SIZE((int)sz.Width, (int)sz.Height);
#endif
		}

		public PInvokeUtils.SIZE ScreenSize
		{
			get
			{
#if WINDOWS || NETCORE
				IntPtr hmonitor = PInvokeWindows.MonitorFromWindow(Handle, PInvokeWindows.MONITOR_DEFAULTTONEAREST);
				PInvokeWindows.MONITORINFO mi = new PInvokeWindows.MONITORINFO() { cbSize = Marshal.SizeOf(typeof(PInvokeWindows.MONITORINFO)) };
				PInvokeWindows.GetMonitorInfo(hmonitor, ref mi);
				return new PInvokeUtils.SIZE(mi.rcMonitor.Width, mi.rcMonitor.Height);
#elif GTKMONO
				return new PInvokeUtils.SIZE();
#elif OSX && XAMARIN
				var sz = _nsview.Window.Screen.Frame.Size;
				return new PInvokeUtils.SIZE((int)sz.Width, (int)sz.Height);
#endif
			}
		}

		public PInvokeUtils.SIZE Size
		{
			get
			{
#if WINDOWS || NETCORE
				PInvokeUtils.RECT rectWindow;
				PInvokeWindows.GetWindowRect(Handle, out rectWindow);
				return new PInvokeUtils.SIZE { cx = rectWindow.Width, cy = rectWindow.Height };
#elif GTKMONO
				int window_width, window_height;
				PInvokeGTK.gtk_window_get_size(_gtkwindow, out window_width, out window_height);
				return new PInvokeUtils.SIZE(window_width, window_height);
#elif OSX && XAMARIN
				var sz = _nsview.Window.Frame.Size;
				return new PInvokeUtils.SIZE { cx = (int)sz.Width, cy = (int)sz.Height };
#endif
			}
		}

		public PInvokeUtils.POINT Position
		{
			get
			{
#if WINDOWS || NETCORE
				PInvokeUtils.RECT rectWindow;
				PInvokeWindows.GetWindowRect(Handle, out rectWindow);
				return new PInvokeUtils.POINT(rectWindow.Left, rectWindow.Top);
#elif GTKMONO
				return new PInvokeUtils.POINT();
#elif OSX && XAMARIN
				var pos = _nsview.Window.Frame.Location;
				return new PInvokeUtils.POINT((int)pos.X, (int)pos.Y);
#endif
			}

			set
			{
#if WINDOWS || NETCORE
				PInvokeWindows.MoveWindow(Handle, value.X, value.Y, Size.cx, Size.cy, false);
#elif GTKMONO
				PInvokeGTK.gtk_window_move(_gtkwindow, value.X, value.Y);
#elif OSX && XAMARIN
				var pt = new CoreGraphics.CGPoint(value.X, value.Y);
				_nsview.Window.SetFrameTopLeftPoint(pt);
#endif
			}
		}

		/// <summary>
		/// Loads the page resource from the given URL or file path
		/// </summary>
		/// <param name="url">URL or file path of the page</param>
		public SciterWindow LoadPage(Uri uri)
		{
			return LoadPage(uri: uri, out _);
		}

        /// <summary>
        /// Loads the page resource from the given URL or file path
        /// </summary>
        /// <param name="url">URL or file path of the page</param>
        /// <param name="loadResult">Result of <see cref="Sciter.SciterApi.SciterLoadFile"/></param>
        public SciterWindow LoadPage(Uri uri, out bool loadResult)
		{
			//TODO: Check why SciterLoadFile() behaves differently in Windows with AbsoluteUri (file:///)
#if WINDOWS
			loadResult = _api.SciterLoadFile(hwnd: Handle, filename: uri.AbsoluteUri.Replace(":///", "://"));
#else
			loadResult = _api.SciterLoadFile(hwnd: Handle, filename: uri.AbsoluteUri);
#endif
			Debug.Assert(loadResult);
			return this;
		}

		/// <summary>
		/// Loads HTML input from a string
		/// </summary>
		/// <param name="html">HTML of the page to be loaded</param>
		/// <param name="baseUrl">Base Url given to the loaded page</param>
		public SciterWindow LoadHtml(string html, string baseUrl = null)
		{
            return LoadHtml(html: html, loadResult: out _, baseUrl: baseUrl);
		}

        /// <summary>
        /// Loads HTML input from a string
        /// </summary>
        /// <param name="html">HTML of the page to be loaded</param>
        /// <param name="loadResult">Result of <see cref="Sciter.SciterApi.SciterLoadHtml"/></param>
        /// <param name="baseUrl">Base Url given to the loaded page</param>
        public SciterWindow LoadHtml(string html, out bool loadResult, string baseUrl = null)
		{
			var bytes = Encoding.UTF8.GetBytes(s: html);
            loadResult = _api.SciterLoadHtml(hwnd: Handle, html: bytes, htmlSize: (uint)bytes.Length, baseUrl: baseUrl);
            Debug.Assert(loadResult);
            return this;
        }

		public SciterWindow Show(bool show = true)
		{
#if WINDOWS || NETCORE
			PInvokeWindows.ShowWindow(Handle, show ? PInvokeWindows.ShowWindowCommands.Show : PInvokeWindows.ShowWindowCommands.Hide);
#elif GTKMONO
			if(show)
				PInvokeGTK.gtk_window_present(_gtkwindow);
			else
				PInvokeGTK.gtk_widget_hide(Handle);
#elif OSX && XAMARIN
			if (show)
			{
				_nsview.Window.MakeMainWindow();
				_nsview.Window.MakeKeyAndOrderFront(null);
			}
			else
			{
				_nsview.Window.OrderOut(_nsview.Window);// PerformMiniaturize?
			}
#endif
			return this;
		}

		public void ShowModal()
		{
			Show();
			PInvokeUtils.RunMsgLoop();
		}


		/// <summary>
		/// Close the window. Posts WM_CLOSE message on Windows.
		/// </summary>
		public void Close()
		{
#if WINDOWS || NETCORE
			PInvokeWindows.PostMessage(Handle, PInvokeWindows.Win32Msg.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
#elif GTKMONO
			PInvokeGTK.gtk_window_close(_gtkwindow);
#elif OSX && XAMARIN
			_nsview.Window.Close();
#endif
		}

		public bool IsVisible
		{
			get
			{
#if WINDOWS || NETCORE
				return PInvokeWindows.IsWindowVisible(Handle);
#elif GTKMONO
				return PInvokeGTK.gtk_widget_get_visible(_gtkwindow) != 0;
#elif OSX && XAMARIN
				return _nsview.Window.IsVisible;
#endif
			}
		}

		public IntPtr VM
		{
			get { return _api.SciterGetVM(Handle); }
		}

#if WINDOWS && !WPF
        public SciterWindow SetIcon(Icon icon)
        {
            // instead of using this property, you can use View.windowIcon on all platforms
			// larger icon
			PInvokeWindows.SendMessageW(Handle, PInvokeWindows.Win32Msg.WM_SETICON, new IntPtr(1), icon.Handle);
			// small icon
			PInvokeWindows.SendMessageW(Handle, PInvokeWindows.Win32Msg.WM_SETICON, IntPtr.Zero, new Icon(icon, 16, 16).Handle);
            return this;
        }
#endif

		public SciterWindow SetTitle(string title)
        {
			Title = title;
			return this;
        }

        public string Title
		{
			private set
			{
				Debug.Assert(Handle != IntPtr.Zero);
#if WINDOWS || NETCORE
				IntPtr strPtr = Marshal.StringToHGlobalUni(value);
				PInvokeWindows.SendMessageW(Handle, PInvokeWindows.Win32Msg.WM_SETTEXT, IntPtr.Zero, strPtr);
				Marshal.FreeHGlobal(strPtr);
#elif GTKMONO
				PInvokeGTK.gtk_window_set_title(_gtkwindow, value);
#elif OSX && XAMARIN
				_nsview.Window.Title = value;
#endif
			}

			get
			{
				Debug.Assert(Handle != IntPtr.Zero);
#if WINDOWS || NETCORE
				IntPtr unmanagedPointer = Marshal.AllocHGlobal(2048);
				IntPtr chars_copied = PInvokeWindows.SendMessageW(Handle, PInvokeWindows.Win32Msg.WM_GETTEXT, new IntPtr(2048), unmanagedPointer);
				string title = Marshal.PtrToStringUni(unmanagedPointer, chars_copied.ToInt32());
				Marshal.FreeHGlobal(unmanagedPointer);
				return title;
#elif GTKMONO
				IntPtr str_ptr = PInvokeGTK.gtk_window_get_title(_gtkwindow);
				return Marshal.PtrToStringAnsi(str_ptr);
#elif OSX && XAMARIN
				return _nsview.Window.Title;
#endif
			}
		}

		public SciterElement RootElement
		{
			get
			{
				Debug.Assert(Handle != IntPtr.Zero);
				IntPtr he;
				var r = _api.SciterGetRootElement(Handle, out he);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);

				if(he == IntPtr.Zero)
					return null;// no page loaded yet?
				return new SciterElement(he);
			}
		}

		/// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		public SciterElement ElementAtPoint(int x, int y)
		{
			PInvokeUtils.POINT pt = new PInvokeUtils.POINT() 
			{ 
				X = x, 
				Y = y 
			};

			IntPtr outhe;
			var r = _api.SciterFindElement(Handle, pt, out outhe);

			Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);

			if(outhe == IntPtr.Zero)
				return null;
			return new SciterElement(outhe);
		}

		/// <summary>
		/// Searches this windows DOM tree for element with the given UID
		/// </summary>
		/// <returns>The element, or null if it doesn't exists</returns>
		public SciterElement ElementByUID(uint uid)
		{
			IntPtr he;
			var r = _api.SciterGetElementByUID(Handle, uid, out he);
			Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);

			if(he != IntPtr.Zero)
			{
				return new SciterElement(he);
			}

			return null;
		}

		public uint GetMinWidth()
		{
			Debug.Assert(Handle != IntPtr.Zero);
			return _api.SciterGetMinWidth(Handle);
		}

		public uint GetMinHeight(uint for_width)
		{
			Debug.Assert(Handle != IntPtr.Zero);
			return _api.SciterGetMinHeight(Handle, for_width);
		}

		/// <summary>
		/// Update pending changes in Sciter window and forces painting if necessary
		/// </summary>
		public bool UpdateWindow()
		{
			return _api.SciterUpdateWindow(Handle);
		}

		public SciterValue CallFunction(string name, params SciterValue[] args)
		{
			Debug.Assert(Handle != IntPtr.Zero, "Create the window first");
			Debug.Assert(name != null);

			Interop.SciterValue.VALUE vret = new Interop.SciterValue.VALUE();
			_api.SciterCall(Handle, name, (uint)args.Length, SciterValue.ToVALUEArray(args), out vret);
			return new SciterValue(vret);
		}

		public SciterValue EvalScript(string script)
		{
			Debug.Assert(Handle != IntPtr.Zero, "Create the window first");
			Debug.Assert(script != null);

			Interop.SciterValue.VALUE vret = new Interop.SciterValue.VALUE();
			_api.SciterEval(Handle, script, (uint)script.Length, out vret);
			return new SciterValue(vret);
		}

		/// <summary>
		/// For example media type can be "handheld", "projection", "screen", "screen-hires", etc.
		/// By default sciter window has "screen" media type.
		/// Media type name is used while loading and parsing style sheets in the engine so
		/// you should call this function* before* loading document in it.
		/// </summary>
		public bool SetMediaType(string mediaType)
		{
			return _api.SciterSetMediaType(Handle, mediaType);
		}

		/// <summary>
		/// For example media type can be "handheld:true", "projection:true", "screen:true", etc.
		/// By default sciter window has "screen:true" and "desktop:true"/"handheld:true" media variables.
		/// Media variables can be changed in runtime. This will cause styles of the document to be reset.
		/// </summary>
		/// <param name="mediaVars">Map that contains name/value pairs - media variables to be set</param>
		public bool SetMediaVars(SciterValue mediaVars)
		{
			Interop.SciterValue.VALUE v = mediaVars.ToVALUE();
			return _api.SciterSetMediaVars(Handle, ref v);
		}

#if WINDOWS || NETCORE
		private IntPtr InternalProcessSciterWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr pParam, ref bool handled)
		{
			Debug.Assert(pParam.ToInt32() == 0);
			Debug.Assert(Handle.ToInt32() == 0 || hwnd == Handle);

			IntPtr lResult = IntPtr.Zero;
			handled = ProcessWindowMessage(hwnd, msg, wParam, lParam, ref lResult);
			return lResult;
		}

		protected virtual bool ProcessWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref IntPtr lResult)// overrisable
		{
			return false;
		}
#endif
	}
}