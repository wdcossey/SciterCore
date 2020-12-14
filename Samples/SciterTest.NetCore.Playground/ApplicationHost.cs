using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciterCore;
using SciterCore.Interop;
using SciterTest.NetCore.Behaviors;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore
{
	public class ApplicationHost : BaseHost
	{
		public ApplicationHost(ILogger<ApplicationHost> logger, ApplicationWindow wnd, HostEventHandler hostEventHandler)
			: base(logger, wnd)
		{
			var host = this;
			host
				.AttachEventHandler(hostEventHandler)
				.RegisterBehaviorHandler<VirtualTreeBehavior>()
				.RegisterBehaviorHandler<DragDropBehavior>()
				.RegisterBehaviorHandler<CustomWindowEventHandler>();

			host.LoadPage("vtree.html",
				onFailed: (sciterHost, window) => throw new InvalidOperationException("Unable to load the requested page."));

#if DEBUG
			host.Window.OnWindowShow += (sender, args) =>
			{
				var treeElement = wnd.RootElement.SelectFirst("widget#tree");
				
				treeElement.AttachEventHandler<VirtualTreeBehavior>();
				
				var button = wnd.RootElement.SelectFirst("#new-dialog");
				
				button.FireEvent(new SciterBehaviorArgs()
				{
					Command = BehaviorEvents.ButtonClick,
					Target = button,
					Source = button,
					Name = "Hello",
					Data = SciterValue.Create("World!"),
					
				});
				
				host.ConnectToInspector();
			};
#endif
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	public class HostEventHandler : SciterEventHandler
	{
		private readonly ILogger<HostEventHandler> _logger;
		private readonly IServiceProvider _provider;

		public HostEventHandler(ILogger<HostEventHandler> logger, IServiceProvider provider)
		{
			_logger = logger;
			_provider = provider;
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
			_logger?.LogDebug($"{nameof(OnEvent)}: {nameof(type)}: {type}; {nameof(eventName)}: {eventName}; {nameof(data)}: {data.AsString()}");
			return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
		}

		protected override bool OnDataArrived(SciterElement element, SciterBehaviors.DATA_ARRIVED_PARAMS prms)
		{
			//_logger?.LogDebug($"{nameof(OnDataArrived)}: {nameof(prms)}: {prms.uri}");
			return base.OnDataArrived(element, prms);
		}

		public void TestNewDialog()
		{
			var scope = _provider.CreateScope();
			{
				scope.ServiceProvider.GetRequiredService<CustomHost>().Window.Show();
			}
			
			GC.Collect();
		}

		public void PostNotification()
		{
			this.Host.PostNotification(IntPtr.Zero, IntPtr.Zero, 0U);
		}
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.rs/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	public class BaseHost : SciterHost
	{
		private readonly ILogger _logger;
		private readonly ISciterApi _api = Sciter.Api;
		private readonly SciterArchive _archive = new SciterArchive();


		public BaseHost(ILogger logger, SciterWindow window)
			: base(window)
		{
			_logger = logger;
			_archive.Open();
		}

		public SciterHost LoadPage(string page, Action<SciterHost, SciterWindow> onCompleted = null, Action<SciterHost, SciterWindow> onFailed = null)
		{
#if DEBUG
			var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var path = Path.Combine(location ?? string.Empty, "wwwroot", page);
			var uri = new Uri(path, UriKind.Absolute);

			Debug.Assert(uri.IsFile);
			Debug.Assert(File.Exists(uri.AbsolutePath));
#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
#endif 

			if (Window.TryLoadPage(uri: uri))
				onCompleted?.Invoke(this, Window);
			else
				onFailed?.Invoke(this, Window);

			return this;
		}

		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			//_logger?.LogDebug(args.Uri.ToString());
			
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (res) => 
			{
				if (res.IsSuccessful)
					_api.SciterDataReady(Window.Handle, res.Path, res.Data, (uint) res.Size);
			});

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sender: sender, args: args);
		}

		protected override bool OnAttachBehavior(SciterElement element, string behaviorName, out SciterEventHandler eventHandler)
		{
			//_logger?.LogDebug($"{nameof(OnAttachBehavior)}: {nameof(element)}: {element.Tag} ({element.UniqueId}); {nameof(behaviorName)}: {behaviorName}");
			return base.OnAttachBehavior(element, behaviorName, out eventHandler);
		}

		protected override void OnDataLoaded(object sender, DataLoadedArgs args)
		{
			base.OnDataLoaded(sender, args);
		}

		protected override void OnEngineDestroyed(object sender, EngineDestroyedArgs args)
		{
			//_logger?.LogDebug(args.Code.ToString());
			base.OnEngineDestroyed(sender, args);
		}

		protected override IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
		{
			return base.OnPostedNotification(wparam, lparam);
		}
		
		
	}
}