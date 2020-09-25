using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterTest.NetCore.Behaviors;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore
{
	public class ApplicationHost<TWindow> : BaseHost
		where TWindow : SciterWindow 
	{
		public ApplicationHost() 
			: this(Activator.CreateInstance<TWindow>())
		{
			 
		}
		
		public ApplicationHost(TWindow wnd)
			: base(wnd)
		{
			var host = this;
			host
				.AttachEventHandler(() => new HostEventHandler(host))
				.RegisterBehaviorHandler<DragDropBehavior>();
			
			host.LoadPage("index.html", 
				(sciterHost, window) =>
				{
					window.Show();
				},
				(sciterHost, window) => throw new InvalidOperationException("Unable to load the requested page."));

#if DEBUG
			host.ConnectToInspector();
#endif
		}
		
		public ApplicationHost(Func<TWindow> wndFunc)
		: this(wndFunc.Invoke())
		{
			
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	public class HostEventHandler : SciterEventHandler
	{
		private readonly SciterHost _host;

		public HostEventHandler(SciterHost host)
		{
			_host = host;
		}
		
		/// A dynamic script call handler. Any call in TIScript to function 'view.Host_HelloSciter()' with invoke this method
		/// Notice that signature of these handlers is always the same
		/// (Hint: install OmniCode snippets which adds the 'ssh' snippet to C# editor so you can easily declare 'Siter Handler' methods)
		/// (see: https://github.com/MISoftware/OmniCode-Snippets)
		public Task HelloSciterCore(SciterElement element, SciterValue onCompleted)
		{
			var stackTrace = new StackTrace(true);
			var stackFrame = stackTrace.GetFrame(0);
				
			var value = SciterValue.Create(System.Text.Json.JsonSerializer.Serialize(new
			{
				MethodName = stackFrame?.GetMethod()?.Name,
				FileUri = new Uri(stackFrame?.GetFileName())?.AbsoluteUri,
				FileName = Path.GetFileName(stackFrame?.GetFileName()),
				LineNumber = stackFrame?.GetFileLineNumber(),
				ColumnNumber = stackFrame?.GetFileColumnNumber()
			}, options: new JsonSerializerOptions() { WriteIndented = true }));
			
			//value = SciterValue.Create($"<h2>Hello Sciter from C# in .Net Core!</h2><code>Method: {stackFrame?.GetMethod()?.Name}<br/>File: <a href=\"{new Uri(stackFrame?.GetFileName())?.AbsoluteUri}\">{Path.GetFileName(stackFrame?.GetFileName())}</a><br/>Line: {stackFrame?.GetFileLineNumber()}<br/>Column: {stackFrame?.GetFileColumnNumber()}</code>");

			onCompleted.Invoke(value);
			
			return Task.CompletedTask;
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
			//Simulate a delay
			//await Task.Delay(3500);

			try
			{
				var value = SciterValue.Create(
					new {
						FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
						ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
						OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
						OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
						SystemVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion()
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
			//This will purposely throw an exception!
			var value = numerator.AsInt32() / denominator.AsInt32();
				
			return Task.FromResult(SciterValue.Create(value));
		}

		public void SynchronousFunction()
		{
			//SciterValue.Create(value: (100 / int.Parse("0")));
			Console.WriteLine($"{nameof(SynchronousFunction)} was executed!");
		}

		public void SynchronousArgumentFunction(SciterElement element, SciterValue[] args)
		{
			Console.WriteLine($"{nameof(SynchronousArgumentFunction)} was executed!\n\t{string.Join(",", args.Select(s => s.AsInt32()))}");
		}

		public void SynchronousArgumentsFunction(SciterElement element, SciterValue arg1, SciterValue arg2, SciterValue arg3, SciterValue arg4, SciterValue arg5)
		{
			Console.WriteLine($"{nameof(SynchronousArgumentsFunction)} was executed!\n\t{arg1.AsInt32()},{arg2.AsInt32()},{arg3.AsInt32()},{arg4.AsInt32()},{arg5.AsInt32()}");
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
			Console.WriteLine($"{nameof(AsynchronousFunction)} was executed!");
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
			return base.OnMethodCall(element, methodId);
		}

		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			return base.OnScriptCall(element, method, args);
		}

		protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement,
			SciterBehaviors.BEHAVIOR_EVENTS type, IntPtr reason, SciterValue data, string eventName)
		{
			return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
		}

		// (Hint: to overload C# methods of SciterEventHandler base class, type 'override', press space, and VS/Xamarin will suggest the methods you can override)
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.rs/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	public class BaseHost : SciterHost
	{
		private readonly Sciter.SciterApi _api = Sciter.Api;
		private readonly SciterArchive _archive = new SciterArchive();


		public BaseHost(SciterWindow window)
			: base(window)
		{
			Window = window;
			_archive.Open();
		}

		protected SciterWindow Window { get; }

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
			return base.OnAttachBehavior(element, behaviorName, out eventHandler);
		}

		protected override void OnDataLoaded(object sender, DataLoadedArgs args)
		{
			base.OnDataLoaded(sender, args);
		}

		protected override void OnEngineDestroyed(object sender, EngineDestroyedArgs args)
		{
			base.OnEngineDestroyed(sender, args);
		}

		protected override IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
		{
			return base.OnPostedNotification(wparam, lparam);
		}
	}
}