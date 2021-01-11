using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using SciterCore;
using SciterCore.Interop;
using SciterTest.NetCore.Behaviors;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore
{
	
	public class CustomWindow : SciterWindow
	{
		public CustomWindow()
		{
			var creationFlags =
			//SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_POPUP |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS |
			SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY;
		
			var frame = new SciterRectangle(800, 600);
			CreateWindow(frame: frame, creationFlags: creationFlags, parent: IntPtr.Zero);
				
			//CreateWindow(800, 600)
			//	.CenterTopLevelWindow()
			//	.SetTitle("SciterCore::NetCore::Playground");
		}
	}
	
	public class CustomHost : BaseHost
	{
		// ReSharper disable once SuggestBaseTypeForParameter
		public CustomHost(ILogger<CustomHost> logger, CustomWindow window, CustomHostEventHandler hostEventHandler)
			: base(logger, window)
		{
			this.AttachEventHandler(hostEventHandler);
				//.RegisterBehaviorHandler<VirtualTreeBehavior>()
				//.RegisterBehaviorHandler<DragDropBehavior>()
				//.RegisterBehaviorHandler<CustomWindowEventHandler>();

				this.LoadPage("index.html",
				onFailed: (_, __) => throw new InvalidOperationException("Unable to load the requested page."));

#if DEBUG
			this.Window.OnWindowShow += (sender, args) =>
			{
				var treeElement = window.RootElement.SelectFirst("widget#tree");
				treeElement.AttachEventHandler<VirtualTreeBehavior>();
				
				this.ConnectToInspector();
			};
#endif
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	public class CustomHostEventHandler : SciterEventHandler
	{
		private readonly ILogger _logger;

		public CustomHostEventHandler(ILogger<CustomHostEventHandler> logger)
		{
			_logger = logger;
		}

		public void SynchronousFunction()
		{
			// _logger.LogInformation($"{nameof(SynchronousFunction)} was executed!");
		}
		
		protected override EventGroups SubscriptionsRequest(SciterElement element)
		{
			return EventGroups.HandleAll;
		}

		protected override void Attached(SciterElement element)
		{
			//_logger?.LogDebug($"{nameof(Attached)}");
			base.Attached(element);
		}

		protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
		{
			//_logger?.LogDebug($"{nameof(OnMethodCall)}: {nameof(methodId)}: {methodId}");
			return base.OnMethodCall(element, methodId);
		}

		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			//_logger?.LogDebug($"{nameof(OnScriptCall)}: {nameof(method)}: {method.Name}");
			return base.OnScriptCall(element, method, args);
		}

		protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement,
			BehaviorEvents type, IntPtr reason, SciterValue data, string eventName)
		{
			//_logger?.LogDebug($"{nameof(OnEvent)}: {nameof(type)}: {type}");
			return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
		}

		protected override bool OnDataArrived(SciterElement element, SciterBehaviors.DATA_ARRIVED_PARAMS prms)
		{
			//_logger?.LogDebug($"{nameof(OnDataArrived)}: {nameof(prms)}: {prms.uri}");
			return base.OnDataArrived(element, prms);
		}
	}
	
}