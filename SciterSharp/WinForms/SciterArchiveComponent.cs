#if WINDOWS && NET45

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Archive = new SciterArchive();
        }

        public Uri Uri
        {
            get
            {
                return Archive.Uri;
            }

            set
            {
                //Archive.Uri = value;
            }
        }

    }
}

#endif
