using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;
using ICSharpCode.Decompiler.Metadata;
using SciterCore.ILSpy.EventHandlers;
using SciterCore.Interop;

namespace SciterCore.ILSpy
{
	public class Host : BaseHost
	{

        //readonly Dictionary<string, NamespaceTreeNode> namespaces = new Dictionary<string, NamespaceTreeNode>();
        //readonly Dictionary<TypeDefinitionHandle, TypeTreeNode> typeDict = new Dictionary<TypeDefinitionHandle, TypeTreeNode>();

        internal AssemblyListManager assemblyListManager;
        internal AssemblyList assemblyList;


        public Host(SciterWindow wnd)
		{
            Load();

			var host = this;
			host.Setup(wnd);

            //host.RegisterBehaviorHandler(typeof(DrawGeometryBehavior), "DrawGeometry")
            //    .AttachEventHandler(new ILSpyEventHandler(this));

			host.SetupPage(() =>
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
						if ((Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major > 6)
                            return "frame-windows.html";
                        else
                            return "frame-else.html";
                    case PlatformID.Unix:
						return "frame-else.html";
					case PlatformID.MacOSX:
						return "frame-mac.html";
					default:
                        throw new PlatformNotSupportedException();
                }
            });


            var folderElement = wnd.RootElement.SelectFirst("folder");

            foreach (var assembly in assemblyList.GetAssemblies())
            {
                var el = SciterElement.Create("option");
                el.SetAttribute("filename", assembly.FileName);

                //el.SetAttribute("version", assembly?.Version?.ToString());
                //el.SetAttribute("isAutoLoaded", assembly?.IsAutoLoaded); 
                el.SetState(SciterXDom.ELEMENT_STATE_BITS.STATE_COLLAPSED, 0, false);
                
                folderElement.Append(el);

                el.AttachEventHandler(new AssemblyEventHandler(null, assembly));
                
                //el.Append("text", assembly.Version.ToString());
            }

            wnd.Show();

            

            //dom::element num = root.find_first("input[type=number]");
        }

        private void Load()
        {
            var spySettings = ILSpySettings.Load();

            assemblyListManager = new AssemblyListManager(spySettings);

            //ILSpySettings spySettings = this.spySettingsForMainWindow_Loaded;
            //this.spySettingsForMainWindow_Loaded = null;
            var loadPreviousAssemblies = false;//Options.MiscSettingsPanel.CurrentMiscSettings.LoadPreviousAssemblies;

            //if (loadPreviousAssemblies)
            //{
            //    // Load AssemblyList only in Loaded event so that WPF is initialized before we start the CPU-heavy stuff.
            //    // This makes the UI come up a bit faster.
            //    this.assemblyList = assemblyListManager.LoadList(spySettings, sessionSettings.ActiveAssemblyList);
            //}
            //else
            //{
            this.assemblyList = new AssemblyList(AssemblyListManager.DefaultListName);
            assemblyListManager.ClearAll();
            //}

            //HandleCommandLineArguments(App.CommandLineArguments);

            //if (assemblyList.GetAssemblies().Length == 0
            //    && assemblyList.ListName == AssemblyListManager.DefaultListName
            //    && loadPreviousAssemblies)
            //{
            LoadInitialAssemblies();
            //}

            //ShowAssemblyList(this.assemblyList);

            //if (sessionSettings.ActiveAutoLoadedAssembly != null)
            //{
            //    this.assemblyList.Open(sessionSettings.ActiveAutoLoadedAssembly, true);
            //}

            //Dispatcher.UIThread.InvokeAsync(new Action(() => OpenAssemblies(spySettings)), DispatcherPriority.Loaded);
		}

        void LoadInitialAssemblies()
        {
            // Called when loading an empty assembly list; so that
            // the user can see something initially.
            System.Reflection.Assembly[] initialAssemblies = {
                typeof(object).Assembly,
                typeof(Uri).Assembly,
                typeof(System.Linq.Enumerable).Assembly,
                typeof(System.Xml.XmlDocument).Assembly,
            };
            foreach (System.Reflection.Assembly asm in initialAssemblies)
                assemblyList.OpenAssembly(asm.Location);
        }

		protected override void OnDataLoaded(SciterXDef.SCN_DATA_LOADED sdl)
        {
            base.OnDataLoaded(sdl);
        }
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
			_archive.Open("SiteResource");
		}

		public void Setup(SciterWindow window)
		{
			_window = window;
			SetupWindow(window);
		}

		public void SetupPage(Func<string> page)
		{
#if DEBUG
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if OSX
			location += "\\..\\..\\..\\..\\..\\..";
#else
			location += "\\..\\..\\..";
#endif

			string path = Path.Combine(location, "res", page.Invoke());
			Debug.Assert(File.Exists(path));

			Uri uri = new Uri(path, UriKind.Absolute);
#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
#endif

			_window.LoadPage(uri: uri);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith(_archive.Uri.AbsoluteUri))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
				byte[] data = _archive.Get(path);
				if(data!=null)
					_api.SciterDataReady(_window.Handle, sld.uri, data, (uint) data.Length);
			}

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sld);
		}
	}
}