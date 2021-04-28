using System;
using SciterCore.Enums;

namespace SciterCore.Options
{
    public class SciterWindowOptions
    {
        private readonly IServiceProvider _serviceProvider;

        #region Constructor(s)
        
        internal SciterWindowOptions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        #endregion
        
        #region Internal properties

        internal Type WindowType { get; set; } = typeof(SciterWindow);
        
        internal string Title { get; set; }

        internal int? Width { get; set; }
        
        internal int? Height { get; set; }
        
        internal SciterWindowPosition Position { get; set; }

        #endregion
        
        #region Public methods

        #region SetTitle

        public SciterWindowOptions SetTitle(string title)
        {
            Title = title;
            return this;
        }
        
        public SciterWindowOptions SetTitle(Func<string> titleFunc)
        {
            if (titleFunc == null)
                throw new ArgumentNullException(nameof(titleFunc));
            
            return SetTitle(titleFunc());
        }

        public SciterWindowOptions SetTitle(Func<IServiceProvider, string> titleFunc)
        {
            if (titleFunc == null)
                throw new ArgumentNullException(nameof(titleFunc));
            
            return SetTitle(titleFunc(_serviceProvider));
        }
        
        #endregion

        #region SetDimensions

        public SciterWindowOptions SetDimensions(int width, int height)
        {
            Width = width;
            Height = height;
            return this;
        }

        #endregion

        #region SetPosition

        public SciterWindowOptions SetPosition(SciterWindowPosition position)
        {
            Position = position;
            return this;
        }
        
        public SciterWindowOptions SetPosition(Func<SciterWindowPosition> positionFunc)
        {
            if (positionFunc == null)
                throw new ArgumentNullException(nameof(positionFunc));
            
            return SetPosition(positionFunc());
        }
        
        public SciterWindowOptions SetPosition(Func<IServiceProvider, SciterWindowPosition> positionFunc)
        {
            if (positionFunc == null)
                throw new ArgumentNullException(nameof(positionFunc));
            
            return SetPosition(positionFunc(_serviceProvider));
        }

        #endregion

        #endregion
    }
    
    public class SciterWindowOptions<T> : SciterWindowOptions
        where T : SciterWindow
    {
        #region Constructor(s)

        internal SciterWindowOptions(IServiceProvider serviceProvider)
            : base(serviceProvider: serviceProvider)
        {
            WindowType = typeof(T);
        }

        #endregion
        
        #region Public methods
        
        #region SetTitle
        
        public new SciterWindowOptions<T> SetTitle(string title)
        {
            base.SetTitle(title: title);
            return this;
        }
        
        public new SciterWindowOptions<T> SetTitle(Func<string> titleFunc)
        {
            base.SetTitle(titleFunc: titleFunc);
            return this;
        }
        
        public new SciterWindowOptions<T> SetTitle(Func<IServiceProvider, string> titleFunc)
        {
            base.SetTitle(titleFunc: titleFunc);
            return this;
        }
        
        #endregion

        #region SetDimensions

        public new SciterWindowOptions<T> SetDimensions(int width, int height)
        {
            base.SetDimensions(width: width, height: height);
            return this;
        }

        #endregion

        #region SetPosition
        
        public new SciterWindowOptions<T> SetPosition(SciterWindowPosition position)
        {
            base.SetPosition(position: position);
            return this;
        }
        
        public new SciterWindowOptions<T> SetPosition(Func<SciterWindowPosition> positionFunc)
        {
            base.SetPosition(positionFunc: positionFunc);
            return this;
        }
        
        public new SciterWindowOptions<T> SetPosition(Func<IServiceProvider, SciterWindowPosition> positionFunc)
        {
            base.SetPosition(positionFunc: positionFunc);
            return this;
        }
        
        #endregion

        #endregion
    }
}