using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using HelloSciterJS.Behaviors;
using Microsoft.Extensions.Logging;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore.JS.HelloSciter
{
	
	[SciterHostEventHandler(typeof(HostEventHandler))]
	[SciterHostWindow(null, 800, 600, "SciterCore.JS::NetCore")]
	[SciterHostArchive]
	[SciterHostBehaviorHandler(typeof(RuntimeInformationBehavior))]
	public class ApplicationHost : SciterArchiveHost
	{
		private readonly ILogger<ApplicationHost> _logger;

		public ApplicationHost(ILogger<ApplicationHost> logger)
		{
			_logger = logger;

			OnCreated += (_, args) =>
			{
				args.Window.CenterWindow();

				string indexPage = null;

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					indexPage = "index-win.html";

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					indexPage = "index-lnx.html";

				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					indexPage = "index-macos.html";

				if (string.IsNullOrWhiteSpace(indexPage))
					throw new PlatformNotSupportedException();

#if DEBUG
				//Used for development/debugging (load file(s) from the disk)
				var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				var path = Path.Combine(location ?? string.Empty, "wwwroot", indexPage);
				var uri = new Uri(path, UriKind.Absolute);
				Debug.Assert(uri.IsFile);
				Debug.Assert(File.Exists(uri.AbsolutePath));
#else
				var uri = new Uri(baseUri: this.Archive.Uri, startPage);
#endif
				args.Window.LoadPage(uri);
			};
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

	public class HostEventHandler : SciterEventHandler
	{
		private readonly ILogger<HostEventHandler> _logger;

		public HostEventHandler(ILogger<HostEventHandler> logger)
		{
			_logger = logger;
		}
		
		public Task StackTrace(SciterElement element, SciterValue onCompleted)
		{
			var stackTrace = new StackTrace(true);
			var stackFrame = stackTrace.GetFrame(0);
			
			var value = SciterValue.Create(
				new
				{
					MethodName = stackFrame?.GetMethod()?.Name,
					Parameters = stackFrame?.GetMethod()?.GetParameters().Select(s => new { s.Name, s.Position, Type = s.ParameterType.Name}),
					FileUri = new Uri(stackFrame?.GetFileName() ?? string.Empty)?.AbsoluteUri,
					FileName = Path.GetFileName(stackFrame?.GetFileName()),
					LineNumber = stackFrame?.GetFileLineNumber(),
					ColumnNumber = stackFrame?.GetFileColumnNumber()
				});

			onCompleted.Invoke(value);
			
			return Task.CompletedTask;
		}

		private ManualResetEventSlim _callMeBackResetEvent;
		
		public async Task CallMeBack(SciterElement element, SciterValue value, SciterValue onProgress, SciterValue onCompleted)
		{
			_callMeBackResetEvent = new ManualResetEventSlim(false);

			for (var i = 0; i < 201; i++)
			{
				if (_callMeBackResetEvent.IsSet)
					break;

				//Simulates a delay
				await Task.Delay(10);
				var val = i / 200d * 100;
				
				onProgress.Invoke(SciterValue.Create(i), SciterValue.Create(val));
			}

			onCompleted.Invoke(SciterValue.Create($"You have {(!_callMeBackResetEvent.IsSet ? "successfully completed" : "cancelled")} your task!"), SciterValue.Create(!_callMeBackResetEvent.IsSet));
		}
		
		public Task CancelCallMeBack()
		{
			_callMeBackResetEvent?.Set();
			return Task.CompletedTask;
		}
		
		[SciterFunctionName("breakMe")]
		[SciterCallbackWrapper]
		public Task ThrowException(SciterValue numerator, SciterValue denominator)
		{
			try
			{ 
				//This will purposely throw an exception!
				var value = numerator.AsInt32() / denominator.AsInt32();
				
				return Task.FromResult(SciterValue.Create(value));
			}
			catch (Exception ex)
			{
				_logger.LogError(exception: ex, message: "{Message}", ex.Message);
				throw;
			}
		}

		public void SynchronousFunction()
		{
			_logger?.LogInformation("{NameOfMethod}() was executed!", nameof(SynchronousFunction));
		}

		public void SynchronousFunction(SciterElement element, SciterValue[] args)
		{
			_logger?.LogInformation("{NameOfMethod}({Parameters}) was executed!", nameof(SynchronousFunction),
				string.Join(", ", args.Select(s => $"{Array.IndexOf(args, s)}: {s.AsInt32()}")));
		}

		public void SynchronousFunction(SciterElement element, SciterValue arg1, SciterValue arg2, SciterValue arg3,
			SciterValue arg4, SciterValue arg5)
		{
			_logger?.LogInformation(
				"{NameOfMethod}(arg1: {Arg1}; arg2: {Arg2}; arg3: {Arg3}; arg4: {Arg4}; arg5: {Arg5}) was executed!",
				nameof(SynchronousFunction), arg1.AsInt32(), arg2.AsInt32(), arg3.AsInt32(), arg4.AsInt32(),
				arg5.AsInt32());
		}

		public async Task AsynchronousFunction()
		{
			await Task.Delay(TimeSpan.FromSeconds(5));
			
			_logger?.LogInformation("{NameOfMethod}() was executed!", nameof(AsynchronousFunction));
		}

		[SciterFunctionName("eval")]
		public SciterValue EvaluateScript(SciterValue input)
		{
			var result = Host.EvalScript($"{input.AsString()}");
			return result;
		}
		
		protected override EventGroups SubscriptionsRequest(SciterElement element)
		{
			return EventGroups.HandleAll;
		}

		// ReSharper disable once RedundantOverriddenMember
		protected override bool OnMouse(SciterElement element, MouseArgs args)
		{
			//Console.WriteLine($"{args.ButtonState}| {args.Cursor} | {args.Event} | {args.DragMode}");
			return base.OnMouse(element, args);
		}

		// ReSharper disable once RedundantOverriddenMember
		protected override bool OnKey(SciterElement element, KeyArgs args)
		{
			//Console.WriteLine($"{args.Event} | {args.KeyboardState} | {(char)args.KeyCode}");
			return base.OnKey(element, args);
		}

		// ReSharper disable once RedundantOverriddenMember
		protected override bool OnFocus(SciterElement element, FocusArgs args)
		{
			//Console.WriteLine($"{args.Event} | {args.Cancel} | {args.IsMouseClick}");
			return base.OnFocus(element, args);
		}

		protected override void Attached(SciterElement element)
		{
			_logger?.LogTrace("{NameOfMethod}()", nameof(Attached));
			base.Attached(element);
		}

		protected override ScriptEventResult OnScriptCall(SciterElement element, string methodName, SciterValue[] args)
		{
			_logger?.LogTrace("{NameOfMethod}(methodName: \"{MethodName}\")", nameof(OnScriptCall), methodName);
			return base.OnScriptCall(element, methodName, args);
		}

		// ReSharper disable once RedundantOverriddenMember
		protected override bool OnGesture(SciterElement element, GestureArgs args)
		{
			//Console.WriteLine($"{args}");
			//if (args.Event == GestureEvent.Request)
			//	return true;
			
			return base.OnGesture(element, args);
		}

		protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
		{
			_logger?.LogTrace("{NameOfMethod}(methodId: \"{MethodId}\")", nameof(OnMethodCall), methodId);
			return base.OnMethodCall(element, methodId);
		}

		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			_logger?.LogTrace("{NameOfMethod}(method: \"{Method}\")", nameof(OnScriptCall), method.Name);
			return base.OnScriptCall(element, method, args);
		}

		protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement,
			BehaviorEvents type, IntPtr reason, SciterValue data, string eventName)
		{
			if (type == BehaviorEvents.DocumentReady)
			{
				//Host.CallFunction("Init", SciterValue.Create(Sciter.SciterApi.SciterVersion().ToString()));
				//Host.ConnectToInspector();
			}

			_logger?.LogTrace(
				"{NameOfMethod}(sourceElement: {SourceElement}; targetElement: {TargetElement}; type: {Type}; data: {DataString}; eventName: {EventName})",
				nameof(OnEvent), sourceElement?.Tag, targetElement?.Tag, type, data.AsString(), eventName);
			
			return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
		}

		protected override bool OnDataArrived(SciterElement element, SciterBehaviors.DATA_ARRIVED_PARAMS prms)
		{
			_logger?.LogTrace(
				"{NameOfMethod}(element: {Element}; prms: {Params})", nameof(OnDataArrived),
				element?.Tag, prms);

			return base.OnDataArrived(element, prms);
		}
	}
}