using SciterCore.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace SciterCore
{
    public class SciterApplication : IDisposable
    {
        private static object _lock = new object();

        private SciterWindow Window { get; set; }

        private SciterHost Host { get; set; }

        public SciterApplication()
        {

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

            return Run(() => {
#if !OSX
                PInvokeUtils.RunMsgLoop();
#endif
            });

            //return Run(() => { host.Show(); });
            //window.Show();
            //NativeMethods.Main();
            //return 0;
        }

        protected int Run(Action action)
        {
            try
            {
                QueueMain(action);
                //NativeMethods.Main();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }
        
        public static void QueueMain(Action action)
        {
            lock (_lock)
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
