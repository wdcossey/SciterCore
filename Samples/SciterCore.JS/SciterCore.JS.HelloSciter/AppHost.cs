using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterCore.JS.HelloSciter.Behaviors;

namespace SciterCore.JS.HelloSciter
{
	
	//[SciterHostEventHandler(typeof(AppEventHandler))]
	//[SciterHostWindow(null, 800, 600, "SciterCore.JS::NetCore")]
	//[SciterHostArchive]
	//[SciterHostBehaviorHandler(typeof(RuntimeInformationBehavior))]
	public class AppHost : SciterArchiveHost
	{
		private readonly ILogger<AppHost> _logger;

		public AppHost(ILogger<AppHost> logger)
		{
			_logger = logger;
		}
		
		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			_logger?.LogTrace(
				"{NameOfMethod}({NameOfArgs}: [{Args}])", nameof(OnLoadData), nameof(args), args);
			
			return base.OnLoadData(sender: sender, args: args);
		}
		
		protected override bool OnAttachBehavior(SciterElement element, string behaviorName,
			out SciterEventHandler eventHandler)
		{
			_logger?.LogTrace("{NameOfMethod}({ElementName}: {ElementTag} [{ElementId}]; {BehaviorName}: {BehaviorNameValue})", nameof(OnAttachBehavior),
				nameof(element), element.Tag, element.UniqueId, nameof(behaviorName), behaviorName);
			
			return base.OnAttachBehavior(element, behaviorName, out eventHandler);
		}

		protected override void OnDataLoaded(object sender, DataLoadedArgs args)
		{
			_logger?.LogTrace(
				"{NameOfMethod}({NameOfArgs}: [{Args}])", nameof(OnDataLoaded), nameof(args), args);

			base.OnDataLoaded(sender, args);
		}

		protected override void OnEngineDestroyed(object sender, EngineDestroyedArgs args)
		{
			_logger?.LogTrace("{NameOfMethod}({NameOfArgs}: [{ArgsValue}])",
				nameof(OnEngineDestroyed), nameof(args), args);
			
			base.OnEngineDestroyed(sender, args);
		}

		protected override IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
		{
			_logger?.LogTrace("{NameOfMethod}()", nameof(OnPostedNotification));

			return base.OnPostedNotification(wparam, lparam);
		}
	}
}