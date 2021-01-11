using System;
using SciterCore.Interop;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace SciterCore.WinForms
{

    public class SciterFormsEventHandler : SciterEventHandler
    {
        internal event EventHandler<ScriptCallEventArgs> InternalScriptCall;
        
        protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, BehaviorEvents eventType, IntPtr reason,
            SciterValue data, string eventName)
        {
            return base.OnEvent(sourceElement, targetElement, eventType, reason, data, eventName);
        }

        protected override bool OnMethodCall(SciterElement element, SciterBehaviors.BEHAVIOR_METHOD_IDENTIFIERS methodId)
        {
            return base.OnMethodCall(element, methodId);
        }

        protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
        {
            return base.OnScriptCall(element, method, args);
        }

        protected override ScriptEventResult OnScriptCall(SciterElement element, string methodName, SciterValue[] args)
        {
            var eventArgs = new ScriptCallEventArgs(element, methodName, args);
            InternalScriptCall?.Invoke(this, eventArgs);

            var methodOwner = eventArgs.Owner ?? this;
            
            return ScriptExecutioner
                .Create(eventArgs.Owner ?? this, element, eventArgs.Method ?? methodOwner.GetType().GetMethod(eventArgs.MethodName), args)
                .Execute();
        }
    }
    
    [DisplayName("FormsHost")]
    [DesignerCategory("Sciter")]
    [Category("Sciter")]
    public class SciterHostComponent : Component
    {
        internal SciterFormsHost FormsHost { get; }

        private SciterArchiveComponent _archive;
        private SciterControl _control;
        
        [Category("Sciter")]
        public string RootPage { get; set; }
        
        [Category("Sciter")]
        public event EventHandler<GetArchiveItemEventArgs> GetArchiveItem;
        
        [Category("Sciter")]
        public event EventHandler<ScriptCallEventArgs> OnScriptCall;
        
        public SciterArchiveComponent Archive
        { 
            get => _archive;
            set
            {
                _archive = value;

                if (this.DesignMode)
                    return;
                
                FormsHost?.SetArchive(value?.Archive);
            }
        }
        
        public SciterControl Control
        {
            get => _control;
            set
            {
                _control = value;

                if (_control == null) 
                    return;
                
                if (this.DesignMode)
                    return;
                
                if (_control.SciterWindow != null && _control.SciterWindow?.Handle != IntPtr.Zero)
                {
                    OnWindowCreated(_control, new WindowCreatedEventArgs(_control.SciterWindow));
                }
                
                _control.WindowCreated += OnWindowCreated;
            }
        }

        private void OnWindowCreated(object sender, WindowCreatedEventArgs e)
        {
            if (this.DesignMode)
                return;
            
            this.FormsHost?.SetWindow(e.Window);

            var winFormsEventHandler = new SciterFormsEventHandler();
            winFormsEventHandler.InternalScriptCall += (o, args) =>
            {
                OnScriptCall?.Invoke(this, args);
            };
            
            winFormsEventHandler.SetHost(FormsHost);
                
            this.FormsHost?.AttachEventHandler(winFormsEventHandler);
            
            e.Window.LoadPage(uri: new Uri(baseUri: new Uri(_archive.BaseAddress), RootPage ?? ""));

        }

        public SciterHostComponent()
        {
            if (this.DesignMode)
                return;
            
            FormsHost = new SciterFormsHost();
            
            FormsHost.InternalGetItem += FormsHostOnInternalGetItem;
        }

        private void FormsHostOnInternalGetItem(object sender, InternalGetArchiveItemEventArgs e)
        {
            if (this.DesignMode)
                return;
            
            var args = new GetArchiveItemEventArgs(_archive.BaseAddress, e.Uri);
            GetArchiveItem?.Invoke(this, args);
            e.Uri = args.Path;
        }
        
    }

    internal class SciterFormsHost : SciterHost
    {
        protected static ISciterApi _api = Sciter.SciterApi;
        private SciterArchive _archive;

        internal event EventHandler<InternalGetArchiveItemEventArgs> InternalGetItem;

        internal SciterFormsHost()
        {
            
        }

        internal SciterFormsHost SetArchive(SciterArchive archive)
        {
            _archive = archive;
            _archive?.Open();
            return this;
        }
        
        internal SciterFormsHost SetWindow(SciterWindow window)
        {
            base.SetupWindow(window: window);
            return this;
        }

        protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
        {
            var intArgs = new InternalGetArchiveItemEventArgs(args.Uri);
                
            InternalGetItem?.Invoke(this, intArgs);
            
            // load resource from SciterArchive
            _archive?.GetItem(uri: args.Uri, onGetResult: (result) =>
            {
                if (result.IsSuccessful)
                    _api.SciterDataReady(WindowHandle, result.Path, result.Data, (uint)result.Size);
            });

            return base.OnLoadData(sender: sender, args: args);
        }

    }
    
    internal class InternalGetArchiveItemEventArgs : EventArgs
    {
        public Uri Uri { get; set; }

        public InternalGetArchiveItemEventArgs(Uri uri)
        {
            Uri = uri;
        }
    }

    public class GetArchiveItemEventArgs : EventArgs
    {
        public string BaseAddress { get; set; }
        public Uri Path { get; set; }

        public GetArchiveItemEventArgs(string baseAddress, Uri path)
        {
            BaseAddress = baseAddress;
            Path = path;
        }
    }
    
    public class ScriptCallEventArgs : EventArgs
    {
        public object Owner { get; set; }
        
        public MethodInfo Method { get; set; }
        
        public SciterElement Element { get; }
        
        public string MethodName { get; }
        
        public SciterValue[] Args { get; }
        
        public ScriptCallEventArgs(SciterElement element, string methodName, SciterValue[] args)
        {
            Element = element;
            MethodName = methodName;
            Args = args;
        }
    }
}