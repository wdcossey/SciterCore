using System;

namespace SciterCore
{
    public class WindowLoadPageEventArgs : EventArgs
    {
        public Uri PageUri { get; set; }
    }
}