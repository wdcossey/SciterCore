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
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using SciterCore.Interop;

namespace SciterCore
{
	public class SciterHost
	{
		const int INVOKE_NOTIFICATION = 0x8206241;

		private static Sciter.SciterApi _api = Sciter.Api;

		private IntPtr _windowHandle;
		private Dictionary<string, EventHandlerRegistry> _behaviorMap = new Dictionary<string, EventHandlerRegistry>();

		private SciterXDef.SCITER_HOST_CALLBACK _hostCallback;
		private SciterEventHandler _windowEventHandler;

		public static bool InjectLibConsole = true;
		private static List<IntPtr> _lib_console_vms = new List<IntPtr>();
		private static SciterArchive _consoleArchive;
		private SciterEventHandler _window_evh;

		private class DefaultEventHandler : SciterEventHandler { }

        protected IntPtr WindowHandle
        {
			get => _windowHandle;
		    set => _windowHandle = value;
        }

		static SciterHost()
		{
			_consoleArchive = new SciterArchive("scitersharp:")
                .Open("LibConsole");

			if(InjectLibConsole)
			{
				byte[] byteArray = Encoding.UTF8.GetBytes("include \"scitersharp:console.tis\";");
				GCHandle pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
				IntPtr pointer = pinnedArray.AddrOfPinnedObject();
				Sciter.Api.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_INIT_SCRIPT, pointer);
				pinnedArray.Free();
			}
		}

		public SciterHost()
        {
            //
        }

		public SciterHost(Func<SciterWindow> windowFunc)
            : this(window: windowFunc?.Invoke())
		{
            //
		}

		public SciterHost(SciterWindow window)
			: this()
		{
			SetupWindow(window.Handle);
		}

		//
		public SciterHost SetupWindow(SciterWindow window)
		{ 
			Debug.Assert(window != null);
			Debug.Assert(window.Handle != IntPtr.Zero);
			Debug.Assert(WindowHandle == IntPtr.Zero, "You already called SetupWindow()");

			if (window == null)
			{
				throw new ArgumentNullException(nameof(window));
			}

			return SetupWindow(window.Handle);
		}

		private SciterHost SetupWindow(IntPtr hwnd)
		{
			Debug.Assert(hwnd != IntPtr.Zero);
			Debug.Assert(WindowHandle == IntPtr.Zero, "You already called SetupWindow()");

			WindowHandle = hwnd;

			// Register a global event handler for this Sciter window
			_hostCallback = HandleNotification;
			_api.SciterSetCallback(hwnd, Marshal.GetFunctionPointerForDelegate(_hostCallback), IntPtr.Zero);

			return this;
		}

		public SciterHost InjectGlobalTISript(string script)
		{
			var ret = new TIScript.tiscript_value();
			var res = EvalGlobalTISript(script, out ret);
			Debug.Assert(res);

			return this;
		}

		public bool EvalGlobalTISript(string script, out TIScript.tiscript_value ret)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero);
			var vm = Sciter.Api.SciterGetVM(WindowHandle);
			Debug.Assert(vm != IntPtr.Zero);

			var global_ns = Sciter.ScriptApi.get_global_ns(vm);

			return Sciter.ScriptApi.eval_string(vm, global_ns, script, (uint)script.Length, out ret);
		}

		public bool EvalGlobalTISriptValuePath(string path, out TIScript.tiscript_value ret)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero);
			var vm = Sciter.Api.SciterGetVM(WindowHandle);

			return Sciter.ScriptApi.get_value_by_path(vm, out ret, path);
		}

		/// <summary>
		/// Attaches a window level event-handler: it receives every event for all elements of the page.
		/// You normally attaches it before loading the page HTML with <see cref="SciterWindow.LoadPage(Uri)"/>
		/// You can only attach a single event-handler.
		/// </summary>
		public SciterHost AttachEventHandler(SciterEventHandler eventHandler)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(eventHandler != null);
			Debug.Assert(_windowEventHandler == null, "You can attach only a single SciterEventHandler per SciterHost/Window");
			
			_windowEventHandler = eventHandler;
			_api.SciterWindowAttachEventHandler(WindowHandle, eventHandler.EventProc, IntPtr.Zero, (uint)SciterBehaviors.EVENT_GROUPS.HANDLE_ALL);

			return this;
		}

		/// <summary>
		/// Detaches the event-handler previously attached with AttachEvh()
		/// </summary>
		public SciterHost DetachEventHandler()
		{
			Debug.Assert(_windowEventHandler != null);
			if(_windowEventHandler != null)
			{
				_api.SciterWindowDetachEventHandler(WindowHandle, _windowEventHandler.EventProc, IntPtr.Zero);
				_windowEventHandler = null;
			}

			return this;
		}

		public SciterValue CallFunction(string name, params SciterValue[] args)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(name != null);

			Interop.SciterValue.VALUE vret = new Interop.SciterValue.VALUE();
			_api.SciterCall(WindowHandle, name, (uint)args.Length, SciterValue.ToVALUEArray(args), out vret);
			return new SciterValue(vret);
		}

		public SciterValue EvalScript(string script)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(script != null);

			Interop.SciterValue.VALUE vret = new Interop.SciterValue.VALUE();
			_api.SciterEval(WindowHandle, script, (uint)script.Length, out vret);
			return new SciterValue(vret);
		}

		/// <summary>
		/// Posts a message to the UI thread to invoke the given Action. This methods returns immediatly, does not wait for the message processing.
		/// </summary>
		/// <param name="what">The delegate which will be invoked</param>
		public void InvokePost(Action what)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(what != null);

			GCHandle handle = GCHandle.Alloc(what);
			PostNotification(new IntPtr(INVOKE_NOTIFICATION), GCHandle.ToIntPtr(handle));
		}

		/// <summary>
		/// Sends a message to the UI thread to invoke the given Action. This methods waits for the message processing until timeout is exceeded.
		/// </summary>
		/// <param name="what">The delegate which will be invoked</param>
		public void InvokeSend(Action what, uint timeout = 3000)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(what != null);
			Debug.Assert(timeout > 0);

			GCHandle handle = GCHandle.Alloc(what);
			PostNotification(new IntPtr(INVOKE_NOTIFICATION), GCHandle.ToIntPtr(handle), timeout);
		}

		public void DebugInspect()
		{
			string inspector_proc = "inspector";
			var ps = Process.GetProcessesByName(inspector_proc);
			if(ps.Length==0)
			{
				throw new Exception("Inspector process is not running. You should run it before calling DebugInspect()");
			}

			Task.Run(() =>
			{
				Thread.Sleep(1000);
				EvalScript("view.connectToInspector()");

#if OSX
				var app_inspector = AppKit.NSRunningApplication.GetRunningApplications("terrainformatica.inspector");
				if(app_inspector.Length==1)
					app_inspector[0].Activate(AppKit.NSApplicationActivationOptions.ActivateAllWindows);
#endif
			});
		}

		/*
		/// <summary>
		/// Runs the inspector process, waits 1 second, and calls view.connectToInspector() to inspect your page.
		/// (Before everything it kills any previous instance of the inspector process)
		/// </summary>
		/// <param name="inspector_exe">Path to the inspector executable, can be an absolute or relative path.</param>
		public void DebugInspect(string inspector_exe)
		{
			var ps = Process.GetProcessesByName(inspector_exe);
			foreach(var p in ps)
				p.Kill();

			string path = null;
#if WINDOWS
			if(!File.Exists(inspector_exe) && !File.Exists(inspector_exe + ".exe"))
			{
				path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SciterHost)).Location) + '\\' + inspector_exe;
			}
#elif OSX
			if(!File.Exists(inspector_exe))
				path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SciterHost)).Location) + "../../../" +  inspector_exe;
#else
			if(!File.Exists(inspector_exe))
				path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(SciterHost)).Location) + inspector_exe;
#endif

			Process proc = null;
			try
			{
				if(path != null && File.Exists(path))
					proc = Process.Start(path);
				else
					// try from PATH environment
					proc = Process.Start(inspector_exe);
			}
			catch(Exception)
			{
			}
			if(proc.HasExited)
				throw new Exception("Could not run inspector. Make sure Sciter DLL is also present in the inspector tool directory.");

			Task.Run(() =>
			{
				Thread.Sleep(1000);
				InvokePost(() =>
				{
					EvalScript("view.connectToInspector()"); ;
				});
			});
		}*/

		/// <summary>
		/// Sciter cross-platform alternative for posting a message in the message queue.
		/// It will be received as a SC_POSTED_NOTIFICATION notification by this SciterHost instance.
		/// Override OnPostedNotification() to handle it.
		/// </summary>
		/// <param name="timeout">
		/// If timeout is > 0 this methods SENDs the message instead of POSTing and this is the timeout for waiting the processing of the message. Leave it as 0 for actually POSTing the message.
		/// </param>
		public IntPtr PostNotification(IntPtr wparam, IntPtr lparam, uint timeout = 0)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			return _api.SciterPostCallback(WindowHandle, wparam, lparam, timeout);
		}

		// Behavior factory
		public SciterHost RegisterBehaviorHandler(Type eventHandlerType, string behaviorName = null)
		{
			var entry = new EventHandlerRegistry(type: eventHandlerType, name: behaviorName);
			_behaviorMap[entry.Name] = entry;
			return this;
		}

		public SciterHost RegisterBehaviorHandler<THandler>(THandler eventHandler, string behaviorName = null)
			where THandler : SciterEventHandler
		{
			var entry = new EventHandlerRegistry(eventHandler: eventHandler, name: behaviorName);
			_behaviorMap[entry.Name] = entry;
			return this;
		}

		public SciterHost RegisterBehaviorHandler<THandler>(Func<THandler> eventHandler, string behaviorName = null)
			where THandler : SciterEventHandler
		{
			return RegisterBehaviorHandler(eventHandler?.Invoke(), behaviorName);
		}

		// Properties
		public SciterElement RootElement
		{
			get
			{
				Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
				IntPtr heRoot;
				_api.SciterGetRootElement(WindowHandle, out heRoot);
				Debug.Assert(heRoot != IntPtr.Zero);
				return new SciterElement(heRoot);
			}
		}

		public SciterElement FocusElement
		{
			get
			{
				Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
				IntPtr heFocus;
				_api.SciterGetRootElement(WindowHandle, out heFocus);
				Debug.Assert(heFocus != IntPtr.Zero);
				return new SciterElement(heFocus);
			}
		}

        public SciterEventHandler WindowEventHandler
        {
            get => _window_evh;
            protected set => _window_evh = value;
        }

        // Notification handler
        private uint HandleNotification(IntPtr ptrNotification, IntPtr callbackParam)
		{
			SciterXDef.SCITER_CALLBACK_NOTIFICATION scn = (SciterXDef.SCITER_CALLBACK_NOTIFICATION)Marshal.PtrToStructure(ptrNotification, typeof(SciterXDef.SCITER_CALLBACK_NOTIFICATION));

			switch(scn.code)
			{
				case SciterXDef.SCITER_CALLBACK_CODE.SC_LOAD_DATA:
					SciterXDef.SCN_LOAD_DATA sld = (SciterXDef.SCN_LOAD_DATA)Marshal.PtrToStructure(ptrNotification, typeof(SciterXDef.SCN_LOAD_DATA));
					return (uint)OnLoadData(sld);

				case SciterXDef.SCITER_CALLBACK_CODE.SC_DATA_LOADED:
					SciterXDef.SCN_DATA_LOADED sdl = (SciterXDef.SCN_DATA_LOADED)Marshal.PtrToStructure(ptrNotification, typeof(SciterXDef.SCN_DATA_LOADED));
					OnDataLoaded(sdl);
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_ATTACH_BEHAVIOR:
					SciterXDef.SCN_ATTACH_BEHAVIOR sab = (SciterXDef.SCN_ATTACH_BEHAVIOR)Marshal.PtrToStructure(ptrNotification, typeof(SciterXDef.SCN_ATTACH_BEHAVIOR));
					SciterEventHandler elementEvh;
					string behaviorName = Marshal.PtrToStringAnsi(sab.behaviorName);
					bool res = OnAttachBehavior(new SciterElement(sab.elem), behaviorName, out elementEvh);
					if(res)
					{
						SciterBehaviors.ELEMENT_EVENT_PROC proc = elementEvh.EventProc;
						IntPtr ptrProc = Marshal.GetFunctionPointerForDelegate(proc);

						IntPtr EVENTPROC_OFFSET = Marshal.OffsetOf(typeof(SciterXDef.SCN_ATTACH_BEHAVIOR), "elementProc");
						IntPtr EVENTPROC_OFFSET2 = Marshal.OffsetOf(typeof(SciterXDef.SCN_ATTACH_BEHAVIOR), "elementTag");
						Marshal.WriteIntPtr(ptrNotification, EVENTPROC_OFFSET.ToInt32(), ptrProc);
						Marshal.WriteInt32(ptrNotification, EVENTPROC_OFFSET2.ToInt32(), 0);
						return 1;
					}
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_ENGINE_DESTROYED:
					if(_windowEventHandler != null)
					{
						_api.SciterWindowDetachEventHandler(WindowHandle, _windowEventHandler.EventProc, IntPtr.Zero);
						_windowEventHandler = null;
					}

					OnEngineDestroyed();
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_POSTED_NOTIFICATION:
					SciterXDef.SCN_POSTED_NOTIFICATION spn = (SciterXDef.SCN_POSTED_NOTIFICATION)Marshal.PtrToStructure(ptrNotification, typeof(SciterXDef.SCN_POSTED_NOTIFICATION));
					IntPtr lreturn = IntPtr.Zero;
					if(spn.wparam.ToInt32() == INVOKE_NOTIFICATION)
					{
						GCHandle handle = GCHandle.FromIntPtr(spn.lparam);
						Action cbk = (Action)handle.Target;
						cbk();
						handle.Free();
					}
					else
					{
						lreturn = OnPostedNotification(spn.wparam, spn.lparam);
					}

					IntPtr OFFSET_LRESULT = Marshal.OffsetOf(typeof(SciterXDef.SCN_POSTED_NOTIFICATION), "lreturn");
					Marshal.WriteIntPtr(ptrNotification, OFFSET_LRESULT.ToInt32(), lreturn);
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_GRAPHICS_CRITICAL_FAILURE:
					SciterXDef.SCN_GRAPHICS_CRITICAL_FAILURE cgf = (SciterXDef.SCN_GRAPHICS_CRITICAL_FAILURE)Marshal.PtrToStructure(ptrNotification, typeof(SciterXDef.SCN_GRAPHICS_CRITICAL_FAILURE));
					OnGraphicsCriticalFailure(cgf.hwnd);
					return 0;

				default:
					Debug.Assert(false);
					break;
			}
			return 0;
		}

		// Overridables
		protected virtual SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");

			var uri = new Uri(sld.uri);

			if(InjectLibConsole)
			{
				_consoleArchive?.GetItem(uri, (data, path) => 
				{ 
					_api.SciterDataReady(WindowHandle, path, data, (uint) data.Length);
				});
			}

			return (uint)SciterXDef.LoadResult.LOAD_OK;
		}

		protected virtual void OnDataLoaded(SciterXDef.SCN_DATA_LOADED sdl)
        {
            //
        }

		protected virtual bool OnAttachBehavior(SciterElement el, string behaviorName, out SciterEventHandler elementEvh)
		{
			// returns a new SciterEventHandler if the behaviorName was registered by a previous RegisterBehaviorHandler() call
			if (_behaviorMap.ContainsKey(behaviorName))
			{
				elementEvh = _behaviorMap[behaviorName].EventHandler;
				return true;
			}
			elementEvh = null;
			return false;
		}

		protected virtual void OnEngineDestroyed()
        {
            //
        }

		protected virtual IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
        {
            return IntPtr.Zero;
        }

		protected virtual void OnGraphicsCriticalFailure(IntPtr hwnd)
        {
            //
        }
	}
}