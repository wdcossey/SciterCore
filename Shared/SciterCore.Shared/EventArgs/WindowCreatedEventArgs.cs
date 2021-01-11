namespace SciterCore
{
    public struct WindowCreatedEventArgs
    {

        internal WindowCreatedEventArgs(SciterWindow window)
        {
            Window = window;
        }
        
        public SciterWindow Window { get; internal set; } 
    }
}