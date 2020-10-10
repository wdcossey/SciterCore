using SciterCore.Interop;
using System;

namespace SciterCore
{
    public class SciterApplication : IDisposable
    {
        private static readonly object QueueLock = new object();

        private SciterWindow Window { get; set; }

        public SciterHost Host { get; internal set; }
        
        public SciterApplication()
        {

        }

        public SciterApplication(SciterHost host)
        {
            Host = host;
        }

        //public int Run<TWindow>(TWindow window)
        //    where TWindow: SciterWindow
        //{
        //    Window = window;
        //    return Run(() => { window.Show(); });
        //    //window.Show();
        //    //NativeMethods.Main();
        //    //return 0;
        //}

        public int Run<THost>(THost host)
            where THost: SciterHost
        {
            Host = host;

            return Run(PInvokeUtils.RunMsgLoop);

            //return Run(() => { host.Show(); });
            //window.Show();
            //NativeMethods.Main();
            //return 0;
        }

        public int Run()
        {
            return Run(PInvokeUtils.RunMsgLoop);
        }
        

        public int Run<THost>()
            where THost: SciterHost
        {
            return Run(Activator.CreateInstance<THost>());
        }

        public int Run<THost>(Func<THost> hostFunc)
            where THost: SciterHost
        {
            return Run(hostFunc.Invoke());
        }

        protected int Run(Action action)
        {
            try
            {
                Host.Window?.Show();
                QueueMain(action);
                //NativeMethods.Main();
            }
            catch
            {
                return -1;
            }
            return 0;
        }
        
        public static void QueueMain(Action action)
        {
            lock (QueueLock)
            {
                action?.Invoke();
            }
        }

        public void Dispose()
        {
            //
        }
    }
}
