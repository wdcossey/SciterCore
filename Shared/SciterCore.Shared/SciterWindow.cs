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
using System.ComponentModel;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;
using SciterCore.PlatformWrappers;
#if OSX && XAMARIN
using AppKit;
using Foundation;
#elif WINDOWS && !WPF
using System.Drawing;
#endif

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global
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

	public class SciterWindow : IDisposable
#if WINDOWS && !WPF
		, System.Windows.Forms.IWin32Window
#endif
	{
		private static readonly ISciterApi SciterApi = Sciter.SciterApi;
		private static readonly ISciterWindowWrapper WindowWrapper = SciterWindowWrapper.NativeMethodWrapper.GetInterface();

		private IntPtr _handle;

		/// <summary>
		/// Typically this is the Sciter Handle.<br/>
		/// Depending on the platform, this does not always coincide with the <see cref="WindowHandle"/>
		/// </summary>
		public IntPtr Handle
		{
			get => _handle;
			protected set => _handle = value;
		}
		
		/// <summary>
		/// Typically this is the (platform) Window Handle.<br/>
		/// Depending on the platform, this may coincide with the <see cref="Handle"/>
		/// </summary>
		public IntPtr WindowHandle => WindowWrapper.GetWindowHandle(_handle);
		
#if OSX && XAMARIN
		public NSView _nsview { get; private set; }
#endif

		public static implicit operator IntPtr(SciterWindow wnd)
		{
			return wnd.Handle;
		}

		public bool SetSciterOption(SciterXDef.SCITER_RT_OPTIONS option, IntPtr value)
		{
			Debug.Assert(Handle != IntPtr.Zero);
			return SetSciterOption(Handle, option, value);
		}

		public bool SetSciterOption(IntPtr windowHandle, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value)
		{
			return SciterApi.SciterSetOption(windowHandle, option, value);
		}

		public SciterWindow()
		{
			SciterApi.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_SCRIPT_RUNTIME_FEATURES, new IntPtr((int)DefaultRuntimeFeatures));

#if WINDOWS || NETCORE
			WindowDelegateRegistry.Set(this, InternalProcessSciterWindowMessage);
#endif
		}

		// TODO: Check this when running x-platform.
		public SciterWindow(IntPtr hwnd, bool weakReference = false)
		{
			Handle = hwnd;

			if (!weakReference)
			{
#if WINDOWS || NETCORE
				WindowDelegateRegistry.Set(this, InternalProcessSciterWindowMessage);
#elif OSX && XAMARIN
				_nsview = new OSXView(Handle);
#endif
			}
		}

		public const SciterXDef.SCITER_CREATE_WINDOW_FLAGS DefaultCreateFlags =
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY;
		
		public const SciterXDef.SCRIPT_RUNTIME_FEATURES DefaultRuntimeFeatures = 
			SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_EVAL |
			SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_FILE_IO |
			SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SOCKET_IO |
			SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SYSINFO;
		
		//
		/// <summary>
		/// Creates the Sciter window and returns the native handle
		/// </summary>
		/// <param name="frame">Rectangle of the window</param>
		/// <param name="creationFlags">Flags for the window creation, defaults to SW_MAIN | SW_TITLEBAR | SW_RESIZEABLE | SW_CONTROLS</param>
		public SciterWindow CreateWindow(SciterRectangle frame = new SciterRectangle(), SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags, IntPtr parent = new IntPtr())
		{

#if DEBUG
			// Force Sciter SW_ENABLE_DEBUG in Debug build.
			creationFlags |= SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG;
#endif
			Debug.Assert(Handle == IntPtr.Zero);
			Handle = SciterApi.SciterCreateWindow(
				creationFlags,
				frame,
				WindowDelegateRegistry.Get(this),
				IntPtr.Zero,
				parent
			);
			Debug.Assert(Handle != IntPtr.Zero);

			if(Handle == IntPtr.Zero)
				throw new Exception("CreateWindow() failed");

#if OSX && XAMARIN
			_nsview = new OSXView(Handle);
#endif
			return this;
		}

		public SciterWindow CreateMainWindow(int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
		{
			var frame = new SciterRectangle(width, height);
			return CreateWindow(frame, creationFlags);
		}

		public SciterWindow CreateOwnedWindow(IntPtr owner, int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
		{
			var frame = new SciterRectangle(width, height);
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
		public static SciterWindow CreateChildWindow(
			IntPtr hwndParent, 
			Func<SciterXDef.SCRIPT_RUNTIME_FEATURES> runtimeFeatures = null)
		{
			if(PInvokeWindows.IsWindow(hwndParent) == false)
				throw new ArgumentException("Invalid parent window");

			PInvokeWindows.GetClientRect(hwndParent, out var frame);

			SciterApi.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_SCRIPT_RUNTIME_FEATURES, new IntPtr((int)(runtimeFeatures?.Invoke() ?? DefaultRuntimeFeatures)));
			
#if DEBUG
			SciterApi.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, new IntPtr(1));
#endif

#if true
            var wndclass = SciterApi.SciterClassName();
            
            var childWindowHandle = PInvokeWindows.CreateWindowEx(
	            (int)(0),
	            wndclass,
	            null,
	            (int)(PInvokeWindows.WindowStyles.WS_CHILD),
	            0, 
	            0, 
	            frame.Right, 
	            frame.Bottom,
	            hwndParent,
	            IntPtr.Zero,
	            IntPtr.Zero,
	            IntPtr.Zero);

            return new SciterWindow(childWindowHandle);
            
			//Hwnd = PInvokeWindows.CreateWindowEx(0, wndclass, null, (int)PInvokeWindows.WindowStyles.WS_CHILD, 0, 0, frame.Right, frame.Bottom, hwnd_parent, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			//SetSciterOption(SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, new IntPtr(1));// NO, user should opt for it
#else
			Hwnd = SciterApi.SciterCreateWindow(flags, ref frame, _proc, IntPtr.Zero, hwnd_parent);
#endif
		}
#endif

		public void Destroy() => 
			WindowWrapper.Destroy(WindowHandle);

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
		public SciterWindow CenterWindow()
		{
#if OSX && XAMARIN
			_nsview.Window.Center();
#else
			WindowWrapper.CenterWindow(WindowHandle);
#endif
			return this;
		}

		/// <summary>
		/// Dimensions of the Primary Screen
		/// </summary>
		/// <returns><see cref="SciterSize"/> dimensions of the primary</returns>
		public static SciterSize PrimaryScreenSize
		{
			get
			{
#if OSX && XAMARIN
				var sz = NSScreen.MainScreen.Frame.Size;
				return new SciterSize((int)sz.Width, (int)sz.Height);
#else
				return WindowWrapper.GetPrimaryScreenSize();
#endif
			}
		}

		/// <summary>
		/// Dimensions of the Screen (owning the <see cref="SciterWindow"/>)
		/// </summary>
		public SciterSize ScreenSize
		{
			get
			{
#if OSX && XAMARIN
				var sz = _nsview.Window.Screen.Frame.Size;
				return new SciterSize((int)sz.Width, (int)sz.Height);
#else
				return WindowWrapper.GetScreenSize(WindowHandle);
#endif
			}
		}

		/// <summary>
		/// Dimensions of this <see cref="SciterWindow"/>
		/// </summary>
		public SciterSize Size
		{
			get
			{
#if OSX && XAMARIN
				var sz = _nsview.Window.Frame.Size;
				return new SciterSize((int)sz.Width, (int)sz.Height);
#else
				return WindowWrapper.Size(WindowHandle);
#endif
			}
		}

		/// <summary>
		/// Get the position of this <see cref="SciterWindow"/> on the Screen
		/// </summary>
		public SciterPoint GetPosition()
		{
#if OSX && XAMARIN
			var pos = _nsview.Window.Frame.Location;
			return new SciterPoint((int)pos.X, (int)pos.Y);
#else
			return WindowWrapper.GetPosition(WindowHandle);
#endif
		}

		/// <summary>
		/// Set the position of this <see cref="SciterWindow"/> on the Screen
		/// </summary>
		public SciterWindow SetPosition(SciterPoint point)
		{
#if OSX && XAMARIN
			var pt = new CoreGraphics.CGPoint(point.X, point.Y);
			_nsview.Window.SetFrameTopLeftPoint(pt);
#else
			WindowWrapper.SetPosition(WindowHandle, point);
#endif
			return this;
		}

		/// <summary>
		/// Loads the page resource from the given URL or file path
		/// </summary>
		/// <param name="uri">URL or file path of the page</param>
		internal void LoadPageInternal(Uri uri)
		{
			TryLoadPageInternal(uri);
		}

		/// <summary>
        /// Loads the page resource from the given URL or file path
        /// </summary>
        /// <param name="uri">URL or file path of the page</param>
        internal bool TryLoadPageInternal(Uri uri)
        {
	        var absoluteUri = uri.AbsoluteUri;

#if WINDOWS || NETCORE
	        //TODO: Check why SciterLoadFile() behaves differently in Windows with AbsoluteUri (file:///)
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				absoluteUri = absoluteUri.Replace(":///", "://");
#endif
	        return SciterApi.SciterLoadFile(hwnd: Handle, filename: absoluteUri);
        }

		/// <summary>
		/// Loads HTML input from a string
		/// </summary>
		/// <param name="html">HTML of the page to be loaded</param>
		/// <param name="baseUrl">Base Url given to the loaded page</param>
        internal void LoadHtmlInternal(string html, string baseUrl = null)
		{
            TryLoadHtmlInternal(html: html, baseUrl: baseUrl);
		}
        
        /// <summary>
        /// Loads HTML input from a string
        /// </summary>
        /// <param name="html">HTML of the page to be loaded</param>
        /// <param name="baseUrl">Base Url given to the loaded page</param>
        internal bool TryLoadHtmlInternal(string html, string baseUrl = null)
		{
			var bytes = Encoding.UTF8.GetBytes(s: html);
			return SciterApi.SciterLoadHtml(hwnd: Handle, html: bytes, htmlSize: (uint)bytes.Length, baseUrl: baseUrl);
		}

		public SciterWindow Show(bool show = true)
		{
#if OSX && XAMARIN
			if (show)
			{
				_nsview.Window.MakeMainWindow();
				_nsview.Window.MakeKeyAndOrderFront(null);
			}
			else
			{
				_nsview.Window.OrderOut(_nsview.Window);// PerformMiniaturize?
			}
#else
			WindowWrapper.Show(WindowHandle, show);
#endif
			
			OnWindowShow?.Invoke(this, EventArgs.Empty);
			
			return this;
		}

		public EventHandler OnWindowShow;

		public void ShowModal() =>
			WindowWrapper.ShowModal(WindowHandle);

		/// <summary>
		/// Close the window. Posts WM_CLOSE message on Windows.
		/// </summary>
		public void Close()
		{
			var args = new CancelEventArgs(false);
			
			OnWindowClosing?.Invoke(this, args);
			
			if (args.Cancel)
				return;
			
#if OSX && XAMARIN
			_nsview.Window.Close();
#else
			WindowWrapper.Close(WindowHandle);
#endif
			
			OnWindowClosed?.Invoke(this, EventArgs.Empty);
		}
		
		public EventHandler<CancelEventArgs> OnWindowClosing;
		
		public EventHandler OnWindowClosed;

		public bool IsVisible
		{
			get
			{
#if OSX && XAMARIN
				return _nsview.Window.IsVisible;
#else
				return WindowWrapper.GetIsVisible(WindowHandle);
#endif
			}
		}

#if WINDOWS && !WPF
        public SciterWindow SetIcon(Icon icon)
        {
            // instead of using this property, you can use View.windowIcon on all platforms
			// larger icon
			PInvokeWindows.SendMessageW(WindowHandle, PInvokeWindows.Win32Msg.WM_SETICON, new IntPtr(1), icon.Handle);
			// small icon
			PInvokeWindows.SendMessageW(WindowHandle, PInvokeWindows.Win32Msg.WM_SETICON, IntPtr.Zero, new Icon(icon, 16, 16).Handle);
            return this;
        }
#endif

		#region Title
		
		internal void SetTitleInternal(string title)
        {
	        Debug.Assert(WindowHandle != IntPtr.Zero);
	        
#if OSX && XAMARIN
			_nsview.Window.Title = title;
#else
	        WindowWrapper.SetTitle(WindowHandle, title);
#endif
        }

		internal string GetTitleInternal()
        {
	        Debug.Assert(WindowHandle != IntPtr.Zero);
#if OSX && XAMARIN
			return _nsview.Window.Title;
#else
	        return WindowWrapper.GetTitle(WindowHandle);
#endif
        }

        public string Title
        {
	        get => GetTitleInternal();
	        private set => SetTitleInternal(value);
        }
        
        #endregion

        #region Elements
        
		public SciterElement RootElement => GetRootElementInternal();

		internal SciterElement GetRootElementInternal()
		{
			TryGetRootElementInternal(out var result);
			return result;
		}

		internal bool TryGetRootElementInternal(out SciterElement element)
		{
			Debug.Assert(Handle != IntPtr.Zero);
			
			var result = SciterApi.SciterGetRootElement(Handle, out var elementHandle)
				.IsOk();

			element = result ? SciterElement.Attach(elementHandle) : null; // no page loaded yet?
			return result;
		}

		/// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		internal SciterElement GetElementAtPointInternal(int x, int y)
		{
			TryGetElementAtPointInternal(out var result, x, y);
			return result;
		}

		/// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		internal bool TryGetElementAtPointInternal(out SciterElement value, int x, int y)
		{
			var point = new SciterPoint(x, y);
			
			var result = SciterApi.SciterFindElement(Handle, point, out var elementHandle)
				.IsOk();

			value = result ? SciterElement.Attach(elementHandle) : null;
				
			return result;
		}

		/// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		internal bool TryGetElementAtPointInternal(out SciterElement value, SciterPoint point)
		{
			return TryGetElementAtPointInternal(out value, point.X, point.Y);
		}

		/// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		internal SciterElement GetElementAtPointInternal(SciterPoint point)
		{
			return GetElementAtPointInternal(point.X, point.Y);
		}

		/// <summary>
		/// Searches this window DOM tree for element with the given UID
		/// </summary>
		/// <returns>The element, or null if it doesn't exists</returns>
		internal SciterElement GetElementByUidInternal(uint uid)
		{
			TryGetElementByUidInternal(out var result, uid);
			return result;
		}

		/// <summary>
		/// Searches this window DOM tree for element with the given UID
		/// </summary>
		/// <returns>The element, or null if it doesn't exists</returns>
		internal bool TryGetElementByUidInternal(out SciterElement value, uint uid)
		{
			var result = SciterApi.SciterGetElementByUID(Handle, uid, out var elementHandle)
				.IsOk();
			
			value = result ? SciterElement.Attach(elementHandle) : null;

			return result;
		}
		
		#endregion

		#region Dimensions

		internal int GetMinWidthInternal()
		{
			return unchecked((int)SciterApi.SciterGetMinWidth(Handle));
		}

		internal int GetMinHeightInternal(int width)
		{
			return unchecked((int)SciterApi.SciterGetMinHeight(Handle, unchecked((uint)width)));
		}

		#endregion

		/// <summary>
		/// Update pending changes in Sciter window and forces painting if necessary
		/// </summary>
		internal void UpdateWindowInternal()
		{
			TryUpdateWindowInternal();
		}

		/// <summary>
		/// Update pending changes in Sciter window and forces painting if necessary
		/// </summary>
		internal bool TryUpdateWindowInternal()
		{
			return SciterApi.SciterUpdateWindow(Handle);
		}

		public SciterValue CallFunction(string name, params SciterValue[] args)
		{
			Debug.Assert(Handle != IntPtr.Zero, "Create the window first");
			Debug.Assert(name != null);

			Interop.SciterValue.VALUE vret = new Interop.SciterValue.VALUE();
			SciterApi.SciterCall(Handle, name, (uint)args.Length, args.AsValueArray(), out vret);
			return SciterValue.Attach(vret);
		}

		public SciterValue EvalScript(string script)
		{
			Debug.Assert(Handle != IntPtr.Zero, "Create the window first");
			Debug.Assert(script != null);

			Interop.SciterValue.VALUE vret = new Interop.SciterValue.VALUE();
			SciterApi.SciterEval(Handle, script, (uint)script.Length, out vret);
			return SciterValue.Attach(vret);
		}

		/// <summary>
		/// For example media type can be "handheld", "projection", "screen", "screen-hires", etc.
		/// By default sciter window has "screen" media type.
		/// Media type name is used while loading and parsing style sheets in the engine so
		/// you should call this function* before* loading document in it.
		/// </summary>
		public bool SetMediaType(string mediaType)
		{
			return SciterApi.SciterSetMediaType(Handle, mediaType);
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
			return SciterApi.SciterSetMediaVars(Handle, ref v);
		}

#if WINDOWS || NETCORE
		private IntPtr InternalProcessSciterWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr pParam, ref bool handled)
		{
			Debug.Assert(pParam.ToInt32() == 0);
			Debug.Assert(WindowHandle.ToInt32() == 0 || hwnd == WindowHandle);

			IntPtr lResult = IntPtr.Zero;
			handled = ProcessWindowMessage(hwnd, msg, wParam, lParam, ref lResult);

			if (msg == (int) PInvokeWindows.Win32Msg.WM_CLOSE && hwnd == WindowHandle)
				this.Dispose();
			
			return lResult;
		}

		protected virtual bool ProcessWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref IntPtr lResult)// overridable
		{
			return false;
		}
#endif
		private void ReleaseUnmanagedResources()
		{ 
			WindowDelegateRegistry.Remove(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			ReleaseUnmanagedResources();
			if (disposing)
			{
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SciterWindow()
		{
			Dispose(false);
		}
	}
}