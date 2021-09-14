using SciterCore.Interop;
using SciterCore.PlatformWrappers;

namespace SciterCore
{
    /// <summary>
    /// var x = new SciterWindowBuilder()
    ///     //.DefaultWindowFlags()
    ///     .AsMain()
    ///     .AsResizeable()
    ///     .WithGlassy()
    ///     .WithDebug()
    ///     //.AddWindowFlags(CreateWindowFlags.EnableDebug)
    ///     .WithSize(800, 600)
    ///     .WithPosition(100, 200)
    ///     //.Centered()
    ///     .Build().Show();
    /// </summary>
    public class SciterWindowBuilder
    {
        private static readonly ISciterWindowWrapper WindowWrapper = SciterWindowWrapper.NativeMethodWrapper.GetInterface();

        internal const CreateWindowFlags DefaultCreateWindowFlags =
            CreateWindowFlags.Main |
            CreateWindowFlags.Titlebar |
            CreateWindowFlags.Resizeable |
            CreateWindowFlags.Controls |
            CreateWindowFlags.Glassy;

        internal const SciterXDef.SCRIPT_RUNTIME_FEATURES DefaultRuntimeFeatures =
            SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_EVAL |
            SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_FILE_IO |
            SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SOCKET_IO |
            SciterXDef.SCRIPT_RUNTIME_FEATURES.ALLOW_SYSINFO;

        private bool _centered;
        private SciterPoint? _position = null;
        private SciterRectangle? _withSize;
        private CreateWindowFlags _flags;

        public SciterWindow Build()
        {
            var windowHandle = WindowWrapper.CreateWindow(_withSize ?? new SciterRectangle(), _flags, null);

            if (_centered)
            {
#if OSX && XAMARIN
			    _nsview.Window.Center();
#else
                WindowWrapper.CenterWindow(windowHandle);
#endif
            }

            return new SciterWindow(windowHandle);
        }

        //public SciterWindow CreateWindow(SciterRectangle frame = new SciterRectangle(), SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags, IntPtr parent = new IntPtr())
        //public SciterWindow CreateMainWindow(int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
        //public SciterWindow CreateMainWindow(int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)
        //public SciterWindow CreateOwnedWindow(IntPtr owner, int width, int height, SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = DefaultCreateFlags)

        /*/// <summary>
        /// Sets the default flags of the <see cref="SciterWindow"/>
        /// <para>Main, TitleBar, Resizeable, Controls, Glassy</para>
        /// </summary>
        public SciterWindowBuilder DefaultWindowFlags()
        {
            _flags = DefaultCreateWindowFlags;
            return this;
        }

        /// <summary>
        /// Adds the specified <paramref name="flags"></paramref>
        /// </summary>
        public SciterWindowBuilder AddWindowFlags(CreateWindowFlags flags)
        {
            _flags |= flags;
            return this;
        }

        /// <summary>
        /// Removes the specified <paramref name="flags"></paramref>
        /// </summary>
        public SciterWindowBuilder RemoveWindowFlags(CreateWindowFlags flags)
        {
            _flags &= ~flags;
            return this;
        }*/

        /// <summary>
        ///
        /// </summary>
        public SciterWindowBuilder AsMain()
        {
            /*			CreateWindowFlags.Main |
			CreateWindowFlags.Titlebar |
			CreateWindowFlags.Resizeable |
			CreateWindowFlags.Controls |
			CreateWindowFlags.Glassy;*/
            _flags |= CreateWindowFlags.Main | CreateWindowFlags.Titlebar | CreateWindowFlags.Controls;
            _flags &= ~CreateWindowFlags.Child;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        public SciterWindowBuilder AsChild()
        {
            _flags |= CreateWindowFlags.Child | CreateWindowFlags.Titlebar | CreateWindowFlags.Controls;
            _flags &= ~CreateWindowFlags.Main;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        public SciterWindowBuilder WithGlassy()
        {
            _flags |= CreateWindowFlags.Glassy;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        public SciterWindowBuilder WithControls()
        {
            _flags |= CreateWindowFlags.Controls;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        public SciterWindowBuilder AsResizeable()
        {
            _flags |= CreateWindowFlags.Resizeable;
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        public SciterWindowBuilder WithTitlebar()
        {
            _flags |= CreateWindowFlags.Titlebar;
            return this;
        }

        /// <summary>
        /// Make the window `Inspector Ready`
        /// </summary>
        public SciterWindowBuilder WithDebug()
        {
            _flags |= CreateWindowFlags.EnableDebug;
            return this;
        }

        /// <summary>
        /// Sets the initial dimensions of the <see cref="SciterWindow"/>
        /// </summary>
        public SciterWindowBuilder WithSize(SciterRectangle rectangle)
        {
            _withSize = rectangle;
            return this;
        }

        /// <summary>
        /// Sets the initial dimensions of the <see cref="SciterWindow"/>
        /// </summary>
        public SciterWindowBuilder WithSize(int width, int height)
            => WithSize(new SciterRectangle(right: width, bottom:  height));

        /// <summary>
        /// Sets the initial dimensions of the <see cref="SciterWindow"/>
        /// </summary>
        public SciterWindowBuilder WithSize(int left, int top, int width, int height)
            => WithSize(new SciterRectangle(left: left, top: top, right: width, bottom: height));

        /// <summary>
        /// Centers the <see cref="SciterWindow"/> in the screen.
        /// </summary>
        public SciterWindowBuilder Centered()
        {
            _centered = true;
            return this;
        }

        /// <summary>
        /// Sets the position of the <see cref="SciterWindow"/> on the Screen
        /// </summary>
        public SciterWindowBuilder WithPosition(SciterPoint point)
        {
            _position = point;
            return this;
        }

        /// <summary>
        /// Sets the position of the <see cref="SciterWindow"/> on the Screen
        /// </summary>
        public SciterWindowBuilder WithPosition(int left, int top)
            => WithPosition(new SciterPoint(left, top));

    }
}