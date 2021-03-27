using System;
using SciterCore.Enums;

namespace SciterCore.Options
{
    public class SciterWindowOptions
    {
        #region Internal properties

        internal Type WindowType { get; set; } = typeof(SciterWindow);
        
        internal string Title { get; set; }

        internal int Width { get; set; }
        
        internal int Height { get; set; }
        
        internal SciterWindowPosition Position { get; set; }

        #endregion
        
        #region Public methods
        
        public SciterWindowOptions SetTitle(string title)
        {
            Title = title;
            return this;
        }
        
        public SciterWindowOptions SetDimensions(int width, int height)
        {
            Width = width;
            Height = height;
            return this;
        }
        
        public SciterWindowOptions SetPosition(SciterWindowPosition position)
        {
            Position = position;
            return this;
        }
        
        #endregion
    }
    
    public class SciterWindowOptions<T> : SciterWindowOptions
        where T : SciterWindow
    {

        #region Contrustor(s)

        public SciterWindowOptions()
        {
            WindowType = typeof(T);
        }

        #endregion
        
        #region Public methods
        
        public new SciterWindowOptions<T> SetTitle(string title)
        {
            Title = title;
            return this;
        }
        
        public new SciterWindowOptions<T> SetDimensions(int width, int height)
        {
            Width = width;
            Height = height;
            return this;
        }
        
        #endregion
    }
}