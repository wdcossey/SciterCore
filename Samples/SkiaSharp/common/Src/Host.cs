using System;
using System.Diagnostics;
using System.IO;
using SciterCore;
using SciterCore.Interop;
using SciterGraphics = SciterCore.Interop.SciterGraphics;
using SciterValue = SciterCore.SciterValue;
using System.Reflection;
using SciterTest.Graphics.Behaviors;

namespace SciterTest.Graphics
{
	class Host : BaseHost
	{

		public Host(SciterWindow window)
			: base(window)
		{
			this.RegisterBehaviorHandler<CheckeredBackgroundBitmapBehavior>()
			
			.RegisterBehaviorHandler<InfoBitmapBehavior>()

			.RegisterBehaviorHandler<SolidBitmapBehavior>()
			.RegisterBehaviorHandler<SolidForegroundBitmapBehavior>()

			.RegisterBehaviorHandler<LinearBitmapBehavior>()
			.RegisterBehaviorHandler<LinearForegroundBitmapBehavior>()

			.RegisterBehaviorHandler<RadialBitmapBehavior>()
			.RegisterBehaviorHandler<RadialForegroundBitmapBehavior>()

			.RegisterBehaviorHandler<DrawTextBehavior>()
			.RegisterBehaviorHandler<DrawGeometryBehavior>()

			.AttachEventHandler(new HostEventHandler());

			SetupPage("index.html");

			window.Show();
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	class HostEventHandler : SciterEventHandler
	{
		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			var r = SciterImage.Create(args[0]);
			var b = r.Save(ImageEncoding.Png);
			File.WriteAllBytes("d:/test.png", b);
            
			return ScriptEventResult.Successful();
		}
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.rs/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	class BaseHost : SciterHost
	{
		protected static Sciter.SciterApi _api = Sciter.Api;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseHost(SciterWindow window)
			: base(window)
		{
			_window = window;
			_archive.Open();
		}

		public void SetupPage(string page)
		{
			var uri = new Uri(baseUri: _archive.Uri, page);
			_window.LoadPage(uri: uri);
		}

		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (data, path) => 
			{ 
				_api.SciterDataReady(_window.Handle, path, data, (uint) data.Length);
			});

			return base.OnLoadData(sender: sender, args: args);
		}
	}
}