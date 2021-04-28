using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore
{
    internal class HostCallbackRegistry : ConcurrentDictionary<SciterHost, HostCallbackWrapper>
    {
        private static readonly Lazy<HostCallbackRegistry>
            Lazy =
                new Lazy<HostCallbackRegistry>
                    (() => new HostCallbackRegistry());

        public static HostCallbackRegistry Instance => Lazy.Value;

        private HostCallbackRegistry() { }
        
        //private static ConcurrentDictionary<SciterHost, SciterXDef.SCITER_HOST_CALLBACK> Registry { get; } =
        //    new ConcurrentDictionary<SciterHost, SciterXDef.SCITER_HOST_CALLBACK>();
        //
        //internal static SciterXDef.SCITER_HOST_CALLBACK Get(SciterHost host)
        //{
        //    return Registry.TryGetValue(host, out var result) ? result : null;
        //}
        //
        //internal static SciterXDef.SCITER_HOST_CALLBACK Set(SciterHost host, 
        //    SciterXDef.SCITER_HOST_CALLBACK @callback)
        //{
        //    if (Registry.ContainsKey(host))
        //        return Get(host);
        //
        //    Registry.TryAdd(host, @callback);
        //
        //    return @callback;
        //}
        //
        //internal static void Remove(SciterHost host)
        //{
        //    if (Registry.ContainsKey(host))
        //        Registry.TryRemove(host, out _);
        //}
    }

    public class HostCallbackWrapper
    {
	    public int InvokeNotification = 0x8206241;
	    
	    private readonly SciterHost _sciterHost;
	    private volatile SciterXDef.SCITER_HOST_CALLBACK _callbackDelegate;

	    public HostCallbackWrapper(SciterHost sciterHost)
	    {
		    _sciterHost = sciterHost;
		    _callbackDelegate = NotificationHandler;
	    }

	    public SciterXDef.SCITER_HOST_CALLBACK CallbackDelegate => _callbackDelegate;

	    private uint NotificationHandler(IntPtr ptrNotification, IntPtr callbackParam)
		{
			var callbackNotification = Marshal.PtrToStructure<SciterXDef.SCITER_CALLBACK_NOTIFICATION>(ptrNotification);

			switch (callbackNotification.code)
			{
				case SciterXDef.SCITER_CALLBACK_CODE.SC_LOAD_DATA:
					var sld = Marshal.PtrToStructure<SciterXDef.SCN_LOAD_DATA>(ptrNotification);
					return (uint) _sciterHost.OnLoadDataInternal(sender: this, args: sld.ToEventArgs());

				case SciterXDef.SCITER_CALLBACK_CODE.SC_DATA_LOADED:
					var sdl = Marshal.PtrToStructure<SciterXDef.SCN_DATA_LOADED>(ptrNotification);
					_sciterHost.OnDataLoadedInternal(sender: this, args: sdl.ToEventArgs());
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_ATTACH_BEHAVIOR:
					var attachBehavior = Marshal.PtrToStructure<SciterXDef.SCN_ATTACH_BEHAVIOR>(ptrNotification);
					var behaviorName = Marshal.PtrToStringAnsi(attachBehavior.behaviorName);
					var attachResult = _sciterHost.OnAttachBehaviorInternal(SciterElement.Attach(attachBehavior.elem), behaviorName,
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

					HostCallbackRegistry.Instance.TryRemove(this._sciterHost, out _);

					if (_sciterHost.WindowEventHandler != null)
					{
						Sciter.SciterApi.SciterWindowDetachEventHandler(_sciterHost.WindowHandle, _sciterHost.WindowEventHandler.EventProc, IntPtr.Zero);
						_sciterHost.WindowEventHandler = null;
					}

					var engineDestroyed = Marshal.PtrToStructure<SciterXDef.SCN_ENGINE_DESTROYED>(ptrNotification);

					_sciterHost.OnEngineDestroyedInternal(sender: this, args: new EngineDestroyedArgs(engineDestroyed.hwnd, engineDestroyed.code));
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_POSTED_NOTIFICATION:
					var spn = Marshal.PtrToStructure<SciterXDef.SCN_POSTED_NOTIFICATION>(ptrNotification);
					var lReturnPtr = IntPtr.Zero;
					if (spn.wparam.ToInt32() == InvokeNotification)
					{
						var handle = GCHandle.FromIntPtr(spn.lparam);
						var cbk = (Action) handle.Target;
						cbk?.Invoke();
						handle.Free();
					}
					else
					{
						lReturnPtr = _sciterHost.OnPostedNotificationInternal(spn.wparam, spn.lparam);
					}

					var lReturnOffset =
						Marshal.OffsetOf<SciterXDef.SCN_POSTED_NOTIFICATION>(nameof(SciterXDef.SCN_POSTED_NOTIFICATION
							.lreturn));
					Marshal.WriteIntPtr(ptrNotification, lReturnOffset.ToInt32(), lReturnPtr);
					return 0;

				case SciterXDef.SCITER_CALLBACK_CODE.SC_GRAPHICS_CRITICAL_FAILURE:
					var cgf = Marshal.PtrToStructure<SciterXDef.SCN_GRAPHICS_CRITICAL_FAILURE>(ptrNotification);
					_sciterHost.OnGraphicsCriticalFailureInternal(handle: cgf.hwnd, code: cgf.code);
					return 0;

				default:
					Debug.Assert(false);
					break;
			}

			return 0;
		}

    }
}