using System;
using System.ComponentModel;

namespace SciterCore.WinForms
{
    [DisplayName("Archive")]
    [DesignerCategory("Sciter")]
    [Category("Sciter")]
    public class SciterArchiveComponent : Component
    {
        internal SciterArchive Archive { get; }

        //private SciterControl _control;

        //public SciterControl Window 
        //{ 
        //    get
        //    {
        //        return _control;
        //    }
        //    set
        //    {
        //        _control = value;

        //        if (value?.SciterWnd != null)
        //        {
        //            Host.SetupWindow(window: _control?.SciterWnd);
        //        }
        //    }
        //}

        public SciterArchiveComponent()
        {
            if (this.DesignMode)
                return;
            
            Archive = new SciterArchive();
        }

        public string BaseAddress
        {
            get
            {
                return Archive?.Uri?.AbsoluteUri ?? SciterArchive.DEFAULT_ARCHIVE_URI;
            }
            set
            {
                //Archive.Uri = value;
            }
        }

    }
}
