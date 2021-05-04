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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SciterCore.Interop;
#if NETCORE
	using SciterCore.Internal;
#endif

// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global

namespace SciterCore
{
	public class SciterHost : IDisposable
	{
		//const int INVOKE_NOTIFICATION = 0x8206241;

		private static readonly ISciterApi Api = Sciter.SciterApi;

		private IntPtr _windowHandle;
		
		//TODO: Remove this!
		private readonly Dictionary<string, EventHandlerRegistry> _behaviorMap = new Dictionary<string, EventHandlerRegistry>();
		
		internal SciterEventHandler WindowEventHandler;

		protected internal readonly ConcurrentDictionary<string, SciterArchive> AttachedArchives = new ConcurrentDictionary<string, SciterArchive>();

		#region Events

		public EventHandler<HostCreatedEventArgs> OnCreated;
		
		public EventHandler<HostSetupWindowEventArgs> OnSetupWindow;
		
		public EventHandler<AttachHandlerEventArgs> OnAttachEventHandler;
		
		public EventHandler<DetachHandlerEventArgs> OnDetachEventHandler;
		
		#endregion

#if NETCORE
		private INamedBehaviorResolver _namedBehaviorResolver;
#endif

		protected internal IntPtr WindowHandle
		{
			get => _windowHandle;
			set => _windowHandle = value;
		}
		
		public SciterHost() { }

		public SciterHost(Func<SciterWindow> windowFunc)
			: this(window: windowFunc?.Invoke()) { }

		// ReSharper disable once SuggestBaseTypeForParameter
		public SciterHost(SciterWindow window)
			: this()
		{
			SetupWindow(window);
		}

		//
		public SciterHost SetupWindow(SciterWindow window)
		{
			if (!WindowHandle.Equals(IntPtr.Zero))
				throw new InvalidOperationException($"You already called {nameof(SetupWindow)}.");

			Window = window ?? throw new ArgumentNullException(nameof(window));
			
			return SetupWindow(window.Handle);
		}

		public SciterWindow Window { get; internal set; }

		public SciterHost SetupWindow(IntPtr handle)
		{
			if (handle.Equals(IntPtr.Zero))
				throw new ArgumentOutOfRangeException(nameof(handle), $"Cannot be {nameof(IntPtr.Zero)}.");

			if (!WindowHandle.Equals(IntPtr.Zero))
				throw new InvalidOperationException($"You already called {nameof(SetupWindow)}.");

			WindowHandle = handle;

			if (Window == null || Window.Handle != handle)
			{
				Window = new SciterWindow(handle, true);
			}
			// Register a global event handler for this Sciter window
			var hostCallbackWrapper = HostCallbackRegistry.Instance.GetOrAdd(this, new HostCallbackWrapper(this));
			Api.SciterSetCallback(handle, (SciterXDef.SCITER_HOST_CALLBACK) hostCallbackWrapper.CallbackDelegate, IntPtr.Zero);
			
			OnSetupWindow?.Invoke(this, new HostSetupWindowEventArgs() { Window = this.Window});
			
			return this;
		}

		/*public SciterHost InjectGlobalTIScript(string script)
		{
			var ret = new TIScript.tiscript_value();
			var res = EvalGlobalTIScript(script, out ret);
			Debug.Assert(res);

			return this;
		}

		public bool EvalGlobalTIScript(string script, out TIScript.tiscript_value ret)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero);
			var vm = Sciter.Api.SciterGetVM(WindowHandle);
			Debug.Assert(vm != IntPtr.Zero);

			var global_ns = Sciter.ScriptApi.get_global_ns(vm);

			return Sciter.ScriptApi.eval_string(vm, global_ns, script, (uint)script.Length, out ret);
		}

		public bool EvalGlobalTIScriptValuePath(string path, out TIScript.tiscript_value ret)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero);
			var vm = Sciter.Api.SciterGetVM(WindowHandle);

			return Sciter.ScriptApi.GetValueByPath(vm, out ret, path);
		}*/

		/// <summary>
		/// Attaches a window level event-handler: it receives every event for all elements of the page.
		/// You normally attaches it before loading the page HTML with <see cref="SciterWindowExtensions.LoadPage"/>
		/// You can only attach a single event-handler.
		/// </summary>
		internal void AttachEventHandlerInternal(SciterEventHandler eventHandler)
		{
			TryAttachEventHandlerInternal(eventHandler: eventHandler);
		}

		/// <summary>
		/// Attaches a window level event-handler: it receives every event for all elements of the page.
		/// You normally attaches it before loading the page HTML with <see cref="SciterWindowExtensions.LoadPage"/>
		/// You can only attach a single event-handler.
		/// </summary>
		internal bool TryAttachEventHandlerInternal(SciterEventHandler eventHandler)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, $"Call {GetType().Name}.{nameof(SetupWindow)}() first");
			Debug.Assert(eventHandler != null);
			Debug.Assert(WindowEventHandler == null,
				"You can attach only a single SciterEventHandler per SciterHost/Window");

			WindowEventHandler = eventHandler;
			var result = Api
				.SciterWindowAttachEventHandler(WindowHandle, eventHandler.EventProc, IntPtr.Zero,
					(uint) SciterBehaviors.EVENT_GROUPS.HANDLE_ALL)
				.IsOk();

			eventHandler?.SetHost(result ? this : null);

			OnAttachEventHandler?.Invoke(this, new AttachHandlerEventArgs() {EventHandler = this.WindowEventHandler});
			return result;
		}

		/// <summary>
		/// Detaches the event-handler previously attached with AttachEvh()
		/// </summary>
		internal void DetachEventHandlerInternal()
		{
			TryDetachEventHandlerInternal();
		}

		/// <summary>
		/// Detaches the event-handler previously attached with AttachEvh()
		/// </summary>
		internal bool TryDetachEventHandlerInternal()
		{
			Debug.Assert(WindowEventHandler != null);

			OnDetachEventHandler?.Invoke(this, new DetachHandlerEventArgs() { EventHandler = this.WindowEventHandler});
			
			var result = WindowEventHandler == null ||
			             Api.SciterWindowDetachEventHandler(WindowHandle, WindowEventHandler.EventProc, IntPtr.Zero)
				             .IsOk();

			if (result && WindowEventHandler != null)
				WindowEventHandler = null;
			
			return result;
		}

		internal SciterValue CallFunctionInternal(string functionName, params SciterValue[] args)
		{
			TryCallFunctionInternal(out var result, functionName: functionName, args: args);
			return result;
		}

		internal bool TryCallFunctionInternal(out SciterValue value, string functionName, params SciterValue[] args)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(functionName != null);

			var result = Api.SciterCall(WindowHandle, functionName, (uint) args.Length, args.AsValueArray(),
				out var retValue);
			value = result ? SciterValue.Attach(retValue) : SciterValue.Null;
			return result;
		}

		internal SciterValue EvalScriptInternal(string script)
		{
			TryEvalScriptInternal(out var result, script: script);
			return result;
		}

		internal bool TryEvalScriptInternal(out SciterValue value, string script)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(script != null);

			var result = Api.SciterEval(WindowHandle, script, (uint) script.Length, out var retValue);
			value = result ? SciterValue.Attach(retValue) : SciterValue.Null;
			return result;
		}

		/// <summary>
		/// Posts a message to the UI thread to invoke the given Action. This methods returns immediately, does not wait for the message processing.
		/// </summary>
		/// <param name="what">The delegate which will be invoked</param>
		public void InvokePost(Action what)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(what != null);

			var handle = GCHandle.Alloc(what);

			HostCallbackRegistry.Instance.TryGetValue(this, out var callbackWrapper);
			
			PostNotificationInternal(new IntPtr(callbackWrapper?.InvokeNotification ?? 0), GCHandle.ToIntPtr(handle));
		}

		/// <summary>
		/// Sends a message to the UI thread to invoke the given Action. This methods waits for the message processing until timeout is exceeded.
		/// </summary>
		/// <param name="what">The delegate which will be invoked</param>
		/// <param name="timeout"></param>
		public void InvokeSend(Action what, uint timeout = 3000)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			Debug.Assert(what != null);
			Debug.Assert(timeout > 0);

			var handle = GCHandle.Alloc(what);
			
			HostCallbackRegistry.Instance.TryGetValue(this, out var callbackWrapper);
			
			PostNotificationInternal(new IntPtr(callbackWrapper?.InvokeNotification ?? 0), GCHandle.ToIntPtr(handle), timeout);
		}

		//TODO: @wdcossey - Clean this up!
		internal async Task ConnectToInspectorInternalAsync()
		{
			var inspectorProc = "inspector";
			var processes = Process.GetProcessesByName(inspectorProc);
			if (!processes.Any())
			{
				var value = EvalScriptInternal(@"view.msgbox { type:#warning, " +
				                               "title:\"Inspector\", " +
				                               "content:\"Inspector process is not running. You should run it before calling ConnectToInspector()\", " +
				                               "buttons:#ok" +
				                               "};");
				return;
			}

			await Task.Delay(100);

#if SCITER_JS
			throw new NotImplementedException();
#else
			this.EvalScriptInternal("view.connectToInspector()");
#endif

#if OSX
			var app_inspector = AppKit.NSRunningApplication.GetRunningApplications("terrainformatica.inspector");
			if(app_inspector.Length==1)
				app_inspector[0].Activate(AppKit.NSApplicationActivationOptions.ActivateAllWindows);
#endif
		}

		/*
		/// <summary>
		/// Runs the inspector process, waits 1 second, and calls view.connectToInspector() to inspect your page.
		/// (Before everything it kills any previous instance of the inspector process)
		/// </summary>
		/// <param name="inspector_exe">Path to the inspector executable, can be an absolute or relative path.</param>
		internal void ConnectToInspectorInternal(string inspector_exe)
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

		#region Notification

		internal IntPtr PostNotificationInternal(IntPtr wparam, IntPtr lparam, uint timeout = 0)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			return Api.SciterPostCallback(WindowHandle, wparam, lparam, timeout);
		}

		#endregion

		#region Behavior Factory

#if NETCORE
		public SciterHost SetBehaviorResolver(INamedBehaviorResolver namedBehaviorResolver)
		{
			_namedBehaviorResolver = namedBehaviorResolver;
			return this;
		}
#endif
		
		//TODO: Remove this!
		[Obsolete("This method is obsolete")]
		internal void RegisterBehaviorHandlerInternal(Type eventHandlerType, string behaviorName = null)
		{
			var entry = new EventHandlerRegistry(type: eventHandlerType, name: behaviorName);
			_behaviorMap[entry.Name] = entry;
		}

		//TODO: Remove this!
		[Obsolete("This method is obsolete")]
		internal void RegisterBehaviorHandlerInternal<TType>(string behaviorName = null)
			where TType : SciterEventHandler
		{
			var entry = new EventHandlerRegistry(type: typeof(TType), name: behaviorName);
			_behaviorMap[entry.Name] = entry;
		}

		//TODO: Remove this!
		[Obsolete("This method is obsolete")]
		internal void RegisterBehaviorHandlerInternal<THandler>(THandler eventHandler, string behaviorName = null)
			where THandler : SciterEventHandler
		{
			var entry = new EventHandlerRegistry(eventHandler: eventHandler, name: behaviorName ?? eventHandler.Name);
			_behaviorMap[entry.Name] = entry;
		}

		//TODO: Remove this!
		[Obsolete("This method is obsolete")]
		public void RegisterBehaviorHandlerInternal<THandler>(Func<THandler> eventHandlerFunc,
			string behaviorName = null)
			where THandler : SciterEventHandler
		{
			var eventHandler = eventHandlerFunc?.Invoke();
			RegisterBehaviorHandlerInternal(eventHandler, behaviorName ?? eventHandler?.Name);
		}

		#endregion

		// Properties
		public SciterElement RootElement
		{
			get
			{
				Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
				Api.SciterGetRootElement(WindowHandle, out var heRoot);
				return SciterElement.Attach(heRoot);
			}
		}

		public SciterElement FocusElement
		{
			get
			{
				Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
				Api.SciterGetFocusElement(WindowHandle, out var heFocus);
				return SciterElement.Attach(heFocus);
			}
		}

		#region Notification handler

		/*private uint NotificationHandler(IntPtr ptrNotification, IntPtr callbackParam)
		{
			var callbackNotification = Marshal.PtrToStructure<SciterXDef.SCITER_CALLBACK_NOTIFICATION>(ptrNotification);

			switch (callbackNotification.code)
			{
				case SciterXDef.SCITER_CALLBACK_CODE.SC_LOAD_DATA:
					var sld = Marshal.PtrToStructure<SciterXDef.SCN_LOAD_DATA>(ptrNotification);
					return (uint) OnLoadData(sender: this, args: sld.ToEventArgs());

				case SciterXDef.SCITER_CALLBACK_CODE.SC_DATA_LOADED:
					var sdl = Marshal.PtrToStructure<SciterXDef.SCN_DATA_LOADED>(ptrNotification);
					OnDataLoaded(sender: this, args: sdl.ToEventArgs());
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_ATTACH_BEHAVIOR:
					var attachBehavior = Marshal.PtrToStructure<SciterXDef.SCN_ATTACH_BEHAVIOR>(ptrNotification);
					var behaviorName = Marshal.PtrToStringAnsi(attachBehavior.behaviorName);
					var attachResult = OnAttachBehavior(new SciterElement(attachBehavior.elem), behaviorName,
						out var eventHandler);

					if (!attachResult)
						return 0;

					var proc = eventHandler.EventProc;
					var ptrProc = Marshal.GetFunctionPointerForDelegate(proc);

					var elementProcOffset =
						Marshal.OffsetOf<SciterXDef.SCN_ATTACH_BEHAVIOR>(nameof(SciterXDef.SCN_ATTACH_BEHAVIOR
							.elementProc));
					var elementTagOffset =
						Marshal.OffsetOf<SciterXDef.SCN_ATTACH_BEHAVIOR>(nameof(SciterXDef.SCN_ATTACH_BEHAVIOR
							.elementTag));
					Marshal.WriteIntPtr(ptrNotification, elementProcOffset.ToInt32(), ptrProc);
					Marshal.WriteInt32(ptrNotification, elementTagOffset.ToInt32(), 0);
					return 1;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_ENGINE_DESTROYED:

					HostCallbackRegistry.Instance.TryRemove(this, out _);

					if (_windowEventHandler != null)
					{
						Api.SciterWindowDetachEventHandler(WindowHandle, _windowEventHandler.EventProc, IntPtr.Zero);
						_windowEventHandler = null;
					}

					var engineDestroyed = Marshal.PtrToStructure<SciterXDef.SCN_ENGINE_DESTROYED>(ptrNotification);

					OnEngineDestroyed(sender: this, args: new EngineDestroyedArgs(engineDestroyed.hwnd, engineDestroyed.code));
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_POSTED_NOTIFICATION:
					var spn = Marshal.PtrToStructure<SciterXDef.SCN_POSTED_NOTIFICATION>(ptrNotification);
					var lReturnPtr = IntPtr.Zero;
					if (spn.wparam.ToInt32() == INVOKE_NOTIFICATION)
					{
						var handle = GCHandle.FromIntPtr(spn.lparam);
						var cbk = (Action) handle.Target;
						cbk?.Invoke();
						handle.Free();
					}
					else
					{
						lReturnPtr = OnPostedNotification(spn.wparam, spn.lparam);
					}

					var lReturnOffset =
						Marshal.OffsetOf<SciterXDef.SCN_POSTED_NOTIFICATION>(nameof(SciterXDef.SCN_POSTED_NOTIFICATION
							.lreturn));
					Marshal.WriteIntPtr(ptrNotification, lReturnOffset.ToInt32(), lReturnPtr);
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_GRAPHICS_CRITICAL_FAILURE:
					var cgf = Marshal.PtrToStructure<SciterXDef.SCN_GRAPHICS_CRITICAL_FAILURE>(ptrNotification);
					OnGraphicsCriticalFailure(handle: cgf.hwnd, code: cgf.code);
					return 0;

				default:
					Debug.Assert(false);
					break;
			}

			return 0;
		}*/

		#endregion

		#region Overridables

		internal LoadResult OnLoadDataInternal(object sender, LoadDataArgs args)
		{
			return OnLoadData(sender, args);
		}

		protected virtual LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			Debug.Assert(WindowHandle != IntPtr.Zero, "Call SciterHost.SetupWindow() first");
			
			if (!AttachedArchives.TryGetValue(args.Uri.Scheme, out var archive)) 
				return LoadResult.Ok;
			
			archive.TryGetItem(args.Uri,
				(result) =>
				{
					if (result.IsSuccessful)
						Api.SciterDataReady(WindowHandle, result.Path, result.Data, (uint) result.Size);
				});

			return LoadResult.Ok;
		}

		internal void OnDataLoadedInternal(object sender, DataLoadedArgs args)
		{
			OnDataLoaded(sender, args);
		}

		protected virtual void OnDataLoaded(object sender, DataLoadedArgs args) { }

		internal bool OnAttachBehaviorInternal(SciterElement element, string behaviorName,
			out SciterEventHandler eventHandler)
		{
			return OnAttachBehavior(element, behaviorName, out eventHandler);
		}
		
		protected virtual bool OnAttachBehavior(SciterElement element, string behaviorName,
			out SciterEventHandler eventHandler)
		{
			// returns a new SciterEventHandler if the behaviorName was registered by a previous RegisterBehaviorHandler() call

#if NETCORE
			if (_namedBehaviorResolver?.ContainsKey(behaviorName) == true)
#else
			if (_behaviorMap.ContainsKey(behaviorName))
#endif
			{
#if NETCORE
				eventHandler = _namedBehaviorResolver.GetBehaviorHandler(behaviorName)
#else
				eventHandler = _behaviorMap[behaviorName].Create(this)
#endif
					?.SetHost(this);//_behaviorMap[behaviorName].Create(this);
				return true;
			}

			eventHandler = null;
			return false;
		}

		internal void OnEngineDestroyedInternal(object sender, EngineDestroyedArgs args)
		{
			OnEngineDestroyed(sender, args);
		}

		protected virtual void OnEngineDestroyed(object sender, EngineDestroyedArgs args) { }

		internal IntPtr OnPostedNotificationInternal(IntPtr wparam, IntPtr lparam)
		{
			return OnPostedNotification(wparam, lparam);
		}
		
		protected virtual IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
		{
			return IntPtr.Zero;
		}

		internal void OnGraphicsCriticalFailureInternal(IntPtr handle, uint code)
		{
			OnGraphicsCriticalFailure(handle, code);
		}
		
		protected virtual void OnGraphicsCriticalFailure(IntPtr handle, uint code) { }

		#endregion

		private void ReleaseUnmanagedResources()
		{
			HostCallbackRegistry.Instance.TryRemove(this, out _);
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

		~SciterHost()
		{
			Dispose(false);
		}
	}
}