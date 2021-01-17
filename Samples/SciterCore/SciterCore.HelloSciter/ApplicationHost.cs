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
using SciterTest.NetCore.Behaviors;

namespace SciterCore.HelloSciter
{
	[SciterHostEventHandler(typeof(HostEventHandler))]
	[SciterHostWindow("this://app/index.html", 800, 600, "SciterCore::Hello")]
	[SciterHostArchive("this://app/")]
	[SciterHostBehaviorHandler(typeof(DragDropBehavior))]
	public class ApplicationHost : SciterArchiveHost
	{
		private readonly ILogger _logger;

		// ReSharper disable once SuggestBaseTypeForParameter
		public ApplicationHost(ILogger<ApplicationHost> logger)
			: base()
		{
			_logger = logger;
			OnCreated += (sender, args) =>
			{
				args.Window.CenterWindow();
			};
		}

		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			_logger?.LogDebug(args.Uri.ToString());
			return base.OnLoadData(sender: sender, args: args);
		}

		protected override bool OnAttachBehavior(SciterElement element, string behaviorName, out SciterEventHandler eventHandler)
		{
			_logger?.LogDebug($"{nameof(OnAttachBehavior)}: {nameof(element)}: {element.Tag} ({element.UniqueId}); {nameof(behaviorName)}: {behaviorName}");
			return base.OnAttachBehavior(element, behaviorName, out eventHandler);
		}

		protected override void OnDataLoaded(object sender, DataLoadedArgs args)
		{
			base.OnDataLoaded(sender, args);
		}

		protected override void OnEngineDestroyed(object sender, EngineDestroyedArgs args)
		{
			_logger?.LogDebug(args.Code.ToString());
			base.OnEngineDestroyed(sender, args);
		}

		protected override IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
		{
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
					FileUri = new Uri(stackFrame?.GetFileName())?.AbsoluteUri,
					FileName = Path.GetFileName(stackFrame?.GetFileName()),
					LineNumber = stackFrame?.GetFileLineNumber(),
					ColumnNumber = stackFrame?.GetFileColumnNumber()
				});

			onCompleted.Invoke(value);
			
			return Task.CompletedTask;
		}
		
		public Task GetRuntimeInfo(SciterElement element, SciterValue onCompleted, SciterValue onError)
		{
			try
			{
				var value = SciterValue.Create(
					new {
						FrameworkDescription = RuntimeInformation.FrameworkDescription,
						ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
						OSArchitecture = RuntimeInformation.OSArchitecture.ToString(),
						OSDescription = RuntimeInformation.OSDescription,
						SystemVersion = RuntimeEnvironment.GetSystemVersion()
					});
			
				onCompleted.Invoke(value);
			}
			catch (Exception e)
			{
				onError.Invoke(SciterValue.MakeError(e.Message));
			}

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
				onProgress.Invoke(SciterValue.Create(i), SciterValue.Create(i / 200d * 100));
			}
			
			onCompleted.Invoke(SciterValue.Create($"You have {(!_callMeBackResetEvent.IsSet ? "successfully completed" : "cancelled")} your task!"), SciterValue.Create(!_callMeBackResetEvent.IsSet));
		}
		
		public Task CancelCallMeBack()
		{
			_callMeBackResetEvent?.Set();
			return Task.CompletedTask;
		}
		
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
				_logger.LogError(exception: ex, message: ex.Message);
				throw;
			}
		}

		public void SynchronousFunction()
		{
			_logger.LogInformation($"{nameof(SynchronousFunction)} was executed!");
		}

		public void SynchronousArgumentFunction(SciterElement element, SciterValue[] args)
		{
			_logger.LogInformation($"{nameof(SynchronousArgumentFunction)} was executed!\n\t{string.Join(",", args.Select(s => s.AsInt32()))}");
		}

		public void SynchronousArgumentsFunction(SciterElement element, SciterValue arg1, SciterValue arg2, SciterValue arg3, SciterValue arg4, SciterValue arg5)
		{
			_logger.LogInformation($"{nameof(SynchronousArgumentsFunction)} was executed!\n\t{arg1.AsInt32()},{arg2.AsInt32()},{arg3.AsInt32()},{arg4.AsInt32()},{arg5.AsInt32()}");
		}
		
		public async Task AsynchronousFunction()
		{
			await Task.Delay(TimeSpan.FromSeconds(2));

			//var value = _host.EvalScript(@"view.msgbox { type:#question, " +
            //                             			"content:\"Is anybody out there?\", " +
            //                                        "buttons:[" +
            //                             			"{id:#yes,text:\"Yes\",role:\"default-button\"}," +
            //                             			"{id:#no,text:\"No\",role:\"cancel-button\"}]" +
            //                                        "};");
            _logger.LogInformation($"{nameof(AsynchronousFunction)} was executed!");
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

		protected override bool OnMouse(SciterElement element, MouseArgs args)
		{
			//Console.WriteLine($"{args.ButtonState}| {args.Cursor} | {args.Event} | {args.DragMode}");
			return base.OnMouse(element, args);
		}

		protected override bool OnKey(SciterElement element, KeyArgs args)
		{
			//Console.WriteLine($"{args.Event} | {args.KeyboardState} | {(char)args.KeyCode}");
			return base.OnKey(element, args);
		}

		protected override bool OnFocus(SciterElement element, FocusArgs args)
		{
			//Console.WriteLine($"{args.Event} | {args.Cancel} | {args.IsMouseClick}");
			return base.OnFocus(element, args);
		}

		protected override void Attached(SciterElement element)
		{
			_logger?.LogDebug($"{nameof(Attached)}");
			base.Attached(element);
		}

		protected override bool OnGesture(SciterElement element, GestureArgs args)
		{
			//Console.WriteLine($"{args}");
			//if (args.Event == GestureEvent.Request)
			//	return true;
			
			return base.OnGesture(element, args);
		}

		protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
		{
			_logger?.LogDebug($"{nameof(OnMethodCall)}: {nameof(methodId)}: {methodId}");
			return base.OnMethodCall(element, methodId);
		}

		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			_logger?.LogDebug($"{nameof(OnScriptCall)}: {nameof(method)}: {method.Name}");
			return base.OnScriptCall(element, method, args);
		}

        protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement,
			BehaviorEvents type, IntPtr reason, SciterValue data, string eventName)
		{
			if (type == BehaviorEvents.DocumentReady)
            {
                //Host.ConnectToInspector();
            }

			_logger?.LogDebug($"{nameof(OnEvent)}: {nameof(type)}: {type}; {nameof(eventName)}: {eventName}; {nameof(data)}: {data.AsString()}");
			return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
		}

		protected override bool OnDataArrived(SciterElement element, SciterBehaviors.DATA_ARRIVED_PARAMS prms)
		{
			_logger?.LogDebug($"{nameof(OnDataArrived)}: {nameof(prms)}: {prms.uri}");
			return base.OnDataArrived(element, prms);
		}
	}
}