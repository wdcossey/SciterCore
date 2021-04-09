using System;

namespace SciterCore
{
    public class WindowLoadHtmlEventArgs: EventArgs
    {
        public string Html { get; set; }
        
        public string BaseUrl { get; set; }
    }
}