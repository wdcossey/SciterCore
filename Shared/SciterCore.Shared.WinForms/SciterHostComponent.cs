using System;
using SciterCore.Interop;
using System.ComponentModel;

namespace SciterCore.WinForms
{
    [DisplayName("FormsHost")]
    [DesignerCategory("Sciter")]
    [Category("Sciter")]
    public class SciterHostComponent : Component
    {
        internal SciterFormsHost FormsHost { get; }

        private SciterArchiveComponent _archive;
        private SciterControl _control;
        
        [Category("Sciter")]
        public string RootPage { get; set; }
        
        [Category("Sciter")]
        public event EventHandler<GetArchiveItemEventArgs> GetArchiveItem;
        
        public SciterArchiveComponent Archive
        { 
            get => _archive;
            set
            {
                _archive = value;

                if (value != null)
                {
                    FormsHost.SetArchive(value?.Archive);
                }
            }
        }
        
        public SciterControl Control
        {
            get => _control;
            set
            {
                _control = value;

                if (_control == null) 
                    return;
                
                if (_control.SciterWnd != null && _control.SciterWnd?.Handle != IntPtr.Zero)
                {
                    OnWindowCreated(_control, new WindowCreatedEventArgs(_control.SciterWnd));
                }
                
                _control.WindowCreated += OnWindowCreated;
            }
        }

        private void OnWindowCreated(object sender, WindowCreatedEventArgs e)
        {
            this.FormsHost.SetWindow(e.Window);
            
            e.Window.LoadPage(uri: new Uri(baseUri: _archive.BaseAddress, RootPage ?? ""));

        }

        public SciterHostComponent()
        {
            FormsHost = new SciterFormsHost();
            
            FormsHost.InternalGetItem += FormsHostOnInternalGetItem;
        }

        private void FormsHostOnInternalGetItem(object sender, InternalGetArchiveItemEventArgs e)
        {
            var args = new GetArchiveItemEventArgs(_archive.BaseAddress.AbsoluteUri, e.Uri);
            GetArchiveItem?.Invoke(this, args);
            e.Uri = args.Path;
        }
        
    }

    internal class SciterFormsHost : SciterHost
    {
        protected static Sciter.SciterApi _api = Sciter.Api;
        private SciterArchive _archive;

        internal event EventHandler<InternalGetArchiveItemEventArgs> InternalGetItem;

        internal SciterFormsHost()
        {
            
        }

        internal SciterFormsHost SetArchive(SciterArchive archive)
        {
            _archive = archive;
            _archive.Open();
            return this;
        }
        
        internal SciterFormsHost SetWindow(SciterWindow window)
        {
            base.SetupWindow(window: window);
            return this;
        }

        protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
        {
            var args = new InternalGetArchiveItemEventArgs(sld.uri);
                
            InternalGetItem?.Invoke(this, args);
            
            // load resource from SciterArchive
            _archive?.GetItem(uriString: args.Uri, onFound: (data, path) =>
            {
               _api.SciterDataReady(WindowHandle, path, data, (uint)data.Length);
            });

            return base.OnLoadData(sld);
        }
    }
    
    internal class InternalGetArchiveItemEventArgs : EventArgs
    {
        public string Uri { get; set; }

        public InternalGetArchiveItemEventArgs(string uri)
        {
            Uri = uri;
        }
    }

    public class GetArchiveItemEventArgs : EventArgs
    {
        public string BaseAddress { get; set; }
        public string Path { get; set; }

        public GetArchiveItemEventArgs(string baseAddress, string path)
        {
            BaseAddress = baseAddress;
            Path = path;
        }
    }
}