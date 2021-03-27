namespace SciterCore
{
    public struct HostCreatedEventArgs
    {
        internal HostCreatedEventArgs(SciterWindow window)
        {
            Window = window;
        }
        
        public SciterWindow Window { get; internal set; } 
    }
}