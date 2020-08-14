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

        public SciterArchiveComponent Archive
        { 
            get
            {
                return _archive;
            }
            set
            {
                _archive = value;

                if (value != null)
                {
                    FormsHost.SetArchive(value?.Archive);
                }
            }
        }

        //public SciterControl Control
        //{ 
        //    get
        //    {
        //        return _control;
        //    }
        //    set
        //    {
        //        _control = value;

        //        if (value != null)
        //        {
        //            FormsHost.SetWindow(value?.SciterWnd);
        //        }
        //    }
        //}

        public SciterHostComponent()
        {
            FormsHost = new SciterFormsHost();
        }
    }

    internal class SciterFormsHost : SciterHost
    {
        protected static Sciter.SciterApi _api = Sciter.Api;
        private SciterArchive _archive;

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
            // load resource from SciterArchive
            _archive?.GetItem(uriString: sld.uri, onFound: (data, path) =>
            {
               _api.SciterDataReady(WindowHandle, path, data, (uint)data.Length);
            });

            return base.OnLoadData(sld);
        }
    }

}