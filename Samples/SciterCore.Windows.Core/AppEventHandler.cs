namespace SciterCore.Windows.Core
{
    class AppEventHandler : SciterEventHandler
    {
        public bool Host_HelloWorld(SciterElement element, SciterValue[] @params, out SciterValue result)
        {
            result = new SciterValue(args => SciterValue.Undefined);
            return true;
        }

        public bool Host_DoSomething(SciterElement element, SciterValue[] @params, out SciterValue result)
        {
            result = null;
            return true;
        }
    }
}