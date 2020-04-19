#if WINDOWS && NET45

using SciterCore.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciterCore.WinForms
{
    [DisplayName("Host")]
    [DesignerCategory("Sciter")]
    [Category("Sciter")]
    public class SciterHostComponent : Component
    {
        internal InternalHost Host { get; }

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
                    Host.SetArchive(value?.Archive);
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
        //            Host.SetWindow(value?.SciterWnd);
        //        }
        //    }
        //}

        public SciterHostComponent()
        {
            Host = new InternalHost();
        }
    }

    class InternalHost : SciterHost
    {
        protected static Sciter.SciterApi _api = Sciter.Api;
        private SciterArchive _archive;

        internal InternalHost()
        {

        }

        internal InternalHost SetArchive(SciterArchive archive)
        {
            _archive = archive;
            return this;
        }
        
        internal InternalHost SetWindow(SciterWindow window)
        {
            this.SetupWindow(window: window);
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

#endif