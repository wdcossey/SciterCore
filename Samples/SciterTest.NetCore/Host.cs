using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using SciterCore;
using SciterCore.Interop;
using SciterTest.NetCore.Behaviors;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore
{
	public class Host<TWindow> : BaseHost
		where TWindow : SciterWindow 
	{
		public static TWindow AppWindow { get; private set; }// must keep a reference to survive GC
		
		public Host(TWindow wnd)
		{
			AppWindow = wnd;
			var host = this;
			host.Setup(wnd);
			host.AttachEventHandler(new HostEvh());
			host.RegisterBehaviorHandler<DragDropBehavior>();
			host.SetupPage("index.html");
			
			wnd.Show();
		}
		
		public Host(Func<TWindow> wndFunc)
		: this(wndFunc.Invoke())
		{
			
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	public class HostEvh : SciterEventHandler
	{
		/// A dynamic script call handler. Any call in TIScript to function 'view.Host_HelloSciter()' with invoke this method
		/// Notice that signature of these handlers is always the same
		/// (Hint: install OmniCode snippets which adds the 'ssh' snippet to C# editor so you can easily declare 'Siter Handler' methods)
		/// (see: https://github.com/MISoftware/OmniCode-Snippets)
		public Task<SciterValue> HelloSciterCore(SciterElement element, SciterValue[] args)
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
			return Task.FromResult(value);
		}
		
		public Task<SciterValue> StackTrace(SciterElement element, SciterValue[] args)
		{
			var stackTrace = new StackTrace(true);
			var stackFrame = stackTrace.GetFrame(0);
			
			var value = SciterValue.FromJsonString(System.Text.Json.JsonSerializer.Serialize(new
			{
				MethodName = stackFrame?.GetMethod()?.Name,
				Parameters = stackFrame?.GetMethod()?.GetParameters().Select(s => new { s.Name, s.Position, Type = s.ParameterType.Name}),
				FileUri = new Uri(stackFrame?.GetFileName())?.AbsoluteUri,
				FileName = Path.GetFileName(stackFrame?.GetFileName()),
				LineNumber = stackFrame?.GetFileLineNumber(),
				ColumnNumber = stackFrame?.GetFileColumnNumber()
			}));
			
			return Task.FromResult(value);
		}
		
		public Task<SciterValue> GetRuntimeInfo(SciterElement element, SciterValue[] args)
		{
			//Simulate a delay
			//await Task.Delay(3500);

			var value = SciterValue.FromJsonString(System.Text.Json.JsonSerializer.Serialize(new
			{
				FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
				ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
				OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
				OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
				SystemVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion()
			}));
			
			return Task.FromResult(value);
		}
		
		public Task<SciterValue> ThrowException(SciterElement element, SciterValue[] args)
		{
			throw new NotImplementedException("This is a fault!");
		}

		protected override bool OnMouse(SciterElement element, MouseEventArgs args)
		{
			//Console.WriteLine($"{args.ButtonState} | {args.Event} | {args.DragMode}");
			return base.OnMouse(element, args);
		}

		protected override bool OnKey(SciterElement element, KeyEventArgs args)
		{
			//Console.WriteLine($"{args.Event} | {args.KeyboardState} | {(char)args.KeyCode}");
			return base.OnKey(element, args);
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
		protected static Sciter.SciterApi _api = Sciter.Api;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseHost()
		{
			_archive.Open();
		}

		public void Setup(SciterWindow window)
		{
			_window = window;
			SetupWindow(window);
		}

		public void SetupPage(string page)
		{
//#if DEBUG
//			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//
//#if OSX
//			location += "\\..\\..\\..\\..\\..\\..";
//#else
//			location += "\\..\\..\\..";
//#endif
//
//			string path = Path.Combine(location, "res", page);
//
//			Uri uri = new Uri(path, UriKind.Absolute);
//
//			Debug.Assert(uri.IsFile);
//
//			Debug.Assert(File.Exists(uri.AbsolutePath));
//#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
//#endif

			_window.LoadPage(uri: uri);
		}

		protected override LoadResult OnLoadData(object sender, LoadDataEventArgs args)
		{
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (data, path) => 
			{
				_api.SciterDataReady(_window.Handle, path, data, (uint) data.Length);
			});

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sender: sender, args: args);
		}

		protected override void OnEngineDestroyed(object sender, EngineDestroyedEventArgs args)
		{
			base.OnEngineDestroyed(sender, args);
		}

		protected override IntPtr OnPostedNotification(IntPtr wparam, IntPtr lparam)
		{
			return base.OnPostedNotification(wparam, lparam);
		}
	}
}